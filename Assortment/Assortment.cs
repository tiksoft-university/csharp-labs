using Products;
using System.Text;

namespace Assortment
{
    public class Assortment<T> : IAssortment<T> where T : class, IProduct {

        /// <summary>Идентификатор склада</summary>
        private int _id;
        public virtual int Id {
            get => _id;
            set {
                _id = value;
                UpdateAllCodes(); 
            }
        }

        /// <summary>Товары склада</summary>
        private T[] _items;


        /// <summary>Защищенный конструктор</summary>
        protected Assortment(int id, int size) {
            if (size <= 0) return;
            _items = new T[size];
            Id = id;
        }

        /// <summary>Перегрузка оператора неявного преобразования из числа</summary>
        public static implicit operator Assortment<T>(int size) => new Assortment<T>(1, size);

        /// <summary>Перегрузка оператора неявного преобразования из кортежа</summary>
        public static implicit operator Assortment<T>((int id, int size) args) => new Assortment<T>(args.id, args.size);

        /// <summary>Индексатор получения</summary>
        public virtual T? this[int index] {
            get
            {
                if (IsOutOfArray(index) || _items[index] == null) return null;

                T? item = _items[index]; _items[index] = null;

                return item;
            }
            set
            {
                // Если индекс выходит за пределы
                if (IsOutOfArray(index) || this[index] != null || _items[index] == value || value is null) return;
                _items[index] = value;
                UpdateAllCodes();
            }
        }

        /// <summary>Размер склада</summary>
        public int Size { get => _items.Length; }

        /// <summary>Проверка, что индекс выходит за границы массива</summary>
        protected bool IsOutOfArray(int index) => index < 0 || index > _items.Length - 1;

        /// <summary>Добавление товара в первый свободный слот</summary>
        public bool Add(T item)
        {
            var firstIndex = -1;

            for (int i = 0; i < Size; i++)
            {
                if (_items[i] == null)
                {
                    firstIndex = i; break;
                }

            }

            if (firstIndex == -1) return false;
            this[firstIndex] = item; return true;
        }

        /// <summary>Добавление товара по конкретному индексу</summary>
        public bool AddAt(int index, T item)
        {
            if (IsOutOfArray(index) || _items[index] != null) return false;
            this[index] = item; return true;
        }


        /// <summary>Поменять местами</summary>
        public void Swap(int first, int second)
        {
            if (IsOutOfArray(first) || IsOutOfArray(second) || first == second) return;

            var firstProd = this[first];
            var secondProd = this[second];

            this[first] = secondProd;
            this[second] = firstProd;
        }

        /// <summary>Замена</summary>
        public bool Replace(int index, T item)
        {
            if (IsOutOfArray(index) || this[index] == item) return false;
            this[index] = item; return true;
        }

        /// <summary>Поиск товара по идентификатору</summary>
        public virtual T? Find(int id) => _items.FirstOrDefault(item => item?.Id == id, null);

        /// <summary>Поиск товара по имени</summary>
        public virtual T? Find(string name) => _items.FirstOrDefault(item => item?.Name == name, null);

        /// <summary>Обновление кодов всех товаров склада</summary>
        private void UpdateAllCodes() {
            for (int i = 0; i < Size; i++) {
                if (_items[i] != null) _items[i]!.SetAssortmentCode(this);
            };
        }

        /// <summary>Сортировка по возрастанию Id</summary>
        public virtual void SortById() {
            _items = _items.OrderBy(x => x == null).ThenBy(x => x?.Id).ToArray();
            UpdateAllCodes();
        }

        /// <summary>Сортировка по возрастанию Name</summary>
        public virtual void SortByName()
        {
            _items = _items.OrderBy(x => x == null).ThenBy(x => x?.Name).ToArray();
            UpdateAllCodes();
        }

        /// <summary>Переопределенное приведение к строке</summary>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"\n ------------------ ASSORTMENT {this._id} ------------------ : \n\n");

            foreach (var item in _items) {
                if(item == null)
                {
                    result.Append("null \n");
                    
                } else
                {
                    result.Append($"{item.ToString()}");
                }                
            }

            return $"{result}\n";
        }
    }
}
