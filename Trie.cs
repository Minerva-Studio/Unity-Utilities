﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minerva.Module
{
    public class Trie : IEnumerable<string>, IEnumerable, ICollection<string>
    {
        private class Node
        {
            public Dictionary<string, Node> children;
            public int count;
            public bool isTerminated;

            public Node()
            {
                children = new Dictionary<string, Node>();
            }

            public Node(bool isTerminated) : this()
            {
                this.isTerminated = isTerminated;
            }


            public Node Clone()
            {
                Node node = new()
                {
                    count = count,
                    isTerminated = isTerminated,
                    children = new()
                };

                foreach (var item in node.children)
                {
                    node.children[item.Key] = item.Value.Clone();
                }

                return node;
            }

            public void TraverseCopy(StringBuilder stringBuilder, char separator, string[] arr, ref int idx)
            {
                foreach (var (key, node) in children)
                {
                    var baseLength = stringBuilder.Length;
                    stringBuilder.Append(key);
                    if (node.isTerminated)
                    {
                        arr[idx] = stringBuilder.ToString();
                        idx++;
                    }
                    stringBuilder.Append(separator);
                    node.TraverseCopy(stringBuilder, separator, arr, ref idx);
                    stringBuilder.Length = baseLength;
                }
            }
        }



        readonly char separator = '.';
        Node root;

        public bool this[string key]
        {
            get { return Contains(key); }
            set { Set(key, value); }
        }

        public int Count => root.count;

        public ICollection<string> FirstLevelKeys => root.children.Keys.ToList();

        public string[] Keys
        {
            get
            {
                string[] arr = new string[Count];
                CopyTo(arr, 0);
                return arr;
            }
        }

        public bool IsReadOnly => false;

        public Trie()
        {
            root = new Node();
        }

        public Trie(char separator) : this()
        {
            this.separator = separator;
        }

        public Trie(IEnumerable<string> ts) : this()
        {
            foreach (var item in ts)
            {
                Add(item);
            }
        }

        public Trie(char separator, IEnumerable<string> ts) : this(separator)
        {
            foreach (var item in ts)
            {
                Add(item);
            }
        }

        private Trie(Node root, char separator) : this(separator)
        {
            this.root = root;
        }


        void ICollection<string>.Add(string item) => Add(item);
        public bool Add(string s)
        {
            string[] prefix = GetPrefix(s);

            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                //move to the path
                if (currentNode.children.ContainsKey(key))
                {
                    currentNode = currentNode.children[key];
                }
                //current path does not exist, create nodes to create the path
                else
                {
                    Node newNode = new Node();
                    currentNode.children.Add(prefix[i], newNode);
                    currentNode = newNode;
                }
            }
            // already exist
            if (currentNode.isTerminated)
            {
                return false;
            }

            currentNode.isTerminated = true;

            currentNode = root;
            currentNode.count++;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                //move to the path 
                currentNode = currentNode.children[key];
                currentNode.count++;
            }
            return true;
        }

        public void AddRange(IEnumerable<string> ts)
        {
            foreach (var item in ts) Add(item);
        }

        public bool Contains(string s)
        {
            string[] prefix = GetPrefix(s);
            Node currentNode = root;

            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                if (!currentNode.children.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    currentNode = currentNode.children[key];
                }
            }
            return currentNode.isTerminated;
        }

        public bool Remove(string s)
        {
            string[] prefix = GetPrefix(s);
            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                if (!currentNode.children.ContainsKey(key)) return false;
                Node childNode = currentNode.children[key];
                currentNode = childNode;
            }

            if (!currentNode.isTerminated) return false;
            currentNode.isTerminated = false;

            root.count--;
            currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                currentNode = currentNode.children[key];
                currentNode.count--;
            }
            return true;
        }

        public bool Set(string s, bool value)
        {
            if (value)
            {
                return Add(s);
            }
            else
            {
                return Remove(s);
            }
        }

        public Trie GetSubTrie(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new Trie(root, separator);
            }
            string[] prefix = GetPrefix(s);
            return TryGetNode(prefix, out var currentNode)
                ? new Trie(currentNode, separator)
                : throw new ArgumentException();
        }

        public bool TryGetSubTrie(string s, out Trie trie)
        {
            string[] prefix = GetPrefix(s);
            if (!TryGetNode(prefix, out Node currentNode))
            {
                trie = null;
                return false;
            }
            trie = new Trie(currentNode, separator);
            return true;
        }

        public List<string> GetChildrenKeys()
        {
            return root.children.Keys.ToList();
        }

        private bool TryGetNode(string[] prefix, out Node node)
        {
            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                if (!currentNode.children.TryGetValue(key, out currentNode))
                {
                    node = null;
                    return false;
                }
            }
            node = currentNode;
            return node != null;
        }

        private void GetKeys(List<string> list, Node node, StringBuilder sb)
        {
            foreach (var item in node.children)
            {
                Node child = item.Value;
                int currentLength = sb.Length;
                sb.Append(item.Key);
                if (child.isTerminated)
                {
                    int length = sb.Length;
                    list.Add(sb.ToString());
                    sb.Length = length;
                }
                GetKeys(list, child, sb.Append(separator));
                sb.Length = currentLength;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] arr = new string[Count];
            CopyTo(arr, 0);
            foreach (var item in arr)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string[] GetPrefix(string s)
        {
            if (s.EndsWith(separator)) return s.Split(separator)[..^1];
            return s.Split(separator);
        }

        public void Clear()
        {
            root = new Node();
        }

        public bool Clear(string key)
        {
            if (!TryGetNode(key.Split(separator), out var node)) return false;
            node.isTerminated = false;
            return true;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            int index = arrayIndex;
            root?.TraverseCopy(new(), separator, array, ref index);
        }

        public Trie Clone()
        {
            return new Trie(root.Clone(), separator);
        }

        public string[] ToArray()
        {
            string[] arry = new string[Count];
            CopyTo(arry, 0);
            return arry;
        }
    }
}