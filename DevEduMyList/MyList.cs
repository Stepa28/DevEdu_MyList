using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevEduMyList
{
    
    class MyList<T> : IList<T>
    {
        T[] _array;
        int _size;
        int _capacity;


        public MyList()
        {
            _size = 0;
            _capacity = 0;
        }
        public MyList(int capasity)
        {
            _array = new T[capasity];
            _size = 0;
            _capacity = capasity;
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


        public void TheIndexIsCorrect(int index)
        {
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException("Индекс выыходит за границы массива!");
        }
        private void TheRangeIsCorrect(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException("Должн быть больше 0");
            if (index > Count || index + count > Count)
                throw new ArgumentException("Не должн выходить за приделы листа");
        }
        private void ExpansionArray()
        {
            T[] arrTmp = new T[(int)(Capacity * 1.2) + 1];
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
        public void Add(params T[] item) =>
            AddRange(item);
        public void AddRange(T[] arr)
        {
            ExpansionArray(arr.Length);
            for (int i = 0; i < arr.Length; i++)
                _array[_size + i] = arr[i];
        }
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T elem in collection)
                Add(elem);
        }



        public void Clear() =>
            _size = 0;

        public bool Contains(T item) =>
            IndexOf(item) >= 0;               


        public IEnumerator<T> GetEnumerator()
        {
           return  _array.GetEnumerator() as IEnumerator<T>;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
          return _array.GetEnumerator();
        }


        public int IndexOf(T item) =>
            IndexOf(item, 0, Count);
        public int IndexOf(T item, int index) =>
            IndexOf(item, index, Count - index);
        public int IndexOf(T item, int index, int count)
        {
            TheRangeIsCorrect(index, count);
            for (int i = index; i < index + count; i++)
                if (_array[i].Equals(item))
                    return i;

            return -1;
        }
        public int LastIndexOf(T item) =>
            LastIndexOf(item, 0, Count);
        public int LastIndexOf(T item, int index) =>
            LastIndexOf(item, index, Count - index);
        public int LastIndexOf(T item, int index, int count)
        {
            if (index > Count || count < 0 || index + count > Count)
                throw new ArgumentOutOfRangeException("Диапозон должен находится в пределах листа");
            for (int i = Count - 1 - index; i >= Count - 1 - index - count; i--)
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
                throw new ArgumentNullException("Колекция не может быть пустой");
            foreach (T elem in collection)
                Insert(index, elem);
        }


        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            TheIndexIsCorrect(index);
            for (int i = index; i < Count; i++)
                _array[i] = _array[i + 1];
            _size--;
        }
        //public int RemoveAll(Predicate<T> match)
        //{

        //}
        public void RemoveRange(int index, int count)
        {
            TheRangeIsCorrect(index, count);
            for (int i = index; i < Count - count; i++)
                _array[i] = _array[i + count];
            _size -= count;
        }


        public ReadOnlyCollection<T> AsReadOnly() => new(this);


        //public int BinarySearch(T item) =>
        //    BinarySearch(0, Count, item, item as IComparer<T>);
        //public int BinarySearch(T item,IComparer<T> comparer) =>
        //    BinarySearch(0, Count, item, comparer);
        //public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        //{
        //    TheRangeIsCorrect(index, count);
        //    if (comparer == null && !(item is IComparable<T>))
        //        throw new InvalidOperationException("Не сравниваемый тип");
        //    int left = index;
        //    int right = index + count;
        //    while (left <= right)
        //    {
        //        var middle = (left + right) / 2;

        //        if (comparer.Compare(item,_array[middle]) == 0)
        //            return middle;

        //        else if (comparer.Compare(item,_array[middle]) < 0)
        //            right = middle - 1;
        //        else
        //            left = middle + 1;
        //    }

        //    return -1;
        //}


        //public MyList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        //{

        //}


        public void CopyTo(T[] array) =>
            CopyTo(0, array, 0, Count);
        public void CopyTo(T[] array, int arrayIndex) =>
            CopyTo(0, array, arrayIndex, Count);
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            TheRangeIsCorrect(index, count);
            if (array == null)
                throw new ArgumentNullException("Масив должен существовать");                        
            for (int i = 0; i < count; i++)
                array[arrayIndex + i] = _array[index + i];
        }


        //public bool Exists(Predicate<T> match)
        //{

        //}


        public MyList<T> GetRange(int index, int count)
        {
            TheRangeIsCorrect(index, count);
            return new MyList<T>(_array[index..(index+ count)]);
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


        //public void Sort()
        //{

        //}


        public T[] ToArray() => _array[.._size];


        public void TrimExcess()
        {
            if (Count != Capacity)
                _capacity = _size;
        }


        //public bool TrueForAll(Predicate<T> match)
        //{

        //}
    }
}
