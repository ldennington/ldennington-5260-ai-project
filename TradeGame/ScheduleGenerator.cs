namespace TradeGame
{
    internal class ScheduleGenerator
    {
        private ICalculator calculator;

        public ScheduleGenerator(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public PriorityQueue<IList<IAction>, double> Search(Node startNode, int depthBound = 3)
        {
            // C# priority queue defaults to dequeuing lowest scores first
            // override this with a custom comparer to dequeue highest scores first
            ScheduleComparer customComparer = new ScheduleComparer();
            PriorityQueue<IList<IAction>, double> solutions = new PriorityQueue<IList<IAction>, double>(customComparer);
            PriorityQueue<Node, double> frontier = new PriorityQueue<Node, double>(customComparer);

            // add initial state to frontier
            Country selfInitialState = startNode.State.Where(c => c.IsSelf).FirstOrDefault();
            double initialExpectedUtility = calculator.CalculateExpectedUtility(new List<IAction>(), selfInitialState, selfInitialState);
            frontier.Enqueue(startNode, initialExpectedUtility);

            while(frontier.Count > 0)
            {
                Node currentNode = frontier.Dequeue();
                if (currentNode.Depth >= depthBound)
                {
                    double currentExpectedUtility = calculator.CalculateExpectedUtility(currentNode.Schedule, selfInitialState, currentNode.State.Where(c => c.IsSelf).FirstOrDefault());
                    solutions.Enqueue(currentNode.Schedule, currentExpectedUtility);
                }
                else
                {
                    foreach(Node successor in GenerateSuccessors(currentNode))
                    {
                        double successorExpectedUtility = calculator.CalculateExpectedUtility(currentNode.Schedule, selfInitialState, currentNode.State.Where(c => c.IsSelf).FirstOrDefault());
                        frontier.Enqueue(successor, successorExpectedUtility);
                    }
                }
            }

            return solutions;
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
                successor.Schedule = currentNode.Schedule;

                // update successor state and schedule
                successor.Schedule.Add(grounded);
                ExecuteTransform(grounded, successor.State.Where(c => c.IsSelf).FirstOrDefault());

                successors.Add(successor);
            }

            return successors;
        }

        /* adapted from Jeff Baranski's explanation of his solution
        at https://piazza.com/class/kyz01i5gip25bn?cid=60_f2  */
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
