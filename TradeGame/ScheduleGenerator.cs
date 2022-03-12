namespace TradeGame
{
    internal class ScheduleGenerator
    {
        private IReader reader;
        private IWriter writer;
        private ICalculator calculator;

        public ScheduleGenerator(IReader reader, IWriter writer, ICalculator calculator)
        {
            this.reader = reader;
            this.writer = writer;
            this.calculator = calculator;
        }

        public void GameScheduler()
        {
            // read input
            ReadFiles();

            // designate self
            Country self = Global.InitialState.Where(x => x.Name.Equals("gaia")).FirstOrDefault();
            self.IsSelf = true;

            // execute search
            Node startNode = new Node()
            {
                State = Global.InitialState
            };
            Search(startNode);

            // write results
            writer.WriteSchedules();
        }

        public void ReadFiles()
        {
            reader.ReadResources(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "resource-input.csv"));
            reader.ReadTransformTemplates(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "transform-template-input.json"));
            reader.ReadCountries(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "country-input.csv"));
        }

        public void Search(Node startNode, int depthBound = 3)
        {
            // C# priority queue defaults to dequeuing lowest scores first
            // override this with a custom comparer to dequeue highest scores first
            ScheduleComparer customComparer = new ScheduleComparer();
            PriorityQueue<Node, double> frontier = new PriorityQueue<Node, double>(customComparer);

            // add initial state to frontier
            frontier.Enqueue(startNode, 0.0);

            while (frontier.Count > 0)
            {
                Node currentNode = frontier.Dequeue();
                if (currentNode.Depth >= depthBound)
                {
                    double currentExpectedUtility = calculator.CalculateExpectedUtility(currentNode.Schedule, startNode.State, currentNode.State);
                    Global.Solutions.Enqueue(new Schedule()
                    {
                        Steps = currentNode.Schedule.Steps,
                        ExpectedUtility = currentExpectedUtility
                    }, currentExpectedUtility);
                }
                else
                {
                    foreach (Node successor in GenerateSuccessors(currentNode))
                    {
                        double successorExpectedUtility = calculator.CalculateExpectedUtility(successor.Schedule, startNode.State, successor.State);
                        frontier.Enqueue(successor, successorExpectedUtility);
                        //UpdateFrontier(frontier, successor, successorExpectedUtility);
                    }
                }
            }
        }

        public IList<Node> GenerateSuccessors(Node currentNode)
        {
            IList<Node> successors = new List<Node>();
            Country self = currentNode.State.Where(c => c.IsSelf).FirstOrDefault();

            // Transfer Sequence:
            // 1. A country transfers resources to self.
            // 2. Self uses those resources in a transform.
            // 3. Self sends half of the transform outputs back to country from Step 1.
            if (DetermineTransferSequence(currentNode, self, out Action oneStepBefore, 
                    out Action twoStepsBefore))
            {
                // if previous action was transfer to self, we are in a transfer sequence
                if (oneStepBefore.Type.Equals("TransferTemplate"))
                {
                    TransferTemplate transfer = (TransferTemplate)oneStepBefore;
                    // Transfer sequences begin with transfer to self
                    if (transfer.ReceivingCountry.Equals(self.Name))
                    {
                        IList<TransformTemplate> potentialTransforms = Global.TransformTemplates.Where(t => t.Inputs.Keys.Contains(transfer.Resource)).ToList();

                        // if we transferred a resource that can't be transformed, proceed with all
                        // transforms and transfers
                        if (potentialTransforms.Count > 0)
                        {
                            // if there's more than one transform template that uses this resource,
                            // randomly select one template to use
                            var random = new Random();
                            TransformTemplate transform = potentialTransforms[random.Next(potentialTransforms.Count)];

                            ExecuteTransform(currentNode, self, successors, transform);
                        }
                        else
                        {
                            ExecuteAllTransformsAndTransfers(currentNode, self, successors);
                        }
                    }
                }
                // if previous action was a transform and the action before that was a transfer to self,
                // we send half the transformed resources back to the original country
                else
                {
                    TransferTemplate transfer = (TransferTemplate)twoStepsBefore;
                    TransformTemplate transform = (TransformTemplate)oneStepBefore;

                    KeyValuePair<string, int> resourceAndAmount = transform.Outputs.Where(r => !r.Key.Equals("population") && !r.Key.Contains("Waste")).FirstOrDefault();
                    ExecuteTransfer(currentNode, self, transfer.ReceivingCountry, resourceAndAmount.Key, successors, resourceAndAmount.Value / 2);
                }
            }
            // if we're not in a Transfer sequence, execute all possible transfer
            // and transform actions
            else
            {
                ExecuteAllTransformsAndTransfers(currentNode, self, successors);
            }
        
            return successors;
        }

        public void ExecuteAllTransformsAndTransfers(Node currentNode, Country self, IList<Node> successors)
        {
            foreach (TransformTemplate template in Global.TransformTemplates)
            {
                ExecuteTransform(currentNode, self, successors, template);
            }

            foreach (Country country in currentNode.State.Where(c => !c.IsSelf))
            {
                foreach (string resource in country.State.Keys)
                {
                    // default to only executing transfers to self to minimize
                    // state space and complexity
                    ExecuteTransfer(currentNode, country, self.Name,
                        resource, successors);
                }
            }
        }

        public void ExecuteTransform(Node currentNode, Country self, IList<Node> successors, TransformTemplate template)
        {
            TransformTemplate grounded = template.DeepCopy();
            // ensure all transactions involve self to limit state space
            grounded.Country = self.Name;

            // maximize the number of resources in transform to limit state space
            // also ensures we won't exceed number of resources required for a given transform
            grounded.SetScale(self);

            if (grounded.Scale > 0)
            {
                Node successor = currentNode.DeepCopy();

                // update successor state and schedule
                successor.Schedule.Steps.Add(grounded);
                grounded.Execute(successor.State.Where(c => c.IsSelf).FirstOrDefault());

                successors.Add(successor);
            }
        }

        public void ExecuteTransfer(Node currentNode, Country transferringCountry, string receivingCountry,
            string resource, IList<Node> successors, int amount = 0)
        {
            // population and land cannot be transferred at this time
            if (!resource.Equals("population") &&
                !resource.Equals("availableLand") &&
                !resource.Equals("farm"))
            {
                TransferTemplate transferTemplate = new TransferTemplate()
                {
                    TransferringCountry = transferringCountry.Name,
                    ReceivingCountry = receivingCountry,
                    Resource = resource,
                    // for now just transfer half by default
                    // potentially improve in part 2
                    Amount = amount != 0 ? amount : transferringCountry.State[resource] / 2
                };

                Node successor = currentNode.DeepCopy();

                // update successor state and schedule
                successor.Schedule.Steps.Add(transferTemplate);
                transferTemplate.Execute(successor.State);

                successors.Add(successor);
            }
        }

        public void UpdateFrontier(PriorityQueue<Node, double> frontier, Node potentialSuccessor, double potentialSuccessorUtility)
        {
            if (frontier.Count < 30)
            {
                frontier.Enqueue(potentialSuccessor, potentialSuccessorUtility);
                return;
            }

            // unfortunately, there does not seem to be a way to toggle dequeuing of both
            // highest and lowest nodes for C# priority queues.
            // because of this, we make a new priority queue with the default behavior of
            // dequeuing the lowest value first (no custom comparer) and compare that utility
            // to that of the potential successor.
            Node node;
            double utility;
            PriorityQueue<Node, double> frontierCopy = new PriorityQueue<Node, double>();

            for (int i = 0; i < frontier.Count; i++)
            {
                frontier.TryDequeue(out node, out utility);
                frontierCopy.Enqueue(node, utility);
            }

            frontierCopy.TryDequeue(out node, out utility);
            if (potentialSuccessorUtility > utility)
            {
                frontierCopy.Enqueue(potentialSuccessor, potentialSuccessorUtility);
            }
            else
            {
                frontierCopy.Enqueue(node, utility);
            }

            for (int i = 0; i < frontierCopy.Count; i++)
            {
                frontierCopy.TryDequeue(out node, out utility);
                frontier.Enqueue(node, utility);
            }
        }

        public bool DetermineTransferSequence(Node currentNode, Country self, out Action oneStepBefore, out Action twoStepsBefore)
        {
            oneStepBefore = null;
            twoStepsBefore = null;

            // if we have no parent, we cannot be in a transfer sequence
            if (currentNode.Parent == null ||
                    currentNode.Parent.Schedule.Steps.Count == 0)
            {
                return false;
            }

            oneStepBefore = currentNode.Parent.Schedule.Steps[^1];
            oneStepBefore.Type = oneStepBefore.GetType().Name;

            if (currentNode.Parent.Schedule.Steps.Count > 1)
            {
                twoStepsBefore = currentNode.Parent.Schedule.Steps[^2];
                twoStepsBefore.Type = twoStepsBefore.GetType().Name;
            }

            IList<Action> steps = new List<Action>();
            foreach (Action step in new List<Action>() { oneStepBefore, twoStepsBefore })
            {
                if (step != null)
                {
                    if (step.GetType().Name.Equals("TransferTemplate"))
                    {
                        TransferTemplate transfer = (TransferTemplate)step;
                        if (transfer.ReceivingCountry.Equals(self.Name))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
