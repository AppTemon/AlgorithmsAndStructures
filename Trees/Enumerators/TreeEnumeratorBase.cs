using System.Collections;
using System.Collections.Generic;
using System;

namespace GenericTrees
{
    public abstract class TreeEnumeratorBase<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        protected KeyValuePair<TKey, TValue> _current;
        protected int _nextIndex;
        protected List<KeyValuePair<TKey, TValue>> _traverseList;

        public TreeEnumeratorBase()
        {
            _traverseList = new List<KeyValuePair<TKey, TValue>>();
            _nextIndex = 0;
        }

        public abstract TreeEnumeratorBase<TKey, TValue> BuildEnumerator(TreeNode<TKey, TValue> rootNode);
        protected abstract void TryAddNode(TreeNode<TKey, TValue> node);

        public KeyValuePair<TKey, TValue> Current
        {
            get { return _current; }
        }
        
        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            _traverseList.Clear();
            _nextIndex = 0;
        }

        public void Reset()
        {
            _nextIndex = 0;
        }

        public bool MoveNext()
        {
            if (_nextIndex >= _traverseList.Count)
                return false;
            _current = _traverseList[_nextIndex];
            _nextIndex++;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this;
        }
    }
}
