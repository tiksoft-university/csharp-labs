using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStorage
{
    public sealed class EventIdPriceChangedArgs : EventArgs
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        public EventIdPriceChangedArgs(int id, decimal price)
        {
            Id = id;
            Price = price;
        }
    }
}
