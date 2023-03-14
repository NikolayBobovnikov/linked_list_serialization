using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSerializer
{
    /// <summary>
    /// Class provides helper types and methods for serializing linked list.
    /// </summary>
    public static class ListNodeSerializerHelper
    {
        #region Public types

        /// <summary>
        /// Intermediary class for storing instead of ListNode.
        /// </summary>
        public struct ListNodeFlat
        {
            public ListNodeFlat(ListNodeFlat other)
            {
                PrevPosition = other.PrevPosition;
                NextPosition = other.NextPosition;
                Random = other.Random;
                Data = other.Data;
            }

            public int PrevPosition = NULL_POS;
            public int NextPosition = NULL_POS;
            public int Random = NULL_POS;
            public string Data;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create ListRandom from list node.
        /// </summary>
        /// <param name="node">List node.</param>
        /// <returns>Instance of ListRandom class fillded from the node data.</returns>
        public static ListRandom MakeFromListNode(ListNode node)
        {
            var result = new ListRandom() { Head = node };

            int count = 0;
            while (node != null)
            {
                result.Tail = node;
                node = node.Next;
                count++;
            }

            result.Count = count;

            return result;
        }

        /// <summary>
        /// Restore ListRandom object from the flatten list of structures.
        /// </summary>
        /// <param name="flatten">Flatten list</param>
        /// <returns>Instance of ListRandom class.</returns>
        public static ListRandom RestoreFromFlatten(IList<ListNodeFlat> flatten)
        {
            // create ListNodes from corresponding flatten elements and store them into list
            var nodes = flatten.Select(x => new ListNode() { Data = x.Data }).ToList();

            // restore node correspondence
            foreach (var (node, index) in nodes.Select((value, i) => (value, i)))
            {
                if (flatten[index].NextPosition is int nextPos && nextPos != NULL_POS)
                {
                    node.Next = nodes[nextPos];
                }

                if (flatten[index].PrevPosition is int prevPos && prevPos != NULL_POS)
                {
                    node.Previous = nodes[prevPos];
                }

                if (flatten[index].Random is int randomPos && randomPos != NULL_POS)
                {
                    node.Random = nodes[randomPos];
                }
            }

            // construct result structure if there are nodes in there
            var result = new ListRandom();
            if (nodes.Count > 0)
            {
                result.Head = nodes.First();
                result.Tail = nodes.Last();
                result.Count = nodes.Count;
            }

            return result;
        }

        /// <summary>
        /// Converts linked list to the list of flatten structures.
        /// </summary>
        /// <param name="linkedList">Linked list.</param>
        /// <returns>List of flatten structures.</returns>
        public static IList<ListNodeFlat> Flatten(ListNode linkedList)
        {
            // result flatten list
            var flatten = new List<ListNodeFlat>();

            // aux data to get node by position
            var nodes = new List<ListNode>();

            // mapping between nodes and positions
            var nodeToPositionDict = new Dictionary<ListNode, int>();


            // prepare aux data (contiguous list of nodes and mapping to node positions)
            var currentNode = linkedList;
            int index = 0;
            while (currentNode != null)
            {
                // add flatten node to list (head and tail are corrected in the end)
                nodes.Add(currentNode);

                // store map between node and its position
                nodeToPositionDict.Add(currentNode, index);

                // go to next node
                currentNode = currentNode.Next;
                index++;
            }


            // fill flatten list
            index = 0;
            foreach (var node in nodes)
            {
                // get position of that node in the list corresponding to node's ptr
                var pos = node.Random == null ? NULL_POS : nodeToPositionDict[node.Random];

                // save position in flatten structure
                flatten.Add(new ListNodeFlat()
                {
                    Data = node.Data,
                    PrevPosition = (index == 0 ? NULL_POS : index - 1),
                    NextPosition = (index == nodes.Count - 1 ? NULL_POS : index + 1),
                    Random = pos
                }
                );

                index++;
            }

            return flatten;
        }


        /// <summary>
        /// Converts ListRandom to the list of flatten structures corresponding to the linked list rooted in Head.
        /// </summary>
        /// <param name="randomList">Instance of ListRandom class.</param>
        /// <returns>List of flatten structures.</returns>
        public static IList<ListNodeFlat> Flatten(ListRandom randomList)
        {
            // flatten list starting from head node
            var flatten = Flatten(randomList.Head);

            // verify
            Debug.Assert(flatten.Count == randomList.Count);

            return flatten;
        }

        /// <summary>
        /// Serialize ListRandom object.
        /// </summary>
        /// <param name="s">Stream.</param>
        /// <param name="list">Instance of ListRandom class to be serialized.</param>
        public static void Serialize(Stream s, ListRandom list)
        {
            // Go through all nodes and get their positions
            // Map pointers to positions to resolve Random
            // Map ListNode class to ListNodeFlat structure containing node positions instead of pointers
            // Write flatten list of ListNodeFlat's into stream

            var flatten = Flatten(list.Head);

            // first save count to know how many elements to read during deserialisation
            s.WriteMarshal(list.Count);

            // write each element in the list
            foreach (var flat in flatten)
            {
                s.WriteMarshal(flat);
            }
        }

        /// <summary>
        /// Decerialize object from specified stream.
        /// </summary>
        /// <param name="s">Stream.</param>
        /// <returns>Instance of ListRandom class containing decerialized linked list.</returns>
        public static ListRandom Deserialize(Stream s)
        {
            // Read flatten list of ListNodeFlat's from stream
            // Restore pointers to Random nodes from positions in flatten structure

            // first save count to know how many elements to read during deserialisation
            int count = s.ReadMarshal<int>();

            // reserve for count elements
            var flatten = new List<ListNodeFlat>(count);

            // reaad from stream and add each element to the list
            for (int i = 0; i < count; i++)
            {
                var node = s.ReadMarshal<ListNodeFlat>();
                flatten.Add(node);
            }

            // Restore linked list from flatten list witn positions
            return RestoreFromFlatten(flatten);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Virtual position which correpsonds to null node.
        /// </summary>
        public static int NULL_POS => int.MinValue;

        #endregion
    }
}
