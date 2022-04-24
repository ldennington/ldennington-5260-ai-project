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

        public void GameScheduler(int depthBound, int frontierBoundary, bool testModel)
        {
            // read input
            ReadFiles();

            // execute search
            Node startNode = new Node()
            {
                State = Global.InitialState
            };
            Search(startNode, depthBound, frontierBoundary, testModel);

            // write results
            writer.WriteSchedules();
        }

        public void ReadFiles()
        {
            reader.ReadResources(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "resource-input.csv"));
            reader.ReadTransformTemplates(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "transform-template-input.json"));
            reader.ReadCountries(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "country-input.csv"));
        }

        public void Search(Node startNode, int depthBound, int frontierBoundary, bool testModel)
        {
            // C# priority queue defaults to dequeuing lowest scores first
            // override this with a custom comparer to dequeue highest scores first
            ScheduleComparer customComparer = new ScheduleComparer();
            PriorityQueue<Node, double> frontier = new PriorityQueue<Node, double>(customComparer);

            // add initial state to frontier
            frontier.Enqueue(startNode, 0.0);

            while (frontier.Count > 0)
            {
                bool isFinal = false;
                Node currentNode = frontier.Dequeue();
                if (currentNode.Depth >= depthBound)
                {
                    isFinal = true;
                    calculator.CalculateExpectedUtility(currentNode.Schedule, startNode.State, currentNode.State, isFinal, testModel);
                    Global.Solutions.Enqueue(new Schedule()
                    {
                        Actions = currentNode.Schedule.Actions,
                    }, currentNode.Schedule.Actions.Last().ExpectedUtility);
                }
                else
                {
                    foreach (Node successor in GenerateSuccessors(currentNode, startNode))
                    {
                        calculator.CalculateExpectedUtility(successor.Schedule, startNode.State, successor.State, isFinal, testModel);
                        double expectedUtility = successor.Schedule.Actions.Last().ExpectedUtility;
                        UpdateFrontier(frontier, successor, expectedUtility, frontierBoundary);
                    }
                }
            }
        }

        public IList<Node> GenerateSuccessors(Node currentNode, Node startNode)
        {
            IList<Node> successors = new List<Node>();
            Country self = currentNode.State.Where(c => c.IsSelf).FirstOrDefault();

            // Set up for Transfer Sequence:
            // 1. A country transfers resources to self.
            // 2. Self uses those resources in a transform.
            // 3. Self sends half of the transform outputs back to country from Step 1.
            Action? lastAction = null;
            Action? actionBeforeLastAction = null;

            if (currentNode.Schedule.Actions.Count > 0)
            {
                lastAction = currentNode.Schedule.Actions.Last();

                if (currentNode.Parent != null &&
                        currentNode.Parent.Schedule.Actions.Count > 0)
                {
                    actionBeforeLastAction = currentNode.Parent.Schedule.Actions.Last();
                }
            }

            // we have completed the first step of a transfer sequence
            // execute the second step now
            if (lastAction != null &&
                lastAction.GetType().Name.Equals("TransferTemplate"))
            {
                TransferTemplate transfer = (TransferTemplate)lastAction;

                if (transfer.ReceivingCountry.Equals(self.Name))
                {
                    IList<TransformTemplate> potentialTransforms = Global.TransformTemplates
                        .Where(t => t.Inputs.Keys.Contains(transfer.Resource))
                        .ToList();
                    // if we transferred a resource that can't be transformed, we cannot
                    // create a sequence
                    if (potentialTransforms.Count > 0)
                    {
                        Node successor = ExecuteTransferSequenceStepTwo(potentialTransforms, currentNode, self);

                        if (successor != null)
                        {
                            successors.Add(successor);
                            return successors;
                        }
                    }
                }
            }

            if (lastAction != null &&
                lastAction.GetType().Name.Equals("TransformTemplate") &&
                actionBeforeLastAction != null &&
                actionBeforeLastAction.GetType().Name.Equals("TransferTemplate"))
            {
                // we have completed the second step of a transfer sequence
                // execute the third step now
                TransformTemplate transform = (TransformTemplate)lastAction;
                TransferTemplate transfer = (TransferTemplate)actionBeforeLastAction;

                if (transfer.ReceivingCountry.Equals(self.Name) &&
                    transform.Inputs.Keys.Contains(transfer.Resource))
                {
                    Node successor = ExecuteTransferSequenceStepThree(transform, currentNode, transfer.TransferringCountry, self);

                    if (successor != null)
                    {
                        successors.Add(successor);
                    }
                    return successors;
                }
            }

            ExecuteTransforms(currentNode, self, successors);
            ExecuteTransfers(currentNode, self, successors, startNode);
            return successors;
        }

        public void ExecuteTransforms(Node currentNode, Country self, IList<Node> successors)
        {
            foreach (TransformTemplate template in Global.TransformTemplates)
            {
                Node successor = ExecuteTransform(currentNode, self, template);
                if (successor != null)
                {
                    successors.Add(successor);
                }
            }
        }

        public void ExecuteTransfers(Node currentNode, Country self, IList<Node> successors, Node startNode)
        {
            foreach (Country country in currentNode.State.Where(c => !c.IsSelf))
            {
                foreach (string resource in country.State.Keys)
                {
                    Node successor = null;
                    successor = ExecuteTransfer(currentNode, country, self.Name, resource, successor: successor);

                    if (successor != null)
                    {
                        TransferTemplate transfer = (TransferTemplate)successor.Schedule.Actions.Last();

                        if (transfer.Amount > 0)
                        {
                            successors.Add(successor);
                        }
                    }
                }
            }
        }

        public Node ExecuteTransferSequenceStepTwo(IList<TransformTemplate> potentialTransforms, Node currentNode, Country self)
        {
            foreach (TransformTemplate potentialTransform in potentialTransforms)
            {
                bool enoughResourcesForTransfer = true;
                foreach (string resource in potentialTransform.Inputs.Keys)
                {
                    if (self.State.ContainsKey(resource))
                    {
                        if (self.State[resource] < potentialTransform.Inputs[resource])
                        {
                            enoughResourcesForTransfer = false;
                            break;
                        }
                    }
                }

                if (enoughResourcesForTransfer)
                {
                    return ExecuteTransform(currentNode, self, potentialTransform);
                }
            }

            // no template was found that fit current state
            return null;
        }

        public Node ExecuteTransferSequenceStepThree(TransformTemplate transform, Node currentNode, string receivingCountry, Country self)
        {
            // if Step 2 transform was successful, send half the transformed resources back to the
            // original country
            KeyValuePair<string, int> resourceAndAmount = transform.Outputs.Where(r => !r.Key.Equals("Population") && !r.Key.Contains("Waste")).FirstOrDefault();
            return ExecuteTransfer(currentNode, self, receivingCountry, resourceAndAmount.Key, resourceAndAmount.Value / 2);
        }

        public Node ExecuteTransform(Node currentNode, Country self, TransformTemplate template, Node? successor = null)
        {
            TransformTemplate grounded = template.DeepCopy();
            // ensure all transactions involve self to limit state space
            grounded.Country = self.Name;

            // maximize the number of resources in transform to limit state space
            // also ensures we won't exceed number of resources required for a given transform
            grounded.SetScale(self);

            if (grounded.Scale > 0)
            {
                successor = successor == null ? currentNode.DeepCopy() : successor;

                // update successor state and schedule
                successor.Schedule.Actions.Add(grounded);
                grounded.Execute(successor.State.Where(c => c.IsSelf).FirstOrDefault());
            }

            // ensure Available Land (our State Quality divisor) never reaches 0
            if (successor != null && successor.State[0].State["Available Land"] > 0)
            {
                return successor;
            }

            return null;
        }

        public Node ExecuteTransfer(Node currentNode, Country transferringCountry, string receivingCountry,
            string resource, int amount = 0, Node? successor = null)
        {
            // population and land cannot be transferred
            if (!resource.Equals("Population") &&
                !resource.Equals("Available Land") &&
                !resource.Equals("Farm"))
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

                successor = successor == null ? currentNode.DeepCopy() : successor;

                // update successor state and schedule
                successor.Schedule.Actions.Add(transferTemplate);
                transferTemplate.Execute(successor.State);

                return successor;
            }

            return null;
        }

        public void UpdateFrontier(PriorityQueue<Node, double> frontier, Node potentialSuccessor, double potentialSuccessorUtility, int frontierBoundary)
        {
            if (frontier.Count < frontierBoundary)
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
    }
}
