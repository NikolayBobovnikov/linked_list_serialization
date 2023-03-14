using ListSerializer;

namespace ListSerializerTests
{
    [TestClass]
    public class ListNodeSerializerHelperTests
    {
        [TestMethod]
        public void TestMakeFromListNode_0()
        {
            // Arrange
            var head = default(ListNode);

            // Act
            var s = ListNodeSerializerHelper.MakeFromListNode(head);

            // Assert
            Assert.IsNotNull(s);
            Assert.AreEqual(0, s.Count);
            Assert.IsNull(head);
        }

        [TestMethod]
        public void TestMakeFromListNode_1()
        {
            // Arrange
            var head = new ListNode() { Data = "Head data"};

            // Act
            var s = ListNodeSerializerHelper.MakeFromListNode(head);

            // Assert
            Assert.IsNotNull(s);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(head, s.Head);
            Assert.AreEqual(head, s.Tail);
            Assert.IsNull(head.Next);
            Assert.IsNull(head.Previous);
        }

        [TestMethod]
        public void TestMakeFromListNode_2()
        {
            // Arrange
            var head = new ListNode() { Data = "Head data" };
            var tail = new ListNode() { Data = "Tail data" };
            head.Next = tail;
            tail.Previous = head;

            // Act
            var s = ListNodeSerializerHelper.MakeFromListNode(head);

            // Assert
            Assert.IsNotNull(s);
            Assert.AreEqual(2, s.Count);
            Assert.AreEqual(head.Next, tail);
            Assert.AreEqual(tail.Previous, head);
            Assert.IsNull(head.Previous);
            Assert.IsNull(tail.Next);
        }

        [TestMethod]
        public void TestMakeFromListNode_3()
        {
            // Arrange
            var head = new ListNode() { Data = "Head data" };
            var mid = new ListNode() { Data = "Mid data" };
            var tail = new ListNode() { Data = "Tail data" };
            head.Next = mid;
            mid.Next = tail;
            mid.Previous = head;
            tail.Previous = mid;

            // Act
            var s = ListNodeSerializerHelper.MakeFromListNode(head);

            // Assert
            Assert.IsNotNull(s);
            Assert.AreEqual(3, s.Count);
            Assert.AreEqual(head.Next, mid);
            Assert.AreEqual(mid.Previous, head);
            Assert.AreEqual(mid.Next, tail);
            Assert.AreEqual(tail.Previous, mid);
            Assert.IsNull(head.Previous);
            Assert.IsNull(tail.Next);
        }
    }
}