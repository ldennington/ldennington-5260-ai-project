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

        public void GameScheduler(int depthBound, bool limitFrontierSize)
        {
            // read input
            ReadFiles();

            // execute search
            Node startNode = new Node()
            {
                State = Global.InitialState
            };
            Search(startNode, depthBound, limitFrontierSize);

            // write results
            writer.WriteSchedules();
        }

        public void ReadFiles()
        {
            reader.ReadResources(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "resource-input.csv"));
            reader.ReadTransformTemplates(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "transform-template-input.json"));
            reader.ReadCountries(Path.Combine(Directory.GetCurrentDirectory(), "InputFiles", "country-input.csv"));
        }

        public void Search(Node startNode, int depthBound, bool limitFrontierSize)
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
                    calculator.CalculateExpectedUtility(currentNode.Schedule, startNode.State, currentNode.State);
                    Global.Solutions.Enqueue(new Schedule()
                    {
                        Actions = currentNode.Schedule.Actions,
                    }, currentNode.Schedule.Actions.Last().ExpectedUtility);
                }
                else
                {
                    foreach (Node successor in GenerateSuccessors(currentNode))
                    {
                        calculator.CalculateExpectedUtility(successor.Schedule, startNode.State, successor.State);
                        double expectedUtility = successor.Schedule.Actions.Last().ExpectedUtility;

                        if (limitFrontierSize)
                        {
                            UpdateFrontier(frontier, successor, expectedUtility);
                        }
                        else
                        {
                            frontier.Enqueue(successor, expectedUtility);
                        }
                    }
                }
            }
        }

        public IList<Node> GenerateSuccessors(Node currentNode)
        {
            IList<Node> successors = new List<Node>();
            Country self = currentNode.State.Where(c => c.IsSelf).FirstOrDefault();
            ExecuteTransformsAndTransfers(currentNode, self, successors);
            return successors;
        }

        public void ExecuteTransformsAndTransfers(Node currentNode, Country self, IList<Node> successors)
        {
            // this means a transfer sequence occurred, and we don't want to
            // generate any successors until we catch up with depth
            if (currentNode.Schedule.Actions.Count > currentNode.Depth)
            {
                return;
            }

            foreach (TransformTemplate template in Global.TransformTemplates)
            {
                Node successor = ExecuteTransform(currentNode, self, template);
                if (successor != null)
                {
                    successors.Add(ExecuteTransform(currentNode, self, template));
                }
            }

            // Transfer Sequence:
            // 1. A country transfers resources to self.
            // 2. Self uses those resources in a transform.
            // 3. Self sends half of the transform outputs back to country from Step 1.
            foreach (Country country in currentNode.State.Where(c => !c.IsSelf))
            {
                foreach (string resource in country.State.Keys)
                {
                    Node successor = null;
                    // population and land cannot be transferred at this time
                    if (!resource.Equals("Population") &&
                        !resource.Equals("Available Land") &&
                        !resource.Equals("Farm"))
                    {
                        successor = ExecuteTransfer(currentNode, country, self.Name, resource, successor: successor);
                        IList<TransformTemplate> potentialTransforms = Global.TransformTemplates.Where(t => t.Inputs.Keys.Contains(resource)).ToList();

                        // if we transferred a resource that can't be transformed, terminate
                        if (potentialTransforms.Count > 0)
                        {
                            // if there's more than one transform template that uses this resource,
                            // randomly select one template to use
                            var random = new Random();
                            TransformTemplate transformTemplate = potentialTransforms[random.Next(potentialTransforms.Count)];
                            successor = ExecuteTransform(currentNode, self, transformTemplate, successor);

                            // if transform was successful, send half the transformed resources back to the
                            // original country
                            Action previousStep = successor.Schedule.Actions.Last();
                            if (previousStep is TransformTemplate)
                            {
                                TransformTemplate transform = (TransformTemplate)previousStep;
                                KeyValuePair<string, int> resourceAndAmount = transform.Outputs.Where(r => !r.Key.Equals("Population") && !r.Key.Contains("Waste")).FirstOrDefault();
                                successor = ExecuteTransfer(currentNode, self, country.Name, resourceAndAmount.Key, resourceAndAmount.Value / 2, successor);
                            }

                            successors.Add(successor);
                        }
                        else
                        {
                            successors.Add(successor);
                        }
                    }
                }
            }
        }

        public Node ExecuteTransform(Node currentNode, Country self, TransformTemplate template, Node successor = null)
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

            return successor;
        }

        public Node ExecuteTransfer(Node currentNode, Country transferringCountry, string receivingCountry,
            string resource, int amount = 0, Node successor = null)
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

        public void UpdateFrontier(PriorityQueue<Node, double> frontier, Node potentialSuccessor, double potentialSuccessorUtility)
        {
            if (frontier.Count < 100)
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
