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

        public void Execute()
        {
            // read input
            ReadFiles();

            // designate self
            Country self = Global.InitialState.Where(x => x.Name.Equals("atlantis")).FirstOrDefault();
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

        public void Search(Node startNode, int depthBound = 10)
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
                    Global.Schedules.Enqueue(new Schedule()
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
                        UpdateFrontier(frontier, successor, successorExpectedUtility);
                    }
                }
            }
        }

        public IList<Node> GenerateSuccessors(Node currentNode)
        {
            IList<Node> successors = new List<Node>();

            // ensure all transactions involve self to limit state space
            Country self = currentNode.State.Where(c => c.IsSelf).FirstOrDefault();

            // iterate over transform templates
            foreach (TransformTemplate template in Global.TransformTemplates)
            {
                TransformTemplate grounded = template.DeepCopy();
                grounded.Country = self.Name;
                // maximize the number of resources in transform to limit state space
                // also ensures we won't exceed number of resources required for a given transform
                grounded.SetScale(self);

                Node successor = currentNode.DeepCopy();

                // update successor state and schedule
                successor.Schedule.Steps.Add(grounded);
                ExecuteTransform(grounded, successor.State.Where(c => c.IsSelf).FirstOrDefault());

                successors.Add(successor);
            }

            // execute transfers
            foreach (Country country in currentNode.State)
            {
                foreach (string resource in country.State.Keys)
                {
                    // population and land cannot be transferred at this time
                    if (!resource.Equals("population") && 
                        !resource.Equals("availableLand") &&
                        !resource.Equals("farm"))
                    {
                        if (!country.IsSelf)
                        {
                            TransferTemplate transferTemplate = new TransferTemplate()
                            {
                                TransferringCountry = country.Name,
                                ReceivingCountry = self.Name,
                                Resource = resource,
                                // for now just transfer half
                                // potentially improve in part 2
                                Amount = country.State[resource] / 2
                            };

                            Node successor = currentNode.DeepCopy();

                            // update successor state and schedule
                            successor.Schedule.Steps.Add(transferTemplate);
                            ExecuteTransfer(transferTemplate, successor.State);

                            successors.Add(successor);
                        }
                    }
                }
            }

            return successors;
        }

        public void ExecuteTransform(TransformTemplate template, Country country)
        {
            foreach (string resource in template.Inputs.Keys)
            {
                country.State[resource] -= template.Inputs[resource];
            }

            foreach (string resource in template.Outputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    country.State.Add(resource, 0);
                }

                country.State[resource] += template.Outputs[resource];
            }
        }

        public void ExecuteTransfer(TransferTemplate transferTemplate, IList<Country> state)
        {
            Country receivingCountry = state.Where(c => c.Name == transferTemplate.ReceivingCountry).FirstOrDefault();
            Country transferringCountry = state.Where(c => c.Name == transferTemplate.TransferringCountry).FirstOrDefault();

            receivingCountry.State[transferTemplate.Resource] = receivingCountry.State[transferTemplate.Resource] += transferTemplate.Amount;
            transferringCountry.State[transferTemplate.Resource] = transferringCountry.State[transferTemplate.Resource] -= transferTemplate.Amount;
        }

        public void UpdateFrontier(PriorityQueue<Node, double> frontier, Node potentialSuccessor, double potentialSuccessorUtility)
        {
            if (frontier.Count < 10)
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
