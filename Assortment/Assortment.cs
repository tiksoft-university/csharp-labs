using Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assortment
{
    public class Assortment<T> : IAssortment<T> where T : class, IProduct
    {

        /// <summary>Идентификатор склада</summary>
        public virtual int Id { get; set; }

        /// <summary>Товары склада</summary>
        private T[] _items;

        /// <summary>Размер склада</summary>
        public int Size { get => _items.Length; }

        /// <summary>Защищенный конструктор</summary>
        protected Assortment(int id, int size) {
            if (size <= 0) return;
            Id = id;           
            _items = new T[size];            
        }

        /// <summary>Перегрузка оператора присваивания</summary>
        public static implicit operator Assortment<T>(int size)
        {
            return new Assortment<T>(1, size);
        }

        /// <summary>Индексатор получения</summary>
        public virtual T? this[int index] {
            get
            {   
                if(IsOutOfArray(index) || IsEmpty() || _items[index] == null) return null;
                
                T? item = _items[index]; _items[index] = null;

                return item;                
            }
            set
            {
                // Если индекс выходит за пределы
                if (IsOutOfArray(index) || _items[index] != null || IsEmpty()) return;
                _items[index] = value;              
            } 
        }

        /// <summary>Утилита: Проверка на пустоту</summary>
        protected bool IsEmpty() => _items.Length == 0;

        /// <summary></summary>
        protected bool IsOutOfArray(int index) => index < 0 || index > _items.Length - 1;

        /// <summary>Добавление товара в первый свободный слот</summary>
        bool IAssortment<T>.Add(T item)
        {
            var firstIndex = -1;

            for (int i = 0; i < Size; i++)
            {
                if (_items[i] == null) firstIndex = i; 
                break;               
            }

            if (firstIndex == -1) return false;            
            this[firstIndex] = item; return true;
        }

        bool IAssortment<T>.AddAt(int index, T item)
        {
            if (IsOutOfArray(index)) return false;
            if (_items[index] != null) return false;

            if (this[index] == null) return false;
            this[index] = item; return true;
        }

        T? IAssortment<T>.At(int index)
        {
            throw new NotImplementedException();
        }

        T? IAssortment<T>.Find(int id)
        {
            throw new NotImplementedException();
        }

        T? IAssortment<T>.Find(string name)
        {
            throw new NotImplementedException();
        }

        bool IAssortment<T>.Replace(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IAssortment<T>.SortById()
        {
            throw new NotImplementedException();
        }

        void IAssortment<T>.SortByName()
        {
            throw new NotImplementedException();
        }
    }
}
