using ListSerializer;

namespace ListSerializerTests
{
    [TestClass]
    public class SerializationTests
    {
        const string _fileName = "dump";

        [TestMethod]
        public void SerializationTest()
        {
            // Arrange
            var head = new ListNode() { Data = "Head data" };
            var mid = new ListNode() { Data = "Mid data" };
            var tail = new ListNode() { Data = "Tail data" };
            head.Next = mid;
            mid.Next = tail;
            ListRandom original = ListNodeSerializerHelper.MakeFromListNode(head);
            ListRandom result = new ListRandom();

            // Act
            using (Stream stream = new FileStream(_fileName, FileMode.Create))
            {
                original.Serialize(stream);
            }
            using (Stream stream = new FileStream(_fileName, FileMode.Open))
            {
                result.Deserialize(stream);
            }

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(original.Count, result.Count);
            Assert.AreEqual(original.Head.Data, result.Head.Data);
            Assert.AreEqual(original.Head.Next.Data, result.Head.Next.Data);
            Assert.AreEqual(original.Head.Next.Next.Data, result.Head.Next.Next.Data);
        }
    }
}