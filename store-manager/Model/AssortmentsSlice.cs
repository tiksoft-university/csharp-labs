using Assortment;
using Products;

namespace store_manager.Model
{
    static class MockAssortmentsData
    {
        public static List<IAssortment<IProduct>> Get()
        {
            List<IAssortment<IProduct>> mockData = new();
            
            Assortment<IProduct> assortment_1 = (1, 10);
            Assortment<IProduct> assortment_2 = (2, 10);
            Assortment<IProduct> assortment_3 = (3, 10);
            
            mockData.Add(assortment_1);
            assortment_1.Add(new PowerTool(145, "Товар_145", 1000, "Описание 1"));
            assortment_1.Add(new Package(new PowerTool(12, "Товар_12", 1000, "Описание 1")));
            assortment_1.Add(new PowerTool(15, "Товар_15", 1000, "Описание 1"));
            assortment_1.Add(new PowerTool(17, "Товар_17", 1000, "Описание 1"));       

            mockData.Add(assortment_2);
            assortment_2.Add(new PowerTool(11, "Товар_11", 1000, "Описание 1"));
            assortment_2.AddAt(8, new PowerTool(15, "Товар_15", 1000, "Описание 1"));
            assortment_2.AddAt(6, new PowerTool(17, "Товар_17", 1000, "Описание 1"));
            assortment_2.Add(new PowerTool(12, "Товар_12", 1000, "Описание 1"));

            mockData.Add(assortment_3);
            assortment_3.Add(new PowerTool(11, "Товар_11", 1000, "Описание 1"));
            assortment_3.Add(new PowerTool(15, "Товар_11", 1000, "Описание 1"));
            assortment_3.Add(new PowerTool(17, "Товар_11", 1000, "Описание 1"));
            assortment_3.Add(new PowerTool(12, "Товар_11", 1000, "Описание 1"));

            return mockData;
        }
    }
    internal sealed class AssortmentsSlice
    {
        private readonly List<IAssortment<IProduct>> _data = new();
        private int _selectedAssortmentIndex = 0;
        private int _selectedProductIndex = 0;

        public int SelectedAssortmentIndex
        {
            get => _selectedAssortmentIndex;
            set
            {
                if (value == _selectedAssortmentIndex) return;
                if (value < 0 || value > _data.Count - 1) return;
                _selectedAssortmentIndex = value;
            }
        }
  
        public int SelectedProductIndex { 
            get => _selectedProductIndex;
            set 
            {
                if(value == _selectedProductIndex || value < 0) return;
                if(value > _data[SelectedAssortmentIndex].Size - 1) return;
                _selectedProductIndex = value;
            }
        }
 
        public AssortmentsSlice()
        {
            _data = MockAssortmentsData.Get();
        }
        
        /// <summary>Сортировка по Id</summary>
        public void SortProductsById() => _data[SelectedAssortmentIndex]?.SortById();

        /// <summary>Сортировка по имени</summary>
        public void SortProductsByName() => _data[SelectedAssortmentIndex]?.SortByName();

        /// <summary>Возвращает ссылку на экземпляр выбранной витрины</summary>
        public IAssortment<IProduct>? GetSelectedAssortment() => _data[SelectedAssortmentIndex];

        /// <summary>Возвращает ссылку на экземпляр выбранного товара</summary>
        public IProduct? GetSelectedProduct()
        {
            var selectedAssortment = GetSelectedAssortment();
            if (selectedAssortment is null) return null;
            return selectedAssortment.At(SelectedProductIndex);
        }

        /// <summary>Упаковать/Запаковать товар</summary>
        public void TogglePackage()
        {
            var selectedAssortment = GetSelectedAssortment();
            IProduct? selectedProduct = GetSelectedProduct();                         
            
            if (selectedProduct is null) return;
            
            if(selectedProduct is Package)
            {
                selectedAssortment?.Replace(
                    SelectedProductIndex,
                    new PowerTool(
                        selectedProduct.Id, 
                        selectedProduct.Name, 
                        selectedProduct.Price / (decimal)1.1, 
                        selectedProduct.Info
                    )
                );
                return;
            }

            selectedAssortment?.Replace(
                SelectedProductIndex,
                new Package(selectedProduct)
            );
        }

        public string GetStringifiedSelectedProduct()
        {
            // TODO: ПЕРЕИСПОЛЬЗОВАТЬ GET_SELECTED_PRODUCT
            var selectedAssortment = _data[SelectedAssortmentIndex];                  
            if(selectedAssortment is null) return "Товар не найден";           

            IProduct? product = selectedAssortment.At(SelectedProductIndex);

            if(product is null) return "В данной ячейке нет товара";
            return product.ToString()!;
        }

        public bool IsPackaged(int assortmentId, int productId)
        {
            var targetAssortment = _data.FirstOrDefault(asrt => asrt.Id == assortmentId);
            if(targetAssortment is null) return false;
            return targetAssortment.IsPackagedById(productId);
        }        

        public List<int> GetAssortmentIds() => _data.Select(p => p.Id).ToList();

        public List<int?> GetProductsIds(int id)
        {
            var assortment = _data.FirstOrDefault(p => p.Id == id);
            if (assortment is null) return [];
            return assortment.GetProductsIds();
        }
    }
}
