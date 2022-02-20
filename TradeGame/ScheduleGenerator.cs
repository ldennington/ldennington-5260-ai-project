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
            Country self = Global.InitialState.Where(x => x.Name.Equals("atlantis", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
            Country selfInitialState = startNode.State.Where(c => c.IsSelf).FirstOrDefault();
            double initialExpectedUtility = calculator.CalculateExpectedUtility(new List<Action>(), selfInitialState, selfInitialState);
            frontier.Enqueue(startNode, initialExpectedUtility);

            while(frontier.Count > 0)
            {
                Node currentNode = frontier.Dequeue();
                if (currentNode.Depth >= depthBound)
                {
                    double currentExpectedUtility = calculator.CalculateExpectedUtility(currentNode.Steps, selfInitialState, currentNode.State.Where(c => c.IsSelf).FirstOrDefault());
                    Global.Schedules.Enqueue(new Schedule()
                    {
                        Steps = currentNode.Steps,
                        ExpectedUtility = currentExpectedUtility
                    }, currentExpectedUtility);
                }
                else
                {
                    foreach(Node successor in GenerateSuccessors(currentNode))
                    {
                        double successorExpectedUtility = calculator.CalculateExpectedUtility(currentNode.Steps, selfInitialState, currentNode.State.Where(c => c.IsSelf).FirstOrDefault());
                        frontier.Enqueue(successor, successorExpectedUtility);
                    }
                }
            }
        }

        // operator rules of thumb?
        // satisfying pre-conditions?
        // consider adding knowledge
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
                SetScale(grounded, self);

                Node successor = new Node();
                successor.Parent = currentNode;
                // initially set successor state and schedule to be the same as that of current node
                successor.State = currentNode.State;
                successor.Steps = currentNode.Steps;

                // update successor state and schedule
                successor.Steps.Add(grounded);
                ExecuteTransform(grounded, successor.State.Where(c => c.IsSelf).FirstOrDefault());

                successors.Add(successor);
            }

            return successors;
        }

        /* adapted from Jeff Baranski's explanation of his solution
        at https://piazza.com/class/kyz01i5gip25bn?cid=60_f2 */
        public void SetScale(TransformTemplate template, Country country)
        {
            int[] maxes = new int[template.Inputs.Count];
            int i = 0;
            foreach (string resource in template.Inputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    maxes[i] = 0;
                }
                else
                {
                    maxes[i] = country.State[resource] / template.Inputs[resource];
                }
                i++;
            }

            template.Scale = maxes.Min();
        }

        public void ExecuteTransform(TransformTemplate template, Country country)
        {
            foreach (string resource in template.Inputs.Keys)
            {
                country.State[resource] -= template.Inputs[resource] * template.Scale;
            }

            foreach (string resource in template.Outputs.Keys)
            {
                if (!country.State.Keys.Contains(resource))
                {
                    country.State.Add(resource, 0);
                }

                country.State[resource] += template.Outputs[resource] * template.Scale;
            }
        }
    }
}
