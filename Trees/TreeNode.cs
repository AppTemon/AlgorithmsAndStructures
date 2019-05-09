using System;

namespace GenericTrees
{
    public class TreeNode<TKey, TValue>  where TKey : IComparable<TKey>
    {
        public TKey key;
        public TValue value;
        public TreeNode<TKey, TValue> parent, left, right;

        public TreeNode(TKey newKey, TValue newValue)
        {
            key = newKey;
            value = newValue;
            parent = left = right = null;
        }
    }
}
