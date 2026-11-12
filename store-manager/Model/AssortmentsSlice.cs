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
            assortment_1.Add(new PowerTool(145, "Товар_14", 1000, "Описание 1"));
            assortment_1.Add(new PowerTool(12, "Товар_12", 1000, "Описание 1"));
            assortment_1.Add(new PowerTool(15, "Товар_15", 1000, "Описание 1"));
            assortment_1.Add(new PowerTool(17, "Товар_17", 1000, "Описание 1"));       

            mockData.Add(assortment_2);
            assortment_2.Add(new PowerTool(11, "Товар_11", 1000, "Описание 1"));
            assortment_2.Add(new PowerTool(15, "Товар_11", 1000, "Описание 1"));
            assortment_2.Add(new PowerTool(17, "Товар_11", 1000, "Описание 1"));
            assortment_2.Add(new PowerTool(12, "Товар_11", 1000, "Описание 1"));

            return mockData;
        }
    }
    internal sealed class AssortmentsSlice
    {
        private const int ASSORTMENTS_MAX_COUNT = 3;
        private int? _selectedAssortmentId = null;
        private int? _selectedProductId = null;
        public int SelectedProductIndex { get; set; }
        public int SelectedAssortmentIndex { get; set; }
        private List<IAssortment<IProduct>> _data = new();

        public AssortmentsSlice()
        {
            _data = MockAssortmentsData.Get();
            SetSelectedAssortmentId(1);
            SelectedProductIndex = 8;
        }       

        public string GetStringifiedSelectedProduct()
        {
            if(_selectedAssortmentId is null) return "Товар не найден";

            var selectedAssortment = _data.FirstOrDefault(assortment => assortment.Id == _selectedAssortmentId);            
            if(selectedAssortment is null) return "Товар не найден";           

            IProduct? product = selectedAssortment.At(SelectedProductIndex);

            if(product is null) return "В данной ячейке нет товара";
            return product.ToString()!;
        }

        public void SetSelectedAssortmentId(int id) => _selectedAssortmentId = id;
        public int? GetSelectedAssortmentId() => _selectedAssortmentId;
        public int GetAssortmentsMaxCount() => ASSORTMENTS_MAX_COUNT;

        public int? GetSelectedProductId() => _selectedProductId;

        public List<int> GetAssortmentIds() => _data.Select(p => p.Id).ToList();

        public List<int?> GetProductsIds(int id)
        {
            var assortment = _data.FirstOrDefault(p => p.Id == id);
            if (assortment is null) return [];
            return assortment.GetProductsIds();
        }

        public bool Add(IAssortment<IProduct> assortment)
        {
            if (_data.Count < ASSORTMENTS_MAX_COUNT)
            {
                _data.Add(assortment);
                return true;
            }

            return false;
        }
    }
}
