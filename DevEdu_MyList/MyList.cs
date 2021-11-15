using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevEdu_MyList
{
    class MyList<T> : IList<T>
    {
        private T[] _array;
        private int _size;
        private int _capacity;


        public MyList()
        {
            _size = 0;
            _capacity = 0;
        }
        public MyList(int capacity)
        {
            _array = new T[capacity];
            _size = 0;
            _capacity = capacity;
        }
        public MyList(T[] collection)
        {
            _array = collection[..];
            _size = collection.Length;
            _capacity = collection.Length;
        }


        public int Count => _size;
        public int Capacity => _capacity;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                TheIndexIsCorrect(index);
                return _array[index];
            }
            set
            {
                TheIndexIsCorrect(index);
                _array[index] = value;
            }
        }


        private void TheIndexIsCorrect(int index)
        {
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException("Индекс выходит за границы массива!");
        }
        private void TheRangeIsCorrect(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException( "index/count","Должен быть больше 0");
            if (index > Count || index + count > Count)
                throw new ArgumentException("Не должн выходить за приделы листа");
        }
        private void TheRangeLastIsCorrect(int index, int count)
        {
            if (index > Count || count < 0 || index - count > Count)
                throw new ArgumentOutOfRangeException("index/count","Диапазон должен находиться в пределах листа");
        }
        private static void NotEmpty(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj),"Ссылка не должна быть пустой");
        }

        private void ExpansionArray()
        {
            T[] arrTmp = new T[(int) (Capacity * 1.2) + 1];
            for (int i = 0; i < Capacity; i++)
                arrTmp[i] = _array[i];

            _capacity = arrTmp.Length;
            _array = arrTmp;
        }
        private void ExpansionArray(int num)
        {
            T[] arrTmp = new T[Capacity + num];
            for (int i = 0; i < Capacity; i++)
                arrTmp[i] = _array[i];

            _capacity = arrTmp.Length;
            _array = arrTmp;
        }


        public void Add(T item)
        {
            if (Count == Capacity)
                ExpansionArray();

            _array[_size] = item;
            _size++;
        }
        public void Add(params T[] item) => AddRange(item);
        public void AddRange(T[] arr)
        {
            ExpansionArray(arr.Length);
            for (int i = 0; i < arr.Length; i++)
                _array[_size + i] = arr[i];
            _size += arr.Length;
        }
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T elem in collection)
                Add(elem);
        }


        public void Clear() => _size = 0;
        public bool Contains(T item) => IndexOf(item) >= 0;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return _array[i];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return _array[i];
        }


        public int IndexOf(T item) => IndexOf(item, 0, Count);
        public int IndexOf(T item, int index) => IndexOf(item, index, Count - index);
        public int IndexOf(T item, int index, int count)
        {
            TheRangeIsCorrect(index, count);
            for (int i = index; i < index + count; i++)
                if (_array[i].Equals(item))
                    return i;

            return -1;
        }
        
        
        public int LastIndexOf(T item) => LastIndexOf(item, 0, Count);
        public int LastIndexOf(T item, int index) => LastIndexOf(item, index, Count - index);
        public int LastIndexOf(T item, int index, int count)
        {
            TheRangeLastIsCorrect(index, count);
            for (int i = index; i >= index - count; i--)
                if (_array[i].Equals(item))
                    return i;

            return -1;
        }


        public void Insert(int index, T item)
        {
            TheIndexIsCorrect(index);
            if (Count == Capacity)
                ExpansionArray();

            for (int i = Count - 1; i >= index; i--)
                _array[i + 1] = _array[i];

            _array[index] = item;
            _size++;
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection),"Коллекция не может быть пустой");
            foreach (T elem in collection)
                Insert(index, elem);
        }


        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0) return false;
            RemoveAt(index);
            return true;
        }
        public void RemoveAt(int index)
        {
            TheIndexIsCorrect(index);
            _size--;
            for (int i = index; i < Count; i++)
                _array[i] = _array[i + 1];
        }
        public int RemoveAll(Predicate<T> match)
        {
            NotEmpty(match);
            int cnt = 0;
            foreach (T elem in this)
                if (match(elem))
                {
                    Remove(elem);
                    cnt++;
                }

            return cnt;
        }
        public void RemoveRange(int index, int count)
        {
            TheRangeIsCorrect(index, count);
            for (int i = index; i < Count - count; i++)
                _array[i] = _array[i + count];
            _size -= count;
        }


        public ReadOnlyCollection<T> AsReadOnly() => new(this);


        public int BinarySearch(T item) => BinarySearch(0, Count, item, Comparer<T>.Default);
        public int BinarySearch(T item, IComparer<T> comparer) => BinarySearch(0, Count, item, comparer);
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            TheRangeIsCorrect(index, count);
            if (comparer == null && item is not IComparable<T>)
                throw new InvalidOperationException("Не сравниваемый тип");
            int left = index;
            int right = index + count;
            while (left <= right)
            {
                var middle = (left + right) / 2;

                if (comparer.Compare(item, _array[middle]) == 0)
                    return middle;

                if (comparer.Compare(item, _array[middle]) < 0)
                    right = middle - 1;
                else
                    left = middle + 1;
            }

            return -1;
        }


        public MyList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            NotEmpty(converter);
            MyList<TOutput> res = new MyList<TOutput>(Count);
            for (int i = 0; i < Count; i++)
                res.Add(converter(_array[i]));

            return res;
        }


        public void CopyTo(T[] array) => CopyTo(0, array, 0, Count);
        public void CopyTo(T[] array, int arrayIndex) => CopyTo(0, array, arrayIndex, Count);
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            TheRangeIsCorrect(index, count);
            if (array == null)
                throw new ArgumentNullException(nameof(array),"Массив должен существовать");
            for (int i = 0; i < count; i++)
                array[arrayIndex + i] = _array[index + i];
        }


        public MyList<T> GetRange(int index, int count)
        {
            TheRangeIsCorrect(index, count);
            return new MyList<T>(_array[index..(index + count)]);
        }


        public void Reverse()
        {
            for (int i = 0; i < Count / 2; i++)
                (_array[i], _array[Count - 1 - i]) = (_array[Count - 1 - i], _array[i]);
        }
        public void Reverse(int index, int count)
        {
            TheRangeIsCorrect(index, count);
            for (int i = 0; i < count / 2; i++)
                (_array[index + i], _array[index + count - 1 - i]) = (_array[index + count - 1 - i], _array[index + i]);
        }


        public void Sort() => Sort(Comparer<T>.Default);
        public void Sort(Comparison<T> comparison)
        {
            NotEmpty(comparison);
            T[] tmp = ToArray();
            Array.Sort(tmp, comparison);
            for (int i = 0; i < tmp.Length; i++)
                _array[i] = tmp[i];
        }
        public void Sort(IComparer<T> comparer) => Sort(0, Count, comparer);
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            NotEmpty(comparer);
            TheRangeIsCorrect(index, count);
            Array.Sort(_array, index, count, comparer);
        }


        public T[] ToArray() => _array[.._size];


        public void TrimExcess()
        {
            if (Count != Capacity)
                _capacity = _size;
        }


        public bool Exists(Predicate<T> match)
        {
            NotEmpty(match);
            for (int i = 0; i < Count; i++)
                if (match(_array[i]))
                    return true;

            return false;
        }


        public T Find(Predicate<T> match)
        {
            NotEmpty(match);
            for (int i = 0; i < Count; i++)
                if (match(_array[i]))
                    return _array[i];

            return default(T);
        }
        public T FindLast(Predicate<T> match)
        {
            NotEmpty(match);
            for (int i = Count - 1; i >= 0; i--)
                if (match(_array[i]))
                    return _array[i];

            return default(T);
        }
        public MyList<T> FindAll(Predicate<T> match)
        {
            NotEmpty(match);
            var tmp = new MyList<T>();
            for (int i = 0; i < Count; i++)
                if (match(_array[i]))
                    tmp.Add(_array[i]);

            return tmp;
        }
        public int FindIndex(Predicate<T> match) => FindIndex(0, Count, match);
        public int FindIndex(int startIndex, Predicate<T> match) => FindIndex(startIndex, Count - startIndex, match);
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            NotEmpty(match);
            TheRangeIsCorrect(startIndex, count);
            for (int i = startIndex; i < startIndex + count; i++)
                if (match(_array[i]))
                    return i;

            return -1;
        }
        public int FindLastIndex(Predicate<T> match) => FindLastIndex(Count - 1, Count, match);
        public int FindLastIndex(int startIndex, Predicate<T> match) => FindLastIndex(startIndex, startIndex, match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            NotEmpty(match);
            TheRangeLastIsCorrect(startIndex, count);
            for (int i = startIndex; i >= startIndex - count; i--)
                if (match(_array[i]))
                    return i;

            return -1;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            NotEmpty(match);
            for (int i = 0; i < Count; i++)
                if (!match(_array[i]))
                    return false;

            return true;
        }
    }
}