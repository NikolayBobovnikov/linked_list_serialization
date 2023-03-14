
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace ListSerializer
{
    /// <summary>
    /// Class represents node of the ListRandom linked list.
    /// </summary>
    public class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string Data;
    }

    /// <summary>
    /// Class represents double linked list with Count elements, containing Head and Tail nodes on the left/right sides correspondently.
    /// </summary>
    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        #region Public methods

        public void Serialize(Stream s)
        {
            ListNodeSerializerHelper.Serialize(s, this);
        }

        public void Deserialize(Stream s)
        {
            var decerialized = ListNodeSerializerHelper.Deserialize(s);
            Head = decerialized.Head;
            Tail = decerialized.Tail;
            Count = decerialized.Count;
        }

        #endregion
    }
}
