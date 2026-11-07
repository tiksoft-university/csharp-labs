using Products;

namespace SmartStorage
{
    public interface ISmartProduct : IProduct
    {
        public event EventHandler<EventIdPriceChangedArgs>? EventIdPriceOrChanged;
    }
}
