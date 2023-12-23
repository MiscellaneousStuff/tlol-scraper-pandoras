using System.Collections;

namespace Api.Utils;

/// <summary>
/// IDK HOW TO CALL IT BUT FUCK GC IT WILL HAVE MAX SIZE IT EVER GOT
/// CLEAR RESETS COUNTER TO 0 SO ADDING ELEMENTS SETS JUST ELEMENTS
/// Its like ObjectPool so GC WONT FREEZ APP
/// </summary>
/// <typeparam name="T"></typeparam>
public class PooledList<T> : IEnumerable<T>
{
    private readonly int _resizeSize;
    private T[] _items;
    private Func<T> _create;
    public int Count { get; private set; }

    public bool IsReadOnly => false;
    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }
    
    public PooledList(int initialSize, int resizeSize, Func<T> create)
    {
        Count = 0;
        _resizeSize = resizeSize;
        _create = create;
        _items = new T[initialSize];
        for (var i = 0; i < initialSize; i++)
        {
            _items[i] = create();
        }
    }
    
    public Enumerator GetEnumerator()
        => new Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
        => new Enumerator(this);


    public IEnumerable<T> Items()
    {
        for (var i = 0; i < Count; i++)
            yield return _items[i];
    }
    
    public void Add(T item)
    {
        return;
    }

    public void Clear()
    {
        Count = 0;
    }

    public T GetNext()
    {
        if (Count >= _items.Length)
        {
            Resize(_items.Length + _resizeSize);
        }
        
        var item = _items[Count];
        Count++;
        return item;
    }
    
    public T GetNext(Action<T> process)
    {
        var next = GetNext();
        process(next);
        return next;
    }

    public void CancelNext()
    {
        Count--;
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Array.Copy(_items, 0, array!, arrayIndex, Count);
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
            for (var i = Count - 1; i < newSize; i++)
            {
                _items[i] = _create();
            }
        }
        else
        {
            _items = Array.Empty<T>();
        }
    }

    public struct Enumerator : IEnumerator<T>, IEnumerator
    {
        private readonly PooledList<T> _list;
        private int _index;
        private T? _current;

        internal Enumerator(PooledList<T> list)
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