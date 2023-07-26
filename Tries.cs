﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Minerva.Module
{
    public class Tries<TValue> : ITries<TValue>, IEnumerable<KeyValuePair<string, TValue>>, IEnumerable, IDictionary<string, TValue>
    {
        readonly char separator = '.';
        Node root;

        public TValue this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }


        private class Node
        {
            public Dictionary<string, Node> children;
            public TValue value;
            public int count;
            public bool isTerminated;

            public Node()
            {
                children = new Dictionary<string, Node>();
            }

            public Node(TValue value) : this()
            {
                this.value = value;
                isTerminated = true;
            }

        }

        public Tries()
        {
            root = new Node();
        }

        public Tries(char separator) : this()
        {
            this.separator = separator;
        }

        public Tries(IEnumerable<string> ts) : this()
        {
            foreach (var item in ts)
            {
                Add(item, default);
            }
        }

        public Tries(char separator, IEnumerable<string> ts) : this(separator)
        {
            foreach (var item in ts)
            {
                Add(item, default);
            }
        }


        private Tries(Node root)
        {
            this.root = root;
        }

        public int Count => root.count;

        public List<string> FirstLevelKeys => root.children.Keys.ToList();

        public List<string> Keys
        {
            get
            {
                List<string> array = new List<string>(Count * 2);
                GetKeys(array, root, "");
                return array;
            }
        }

        public List<TValue> Values
        {
            get
            {
                List<TValue> array = new List<TValue>(Count * 2);
                GetValues(array, root);
                return array;
            }
        }

        ICollection<string> IDictionary<string, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<string, TValue>.Values => Values;

        public bool IsReadOnly => false;

        public bool Add(string s, TValue value)
        {
            if (ContainsKey(s))
            {
                return false;
            }

            string[] prefix = s.Split(separator);

            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                currentNode.count++;
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
            currentNode.value = value;
            currentNode.isTerminated = true;

            return true;
        }
        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool ContainsKey(string s)
        {
            string[] prefix = s.Split(separator);
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

        public TValue Remove(string s)
        {
            if (!ContainsKey(s))
            {
                return default;
            }

            string[] prefix = s.Split(separator);

            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                currentNode.count--;
                Node childNode = currentNode.children[key];
                if (childNode.count == 1)
                {
                    currentNode.children.Remove(key);
                }
                currentNode = childNode;

            }

            return currentNode.value;
        }

        public bool TryGetValue(string s, out TValue value)
        {
            string[] prefix = s.Split(separator);

            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                if (!currentNode.children.ContainsKey(key))
                {
                    value = default;
                    return false;
                }
                else
                {
                    currentNode = currentNode.children[key];
                }
            }
            if (!currentNode.isTerminated)
            {
                value = default;
                return false;
            }
            value = currentNode.value;
            return true;
        }

        public TValue Get(string s)
        {
            string[] prefix = s.Split(separator);
            Node currentNode = GetNode(prefix);
            if (!currentNode.isTerminated)
            {
                throw new KeyNotFoundException();
            }
            return currentNode.value;
        }
        public void Set(string s, TValue value)
        {
            string[] prefix = s.Split(separator);

            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                currentNode.count++;
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
            currentNode.value = value;
            currentNode.isTerminated = true;
        }

        public Tries<TValue> GetSubTrie(string s)
        {
            string[] prefix = s.Split(separator);
            Node currentNode = GetNode(prefix);
            if (currentNode == null)
            {
                throw new ArgumentException();
            }
            return new Tries<TValue>(currentNode);
        }

        public bool TryGetSubTrie(string s, out Tries<TValue> trie)
        {
            try
            {
                trie = GetSubTrie(s);
                return true;
            }
            catch (Exception)
            {
                trie = null;
                return false;
            }
        }

        public List<string> GetChildrenKeys()
        {
            return root.children.Keys.ToList();
        }

        private Node GetNode(string[] prefix)
        {
            Node currentNode = root;
            for (int i = 0; i < prefix.Length; i++)
            {
                string key = prefix[i];
                if (!currentNode.children.ContainsKey(key))
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    currentNode = currentNode.children[key];
                }
            }

            return currentNode;
        }

        private void GetValues(List<TValue> list, Node node)
        {
            foreach (var item in node.children)
            {
                Node child = item.Value;
                if (child.isTerminated)
                {
                    list.Add(child.value);
                }
                GetValues(list, child);
            }
        }

        private void GetKeys(List<string> list, Node node, string prefix)
        {
            foreach (var item in node.children)
            {
                Node child = item.Value;
                string conbinedKey = prefix + item.Key;
                if (child.isTerminated)
                {
                    list.Add(conbinedKey);
                }
                GetKeys(list, child, conbinedKey + ".");
            }
        }

        private void GetKeyValuePairs(List<KeyValuePair<string, TValue>> list, Node node, string prefix = "")
        {
            foreach (var item in node.children)
            {
                Node child = item.Value;
                string conbinedKey = prefix + item.Key;
                if (child.isTerminated)
                {
                    list.Add(new KeyValuePair<string, TValue>(conbinedKey, child.value));
                }
                GetKeyValuePairs(list, child, conbinedKey + ".");
            }
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            var list = new List<KeyValuePair<string, TValue>>();
            GetKeyValuePairs(list, root);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDictionary<string, TValue>.Add(string key, TValue value)
        {
            Add(key, value);
        }

        bool IDictionary<string, TValue>.Remove(string key)
        {
            return Remove(key) != null;
        }

        public void Clear()
        {
            root = new Node();
        }
        public void Clear(string key)
        {
            var node = GetNode(key.Split(separator));
            node.isTerminated = false;
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ContainsKey(item.Key) && Get(item.Key).Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            var list = new List<KeyValuePair<string, TValue>>();
            GetKeyValuePairs(list, root);
            Array.Copy(list.ToArray(), 0, array, arrayIndex, list.Count);
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return Contains(item) && ((IDictionary<string, TValue>)this).Remove(item.Key);
        }
    }

    public interface ITries<TValue>
    {
        TValue Get(string s);
    }
}