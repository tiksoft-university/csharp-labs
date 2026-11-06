using Products;
using System.Xml.Linq;

namespace Assortment
{
    public sealed class SmartAssortment<T> : Assortment<T> where T : class, IProduct
    {

        /// <summary>Делегат</summary>
        private Action<SmartAssortment<T>>? _onIdchange;

        /// <summary>Конструктор</summary>
        public SmartAssortment(int id, int size) : base(id, size)
        {
            Id = id;
        }

        /// <summary>Подписка на изменение ID</summary>
        public void RegisterOnIdChange(Action<SmartAssortment<T>> callback) => _onIdchange += callback;

        /// <summary>Отпиоска от изменения ID</summary>
        public void UnregisterOnIdChange(Action<SmartAssortment<T>> callback) => _onIdchange -= callback;
        
        /// <summary>Идентификатор умного склада</summary>
        public override int Id
        {
            get => base.Id;
            set
            {
                base.Id = value;
                _onIdchange?.Invoke(this);
            }
        }

        /// <summary>Индексатор c подписками</summary>
        public override T? this[int index]
        {
            get
            {
                T? gettedProduct = base[index];
                if (gettedProduct is null) return gettedProduct;

                // Тут отписываем удаленный элемент, чтобы он больше не следил за изменением
                UnregisterOnIdChange(gettedProduct.SetAssortmentCode);

                return gettedProduct;
            }
            set
            {
                T? prevProduct = base[index];
                
                // Если мы заносим тот же самый продукт или пустой
                if(prevProduct == value || value is null) return;

                // Если уже лежал другой, не меняем
                if (prevProduct != null) { base[index] = prevProduct; return; }                                               
            
                base[index] = value;
                RegisterOnIdChange(value.SetAssortmentCode);
            }
        }

        /// <summary>Универсальная сортировка</summary>
        private void Sort(Func<T, object> callback)
        {
            var bufferItems = new T?[Size];

            for (int i = 0; i < bufferItems.Length; i++) bufferItems[i] = this[i];

            var sorted = bufferItems
                .OrderBy(x => x == null)
                .ThenBy(x => x != null ? callback(x) : default)
                .ToArray();

            for (int i = 0; i < sorted.Length; i++) this[i] = sorted[i];            
        }

        /// <summary>Универсальный поиск</summary>
        private T? Find(Predicate<T> callback)
        {
            var bufferItems = new T?[Size];

            for (int i = 0; i < bufferItems.Length; i++)
            {
                bufferItems[i] = this[i];
                this[i] = bufferItems[i];
            }

            return bufferItems.FirstOrDefault(item => item != null && callback(item), null);
        }


        /// <summary>Переопределенная сортировка по возрастанию Id</summary>
        public override void SortById() => Sort(product => product.Id);

        /// <summary>Переопределенная сортировка по возрастанию Name</summary>
        public override void SortByName() => Sort(product => product.Name);
  
        /// <summary>Переопределенный поиск Id</summary>
        public override T? Find(int id) => Find(product => product.Id == id);

        /// <summary>Переопределенный поиск по Name</summary>
        public override T? Find(string name) => Find(product => product.Name == name);

    }
}
