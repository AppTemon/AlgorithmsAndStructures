using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GenericTrees
{
    internal class TreeLeftShoulderEnumerator<TKey, TValue> : TreeEnumeratorBase<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TreeLeftShoulderEnumerator() : base()
        {

        }

        public override TreeEnumeratorBase<TKey, TValue> BuildEnumerator(TreeNode<TKey, TValue> rootNode)
        {
            TryAddNode(rootNode);
            return this;
        }

        protected override void TryAddNode(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return;
            else
            {
                TryAddNode(node.left);
                _traverseList.Add(new KeyValuePair<TKey, TValue>(node.key, node.value));
            }
        }
    }
}
