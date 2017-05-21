using System;
using System.Collections;
using System.Collections.Generic;

namespace VulkanCore {
    public class VirtualList<T> : IList<T>
    {
        private readonly Func<int> _getCount;
        private readonly Func<int, T> _getItem;
        private readonly Func<T, bool> _addItem;
        private readonly Func<int, T, bool> _setItem;
        private readonly Func<int, bool> _removeItem;
        private readonly Func<int, T, bool> _insertItem;
        private readonly Func<bool> _clear;

        public VirtualList(
            Func<int> getCount,
            Func<int,T> getItem,
            Func<T, bool> addItem,
            Func<int, T, bool> setItem,
            Func<int, bool> removeItem,
            Func<int, T, bool> insertItem,
            Func<bool> clear = null)
        {
            _getCount = getCount;
            _getItem = getItem;
            _addItem = addItem;
            _setItem = setItem;
            _removeItem = removeItem;
            _insertItem = insertItem;
            _clear = clear;
        }

        public VirtualList(
            Func<int> getCount,
            Func<int,T> getItem,
            Func<int, T, bool> setItem,
            Func<int, bool> removeItem,
            Func<bool> clear = null)
        {
            _getCount = getCount;
            _getItem = getItem;
            _setItem = setItem;
            _removeItem = removeItem;
            _clear = clear;

            if (_setItem == null && _getCount == null)
                return;

            _addItem = item => _setItem(_getCount(), item);
            _insertItem = null;
        }
        
        public VirtualList(
            Func<int> getCount,
            Func<int,T> getItem,
            Func<T, bool> addItem,
            Func<int, bool> removeItem,
            Func<int, T, bool> insertItem,
            Func<bool> clear = null)
        {
            _getCount = getCount;
            _getItem = getItem;
            _addItem = addItem;
            _removeItem = removeItem;
            _insertItem = insertItem;
            _clear = clear;

            if (_insertItem == null)
                return;

            _setItem = (i, item) => _insertItem(i, item) && _removeItem(i + 1);
        }
        
        public VirtualList(
            Func<int> getCount,
            Func<int,T> getItem,
            Func<int, T, bool> setItem,
            Func<int, bool> removeItem,
            Func<int, T, bool> insertItem,
            Func<bool> clear = null)
        {
            _getCount = getCount;
            _getItem = getItem;
            _insertItem = insertItem;
            _setItem = setItem;
            _removeItem = removeItem;
            _clear = clear;

            if (_insertItem == null)
                return;

            _addItem = item => _insertItem(Count, item);
        }
        
        public VirtualList(
            Func<int> getCount,
            Func<int,T> getItem,
            Func<int, bool> removeItem,
            Func<int, T, bool> insertItem,
            Func<bool> clear = null)
        {
            _getCount = getCount;
            _getItem = getItem;
            _removeItem = removeItem;
            _insertItem = insertItem;
            _clear = clear;

            if (_insertItem == null)
                return;

            _addItem = item => _insertItem(Count, item);

            if (_removeItem == null)
                return;

            _setItem = (i, item) => _insertItem(i, item) && _removeItem(i + 1);
        }

        public class Enumerator : IEnumerator<T>
        {
            private VirtualList<T> _list;

            public Enumerator(VirtualList<T> list)
            {
                _list = list;
            }

            private int _index = -1;

            public bool MoveNext()
            {
                if (_index >= _list.Count - 1)
                    return false;
                ++_index;
                return true;
            }

            public void Reset()
                => _index = -1;

            public T Current
                => _list[_index];

            object IEnumerator.Current 
                => Current;

            public void Dispose()
                => _list = null;
        }

        public IEnumerator<T> GetEnumerator()
            => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void Add(T item)
        {
            if (_addItem != null && !_addItem(item))
                throw new ArgumentException("Item rejected", nameof(item));
        }

        public void Clear()
        {
            if ( _clear == null )
                for ( var i = Count-1; i >= 0; --i )
                    RemoveAt(i);
            else if (!_clear())
                throw new InvalidOperationException("Can't clear");
        }

        public bool Contains(T item)
        {
            for( int i = 0, c = Count; i < c; ++i)
                if (item.Equals(this[i]))
                    return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0, c = Count ; i < c ; ++i)
                array[arrayIndex + i] = this[i];
        }

        public bool Remove(T item)
        {
            for( int i = 0, c = Count; i < c; ++i)
                if (item.Equals(this[i]))
                {
                    RemoveAt(i);
                    return true;
                }
            return false;
        }

        public int Count
            => _getCount();

        public bool IsReadOnly
            => false;

        public int IndexOf(T item)
        {
            for( int i = 0, c = Count; i < c; ++i)
                if (item.Equals(this[i]))
                    return i;
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (_insertItem != null && !_insertItem(index, item))
                throw new ArgumentOutOfRangeException(nameof(index),index,"Can't insert item");
        }

        public void RemoveAt(int index)
        {
            if (_removeItem != null && !_removeItem(index))
                throw new ArgumentOutOfRangeException(nameof(index),index,"Can't remove item");
        }

        public T this[int index]
        {
            get => _getItem(index);
            set => _setItem(index, value);
        }
    }
}