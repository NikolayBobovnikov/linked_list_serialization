namespace ListSerializer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var head = new ListNode() { Data = "HeadData" };
            var tail = new ListNode() { Data = "TailData" };
            head.Next= tail;

            var s = ListNodeSerializerHelper.MakeFromListNode(head);

            using (Stream stream = new FileStream("log", FileMode.Create))
            {
                s.Serialize(stream);
            }

            using (Stream stream = new FileStream("log", FileMode.Open))
            {
                s.Deserialize(stream);
            }
            
        }
    }
}