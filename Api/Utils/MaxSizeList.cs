using System.Collections;

namespace Api.Utils;

/// <summary>
/// IDK HOW TO CALL IT BUT FUCK GC IT WILL HAVE MAX SIZE IT EVER GOT
/// CLEAR RESETS COUNTER TO 0 SO ADDING ELEMENTS SETS JUST ELEMENTS
/// </summary>
/// <typeparam name="T"></typeparam>
public class MaxSizeList<T> : IList<T>
{
    private readonly int _resizeSize;
    private T[] _items;

    public MaxSizeList(int initialSize, int resizeSize)
    {
        _resizeSize = resizeSize;
        _items = new T[initialSize];
    }

    public int Count { get; private set; }

    public bool IsReadOnly => false;
    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }
    
    public void Add(T item)
    {
        if (Count == _items.Length)
        {
            Resize(_items.Length + _resizeSize);
        }
        _items[Count] = item;
        Count++;
    }

    public void Clear()
    {
        Count = 0;
    }
    
    public bool Remove(T item)
    {
        var itemIndex = IndexOf(item);
        if (itemIndex < 0)
        {
            return false;
        }
        
        RemoveAt(itemIndex);

        return true;
    }

    public int IndexOf(T item) => Array.IndexOf(_items, item, 0, Count);

    public void Insert(int index, T item)
    {
        if (index > Count)
        {
            return;
        }

        if (index < Count)
        {
            _items[Count] = _items[index];
            _items[index] = item;
            Count++;
        }
        else
        {
            _items[index] = item;
        }
    }

    public void RemoveAt(int index)
    {
        (_items[index], _items[Count - 1]) = (_items[Count - 1], _items[index]);
        Count--;
    }
    
    public void Resize(int newSize)
    {
        if (newSize > 0)
        {
            var newItems = new T[newSize];
            if (_items.Length > 0)
            {
                Array.Copy(_items, newItems, _items.Length);
            }
            _items = newItems;
        }
        else
        {
            _items = Array.Empty<T>();
        }
    }
    
    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Array.Copy(_items, 0, array!, arrayIndex, Count);
    }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
        => new Enumerator(this);
    
    public IEnumerable<T> Items()
    {
        for (var i = 0; i < Count; i++)
            yield return _items[i];
    }
    
    public struct Enumerator : IEnumerator<T>, IEnumerator
    {
        private readonly MaxSizeList<T> _list;
        private int _index;
        private T? _current;

        internal Enumerator(MaxSizeList<T> list)
        {
            _list = list;
            _index = 0;
            _current = default;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_index >= _list.Count) return MoveNextRare();
            
            _current = _list._items[_index];
            _index++;
            return true;

        }

        public void Reset()
        {
            _index = 0;
            _current = default;
        }

        private bool MoveNextRare()
        {
            _index = _list.Count + 1;
            _current = default;
            return false;
        }

        public T Current => _current!;

        object? IEnumerator.Current
        {
            get
            {
                if (_index == 0 || _index == _list.Count + 1)
                {
                    return null;
                }

                return Current;
            }
        }
    }
}