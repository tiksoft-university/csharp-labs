using QR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{
    public class PowerTool : Product
    {
        
        /// <summary>
        /// Тип товара
        /// </summary>
        private string _type = "Дрель";
       

        /// <summary>
        /// Информация о товаре
        /// </summary>
        private string _info;

        /// <summary>
        ///  Конструктор продукта
        /// </summary>
        /// 

        public PowerTool(
            int id, 
            string name, 
            decimal price, 
            string info = "Информация о товаре"
        ) : base(id, name, price, new QRController())
        {
            _info = info;
        }

        /// <summary>
        /// Геттер типа
        /// </summary>
        public override string Type { get => _type; }
       
        /// <summary>
        /// 
        /// </summary>
        public override string Info { get => _info; }

    }
}
