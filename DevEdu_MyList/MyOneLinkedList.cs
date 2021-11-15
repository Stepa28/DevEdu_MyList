using System;
using System.Collections;
using System.Collections.Generic;

namespace DevEdu_MyList
{
    public class MyOneLinkedList<T> : IEnumerable<T>
    {
        public class OneLinkedNode<T>
        {
            public T Data { get; set; }
            public OneLinkedNode<T> Next { get; set; }

            public OneLinkedNode(T data)
            {
                Data = data;
            }
        }
        
        private static void NotEmpty(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj),"Ссылка не должна быть пустой");
        }
        
        public MyOneLinkedList(){}
        public MyOneLinkedList(IEnumerable<T> collections)
        {
            NotEmpty(collections);
            foreach (var elem in collections)
            {
                Add(elem);
            }
        }
        

        private OneLinkedNode<T> _head;
        private OneLinkedNode<T> _tail;
        private int _count;

        
        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public OneLinkedNode<T> First => _head;
        public OneLinkedNode<T> Last => _tail;
    
        
        public IEnumerator<T> GetEnumerator()
        {
            OneLinkedNode<T> current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
        

        public void Add(T data)
        {
            OneLinkedNode<T> node = new(data);
            if (_head == null)
                _head = node;
            else
                _tail = node;
            _count++;
        }
        public void AddAfter(OneLinkedNode<T> node, T data)
        {
            AddAfter(node, new OneLinkedNode<T>(data));
        }
        public void AddAfter(OneLinkedNode<T> node, OneLinkedNode<T> newNode)
        {
            NotEmpty(node);
            NotEmpty(newNode);
            OneLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Equals(node))
                {
                    if (!current.Equals(_head))
                    {
                        newNode.Next = current.Next;
                        current.Next = newNode;
                        if (newNode.Next == null)
                            _tail = newNode;
                    }
                    else
                    {
                        newNode.Next = _head.Next;
                        _head.Next = newNode;
                    }
                    _count++;
                    return;
                }

                current = current.Next;
            }
            throw new InvalidOperationException("Указанный узел не найден");
        }
        public void AppendFirst(T data)
        {
            AppendFirst(new OneLinkedNode<T>(data));
        }
        public void AppendFirst(OneLinkedNode<T> node)
        {
            NotEmpty(node);
            node.Next = _head;
            _head = node;
            if (_count == 0) _tail = _head;
            _count++;
        }
        public void AddBefore(OneLinkedNode<T> node, T data)
        {
            AddBefore(node, new OneLinkedNode<T>(data));
        }
        public void AddBefore(OneLinkedNode<T> node, OneLinkedNode<T> newNode)
        {
            NotEmpty(node);
            NotEmpty(newNode);
            OneLinkedNode<T> current = _head;
            OneLinkedNode<T> previous = null;
            while (current != null)
            {
                if (current.Equals(node))
                {
                    if (previous != null)
                    {
                        newNode.Next = current;
                        previous.Next = newNode;
                    }
                    else
                    {
                        newNode.Next = _head;
                        _head = newNode;
                    }
                    _count++;
                    return;
                }

                previous = current;
                current = current.Next;
            }
            throw new InvalidOperationException("Указанный узел не найден");
        }
        
        
        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
        
        
        public bool Contains(T data)
        {
            OneLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        
        public void RemoveFirst()
        {
            if (_head == null) 
                throw new InvalidOperationException("Список должен быть не пустым");
            _head = _head.Next;
            _count--;
        }
        public void RemoveLast()
        {
            if (_head == null) 
                throw new InvalidOperationException("Список должен быть не пустым");
            Remove(_tail);
        }
        public bool Remove(T data)
        {
            OneLinkedNode<T> current = _head;
            OneLinkedNode<T> previous = null;
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        _head = _head.Next;
                        if (_head == null)
                            _tail = null;
                    }
                    _count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }
        public bool Remove(OneLinkedNode<T> node)
        {
            NotEmpty(node);
            OneLinkedNode<T> current = _head;
            OneLinkedNode<T> previous = null;
            while (current != null)
            {
                if (current.Equals(node))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        _head = _head.Next;
                        if (_head == null)
                            _tail = null;
                    }
                    _count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        
        public void CopyTo(T[] arr, int index)
        {
            NotEmpty(arr);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс должен быть больше 0");
            OneLinkedNode<T> current = _head;
            for (int i = index; current != null; i++)
            {
                arr[i] = current.Data;
                current = current.Next;
            }
        }


        public OneLinkedNode<T> Find(T data)
        {
            OneLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return current;
                current = current.Next;
            }
            return null;
        }
        public OneLinkedNode<T> FindLast(T data)
        {
            OneLinkedNode<T> res = null;
            OneLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    res = current;
                current = current.Next;
            }
            return res;
        }
        
        
        public OneLinkedNode<T> NodeAt(int index)
        {
            if (index >= _count || index < 0)
                throw new IndexOutOfRangeException("Индекс выходит за приделы списка");
            OneLinkedNode<T> returnable = _head;
            while (index!=0)
            {
                returnable = returnable.Next;
                index--;
            }

            return returnable;
        }
        public T ElementAt(int index) => NodeAt(index).Data;
        
    }
}