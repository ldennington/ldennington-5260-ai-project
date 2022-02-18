using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class ScheduleComparerTest
    {
        [TestMethod]
        public void EnsureHighestValueIsDequeuedFirst()
        {
            ScheduleComparer comparer = new ScheduleComparer();
            PriorityQueue<string, double> priorityQueue = new PriorityQueue<string, double>(comparer);

            priorityQueue.Enqueue("lowest priority", -2.0);
            priorityQueue.Enqueue("highest priority", 5.0);
            priorityQueue.Enqueue("medium-high priority", 3.0);
            priorityQueue.Enqueue("medium-low priority", 0.0);

            priorityQueue.Dequeue().Should().Be("highest priority");
            priorityQueue.Dequeue().Should().Be("medium-high priority");
            priorityQueue.Dequeue().Should().Be("medium-low priority");
            priorityQueue.Dequeue().Should().Be("lowest priority");
        }
    }
}
