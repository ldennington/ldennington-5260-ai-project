using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
