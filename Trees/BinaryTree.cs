using System.Collections.Generic;
using System;

namespace GenericTrees
{
    public class BinaryTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int Count => _count;

        protected TreeNode<TKey, TValue> _rootNode;
        protected int _count;

        public TValue this[TKey key]
        {
            get
            {
                TryGetValue(key, out TValue value);
                return value;
            }
            set
            {
                AddOrReplace(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return FindNode(key) != null;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node == null)
            {
                value = default;
                return false;
            }
            value = node.value;
            return true;
        }

        public void Clear()
        {
            _rootNode = null;
            _count = 0;
        }

        public bool Add(TKey key, TValue value)
        {
            var node = FindNode(key);
            if (node != null)
                return false;

            var newNode = new TreeNode<TKey, TValue>(key, value);
            AddNode(newNode);

            return true;
        }

        public bool AddOrReplace(TKey key, TValue value)
        {
            var node = FindNode(key);
            if (node != null)
            {
                node.value = value;
                return false;
            }

            var newNode = new TreeNode<TKey, TValue>(key, value);
            AddNode(newNode);

            return true;
        }

        public bool Remove(TKey key)
        {
            var node = FindNode(key);
            if (node == null)
                return false;

            RemoveNode(node);
            return true;
        }

        protected virtual void AddNode(TreeNode<TKey, TValue> node)
        {
            if (_rootNode == null)
            {
                _rootNode = node;
                _count = 1;
                return;
            }

            node.parent = FindParent(node.key);
            if (node.key.CompareTo(node.parent.key) > 0)
                node.parent.right = node;
            else
                node.parent.left = node;

            _count++;
        }

        protected virtual void RemoveNode(TreeNode<TKey, TValue> node)
        {
            var parent = node.parent;

            if (_count == 1 && parent == null) // remove last node
            {
                _rootNode = null;
            }
            else if (node.left == null && node.right == null) // remove leaf
            {
                if (parent.left == node)
                    parent.left = null;
                else
                    parent.right = null;
            }
            else if (node.left != null && node.right != null) // remove node with who childrens
            {
                TreeNode<TKey, TValue> successorNode = null;

                if (node.left != null)
                {
                    successorNode = node.left;
                    while (successorNode.right != null)
                        successorNode = successorNode.right;
                }
                else
                {
                    successorNode = node.right;
                    while (successorNode.left != null)
                        successorNode = successorNode.left;
                }

                node.key = successorNode.key;
                node.value = successorNode.value;

                RemoveNode(successorNode);
                return;
            }
            else // remove node with single child
            {
                TreeNode<TKey, TValue> successorNode = null;
                if (node.left != null)
                    successorNode = node.left;
                else
                    successorNode = node.right;

                successorNode.parent = parent;

                if (parent != null)
                {
                    if (parent.left == node)
                        parent.left = successorNode;
                    else
                        parent.right = successorNode;
                }
                else
                {
                    _rootNode = successorNode;
                }
            }

            node.parent = null;
            node.left = null;
            node.right = null;

            _count--;
        }

        TreeNode<TKey, TValue> FindNode(TKey key)
        {
            TreeNode<TKey, TValue> node = _rootNode;
            while (node != null)
            {
                switch (key.CompareTo(node.key))
                {
                    case -1:
                        node = node.left;
                        break;
                    case 1:
                        node = node.right;
                        break;
                    case 0:
                        return node;
                    default:
                        return null;
                }
            }
            return null;
        }

        TreeNode<TKey, TValue> FindParent(TKey key)
        {
            TreeNode<TKey, TValue> node = _rootNode;
            while (node != null)
            {
                switch (key.CompareTo(node.key))
                {
                    case -1:
                        if (node.left == null)
                            return node;
                        node = node.left;
                        break;
                    case 1:
                        if (node.right == null)
                            return node;
                        node = node.right;
                        break;
                    default:
                        return null;
                }
            }
            return null;
        }

        public int GetHeight()
        {
            return GetHeight(_rootNode);
        }

        public int GetHeight(TKey key)
        {
            TreeNode<TKey, TValue> node = FindNode(key);
            if (node == null)
                return 0;
            return GetHeight(node);
        }

        protected int GetHeight(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return 0;

            return 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new TreeInOrderEnumerator<TKey, TValue>(_rootNode);
        }
        
        public virtual TreeEnumeratorBase<TKey, TValue> BuilEnumerator(TreeEnumeratorBase<TKey, TValue> enumerator)
        {
            return enumerator.BuildEnumerator(_rootNode);
        }
    }
}
