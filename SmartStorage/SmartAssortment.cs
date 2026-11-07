using Products;
using SmartStorage;

namespace Assortment
{
    
    public sealed class SmartAssortment<T> : Assortment<T>, IDisposable where T : class, ISmartProduct
    {

        /// <summary>Делегат</summary>
        private Action<SmartAssortment<T>>? _onIdchange;

        /// <summary>Конструктор</summary>
        private SmartAssortment(int id, int size) : base(id, size)
        {
            Id = id;
        }

        /// <summary>Перегрузка оператора неявного преобразования из числа</summary>
        public static implicit operator SmartAssortment<T>(int size) => new SmartAssortment<T>(1, size);

        /// <summary>Перегрузка оператора неявного преобразования из кортежа</summary>
        public static implicit operator SmartAssortment<T>((int id, int size) args) => new SmartAssortment<T>(args.id, args.size);

        /// <summary>Подписка на изменение ID</summary>
        private void RegisterOnIdChange(Action<SmartAssortment<T>> callback)  {
            _onIdchange += callback;
        }

        /// <summary>Отпиоска от изменения ID</summary>
        private void UnregisterOnIdChange(Action<SmartAssortment<T>> callback) {
            _onIdchange -= callback;
        }

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
                Unsubcribe(gettedProduct);

                return gettedProduct;
            }
            set
            {
                T? prevProduct = base[index];

                // Если мы заносим тот же самый продукт или пустой
                if (prevProduct == value || value is null) return;

                // Если уже лежал другой, не меняем
                if (prevProduct != null) { base[index] = prevProduct; return; }

                //base[index] = value;
                Subscribe(value);
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


        public void Subscribe(T product)
        {
            if (product is null) return;
            RegisterOnIdChange(product.SetAssortmentCode);
            product?.EventIdPriceOrChanged += OnProductChange;
        }


        public void Unsubcribe(T product)
        {
            if (product is null) return;
            UnregisterOnIdChange(product.SetAssortmentCode);
            product?.EventIdPriceOrChanged -= OnProductChange;
        }


        public void OnProductChange(object? sender, EventIdPriceChangedArgs e) { 
            if (sender is T) RefreshProduct(sender as T);            
        }

        private void RefreshProduct(T product)
        {
            if (product == null) return;
            product.SetAssortmentCode(this);
        }

        /// <summary>Переопределенная сортировка по возрастанию Id</summary>
        public override void SortById() => Sort(product => product.Id);

        /// <summary>Переопределенная сортировка по возрастанию Name</summary>
        public override void SortByName() => Sort(product => product.Name);

        /// <summary>Переопределенный поиск Id</summary>
        public override T? Find(int id) => Find(product => product.Id == id);

        /// <summary>Переопределенный поиск по Name</summary>
        public override T? Find(string name) => Find(product => product.Name == name);

        public void Dispose()
        {
            for (int i = 0; i < Size; i++) Unsubcribe(this[i]);            
        }
    }
}
