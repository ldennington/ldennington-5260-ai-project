using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void GetDepth()
        {
            Node node1 = new Node();
            Node node2 = new Node() { Parent = node1 };
            Node node3 = new Node() { Parent = node2 };
            Node node4 = new Node() { Parent = node3 };

            node1.Depth.Should().Be(0);
            node2.Depth.Should().Be(1);
            node3.Depth.Should().Be(2);
            node4.Depth.Should().Be(3);
        }

        [TestMethod]
        public void DeepCopy()
        {
            Node node = new Node()
            {
                State = new List<Country>
                { },
                Schedule = new Schedule()
                { }
            };

            Node copy = node.DeepCopy();

            copy.Parent.Should().Be(node);
            copy.Depth.Should().Be(1);
            copy.State.Should().NotBeSameAs(node.State);
            copy.Schedule.Should().NotBeSameAs(node.Schedule);
        }
    }
}
