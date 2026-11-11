namespace Assortment
{
    /// <summary>
    /// Интерфейс контейнера склада
    /// </summary>
    public interface IAssortment<T>
    {
        /// <summary> Идентификатор склада </summary>
        public int Id { get; set; }

        /// <summary> Размер склада </summary>
        public int Size { get; }

        /// <summary> Индексатор для товара </summary>
        public T? this[int index] { get; set; }

        /// <summary> Добавление товара в первую пустую ячейку </summary>
        public bool Add(T item);
      
        /// <summary> Добавление товара в ячейку по индексу</summary>
        public bool AddAt(int index, T item);

        /// <summary> Получение товара по индексу</summary>
        public void Swap(int first, int second);

        /// <summary>Замена по индексу</summary>
        public bool Replace(int index, T item);

        /// <summary>Замена по индексу</summary>
        public int IndexOf(T item);

        /// <summary>Поиск по идентификатору</summary>
        public T? Find(int id);
        
        /// <summary>Поиск по имени</summary>
        public T? Find(string name);
        
        /// <summary>Сортировка по возрастанию по идентификатору</summary>
        public void SortById();

        /// <summary>Сортировка по возрастанию по наименованию</summary>
        public void SortByName();

        /// <summary>Получить список идентификаторов товаров</summary>
        public List<int?> GetProductsIds();
    }
}
