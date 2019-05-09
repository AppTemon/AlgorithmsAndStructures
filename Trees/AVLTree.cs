using System;
using System.Collections.Generic;

namespace GenericTrees
{
    public class AVLTree<TKey, TValue> : BinaryTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        protected override void AddNode(TreeNode<TKey, TValue> node)
        {
            base.AddNode(node);

            TreeNode<TKey, TValue> parent = node.parent;

            while (parent != null)
            {
                int balance = GetBalance(parent);
                if (Math.Abs(balance) == 2)
                    BalanceAt(parent, balance);

                parent = parent.parent;
            }
        }

        protected override void RemoveNode(TreeNode<TKey, TValue> node)
        {
            TreeNode<TKey, TValue> parent = node.parent;

            base.RemoveNode(node);

            while (parent != null)
            {
                int balance = GetBalance(parent);

                if (Math.Abs(balance) == 1)
                    break;

                if (Math.Abs(balance) == 2)
                {
                    BalanceAt(parent, balance);
                }

                parent = parent.parent;
            }
        }

        void BalanceAt(TreeNode<TKey, TValue> node, int balance)
        {
            if (balance == 2)
            {
                int rightBalance = GetBalance(node.right);

                if (rightBalance == 1 || rightBalance == 0)
                {
                    RotateLeft(node);
                }
                else if (rightBalance == -1)
                {
                    RotateRight(node.right);

                    RotateLeft(node);
                }
            }
            else if (balance == -2)
            {
                int leftBalance = GetBalance(node.left);

                if (leftBalance == 1)
                {
                    RotateLeft(node.left);

                    RotateRight(node);
                }
                else if (leftBalance == -1 || leftBalance == 0)
                {
                    RotateRight(node);
                }
            }
        }

        int GetBalance(TreeNode<TKey, TValue> node)
        {
            return GetHeight(node.right) - GetHeight(node.left);
        }

        void RotateLeft(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return;

            TreeNode<TKey, TValue> pivot = node.right;

            if (pivot == null)
                return;

            //Rotate
            node.right = pivot.left;
            pivot.left = node;

            if (node.right != null)
                node.right.parent = node;

            UpdateParents(node, pivot);
        }

        void RotateRight(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return;

            TreeNode<TKey, TValue> pivot = node.left;

            if (pivot == null)
                return;

            //Rotate
            node.left = pivot.right;
            pivot.right = node;

            if (node.left != null)
                node.left.parent = node;

            UpdateParents(node, pivot);
        }

        void UpdateParents(TreeNode<TKey, TValue> node, TreeNode<TKey, TValue> pivot)
        {
            TreeNode<TKey, TValue> parent = node.parent;
            node.parent = pivot;
            pivot.parent = parent;

            if (_rootNode == node)
                _rootNode = pivot;

            if (parent != null)
            {
                if (parent.left == node)
                    parent.left = pivot;
                else
                    parent.right = pivot;
            }
        }

        public override TreeEnumeratorBase<TKey, TValue> BuilEnumerator(TreeEnumeratorBase<TKey, TValue> enumerator)
        {
            throw new NotSupportedException($"AVLTree not support {enumerator.GetType()} enumerator type");
        }
    }
}
