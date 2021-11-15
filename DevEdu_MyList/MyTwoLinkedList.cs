using System;
using System.Collections;
using System.Collections.Generic;

namespace DevEdu_MyList
{
    public class MyTwoLinkedList<T>: IEnumerable<T>
    {
        public class TwoLinkedNode<T>
        {
            public T Data { get; set; }
            public TwoLinkedNode<T> Previous { get; set; }
            public TwoLinkedNode<T> Next { get; set; }
            public TwoLinkedNode(T data)
            {
                Data = data;
            }
        }

        
        private static void NotEmpty(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Ссылка не должна быть пустой");
        }

        
        private TwoLinkedNode<T> _head;
        private TwoLinkedNode<T> _tail;
        private int _count;
        
        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public TwoLinkedNode<T> First => _head;
        public TwoLinkedNode<T> Last => _tail;

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            TwoLinkedNode<T> current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        public IEnumerable<T> BackEnumerator()
        {
            TwoLinkedNode<T> current = _tail;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }

        
        public MyTwoLinkedList(){}
        public MyTwoLinkedList(IEnumerable<T> collections)
        {
            NotEmpty(collections);
            foreach (var elem in collections)
            {
                Add(elem);
            }
        }
        
        
        public void Add(T data)
        {
            TwoLinkedNode<T> node = new(data);
            
            if (_head == null)
                _head = node;
            else
            {
                _tail.Next = node;
                node.Previous = _tail;
            }
            _tail = node;
            _count++;
        }
        public void AddAfter(TwoLinkedNode<T> node, T data)
        {
            AddAfter(node, new TwoLinkedNode<T>(data));
        }
        public void AddAfter(TwoLinkedNode<T> node, TwoLinkedNode<T> newNode)
        {
            NotEmpty(node);
            NotEmpty(newNode);
            TwoLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Equals(node))
                {
                    if (!current.Equals(_head))
                    {
                        newNode.Next = current.Next;
                        current.Next = newNode;
                        newNode.Previous = current;
                        if (newNode.Next == null)
                            _tail = newNode;
                        else
                            newNode.Next.Previous = newNode;
                    }
                    else
                    {
                        newNode.Next = _head.Next;
                        newNode.Previous = _head;
                        _head.Next = newNode;
                        if (newNode.Next != null) 
                            newNode.Next.Previous = newNode;
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
            AppendFirst(new TwoLinkedNode<T>(data));
        }
        public void AppendFirst(TwoLinkedNode<T> node)
        {
            NotEmpty(node);
            node.Next = _head;
            _head.Previous = node;
            _head = node;
            if (_count == 0) _tail = _head;
            _count++;
        }
        public void AppendEnd(T data)
        {
            AppendFirst(new TwoLinkedNode<T>(data));
        }
        public void AppendEnd(TwoLinkedNode<T> node)
        {
            NotEmpty(node);
            node.Next = null;
            node.Previous = _tail;
            _tail.Next = node;
            _tail = node;
            if (_count == 0) _head = _tail ;
            _count++;
        }
        public void AddBefore(TwoLinkedNode<T> node, T data)
        {
            AddBefore(node, new TwoLinkedNode<T>(data));
        }
        public void AddBefore(TwoLinkedNode<T> node, TwoLinkedNode<T> newNode)
        {
            NotEmpty(node);
            NotEmpty(newNode);
            TwoLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Equals(node))
                {
                    if (!current.Equals(_head))
                    {
                        newNode.Next = current;
                        newNode.Previous = current.Previous;
                        current.Previous = newNode;
                        newNode.Previous.Next = newNode;
                    }
                    else
                    {
                        newNode.Previous = null;
                        newNode.Next = _head;
                        _head.Previous = newNode;
                        _head = newNode;
                    }
                    _count++;
                    return;
                }

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
            TwoLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }
        
        
        public bool Remove(T data)
        {
            TwoLinkedNode<T> current = _head;
 
            // поиск удаляемого узла
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    break;
                }
                current = current.Next;
            }

            if (current == null) return false;
            // если узел не последний
            if(current.Next!=null)
            {
                current.Next.Previous = current.Previous;
            }
            else
            {
                // если последний, переустанавливаем tail
                _tail = current.Previous;
            }
 
            // если узел не первый
            if(current.Previous!=null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                // если первый, переустанавливаем head
                _head = current.Next;
            }
            _count--;
            return true;
        }
        public bool Remove(TwoLinkedNode<T> node)
        {
            NotEmpty(node);
            TwoLinkedNode<T> current = _head;

            // поиск удаляемого узла
            while (current != null)
            {
                if (current.Equals(node))
                {
                    break;
                }

                current = current.Next;
            }

            if (current == null) return false;
            // если узел не последний
            if (current.Next != null)
            {
                current.Next.Previous = current.Previous;
            }
            else
            {
                // если последний, переустанавливаем tail
                _tail = current.Previous;
            }

            // если узел не первый
            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                // если первый, переустанавливаем head
                _head = current.Next;
            }

            _count--;
            return true;
        }
        public void RemoveFirst()
        {
            if (_head == null)
                throw new InvalidOperationException("Список должен быть не пустым");
            _head = _head.Next;
            _head.Previous = null;
            _count--;
        }
        public void RemoveLast()
        {
            if (_head == null)
                throw new InvalidOperationException("Список должен быть не пустым");
            _tail = _tail.Previous;
            _tail.Next = null;
            _count--;
        }

        
        public void CopyTo(T[] arr, int index)
        {
            NotEmpty(arr);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс должен быть больше 0");
            TwoLinkedNode<T> current = _head;
            for (int i = index; current != null; i++)
            {
                arr[i] = current.Data;
                current = current.Next;
            }
        }

        
        public TwoLinkedNode<T> Find(T data)
        {
            TwoLinkedNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return current;
                current = current.Next;
            }

            return null;
        }
        public TwoLinkedNode<T> FindLast(T data)
        {
            TwoLinkedNode<T> current = _tail;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return current;
                current = current.Previous;
            }

            return null;
        }

        
        public TwoLinkedNode<T> NodeAt(int index)
        {
            if (index >= _count || index < 0)
                throw new IndexOutOfRangeException("Индекс выходит за приделы списка");
            TwoLinkedNode<T> returnable;
            int i = 0;
            if (index < _count / 2)
            {
                returnable = _head;
                while (i < index)
                {
                    returnable = returnable.Next;
                    i++;
                }
            }
            else
            {
                returnable = _tail;
                i = _count;
                while (i > index + 1)
                {
                    returnable = returnable.Previous;
                    i--;
                }
            }
            
            return returnable;
        }
        public T ElementAt(int index) => NodeAt(index).Data;
    }
}