

using QR;

namespace Products
{
    public class PackageQRController : IQRController
    {
        /// <summary>
        /// Базовый QR
        /// </summary>
        private readonly IQRController _baseQR = new QRController();

        /// <summary>
        /// Передача и получение значения в базовый QR
        /// </summary>
        public string Text {
            get => _baseQR.Text;
            set => _baseQR.Text = value;
        }
        
        /// <summary>
        /// Получение кода из базового QR
        /// </summary>
        public string Code { get => _baseQR.Code; }

        /// <summary>
        /// Вывод информации QR
        /// </summary>
        public override string ToString() => IQRController.OUTPUT_MODE switch
        {
            OutputMode.Text => $"*{Text}*",
            OutputMode.Full => $"*{this.Text}*\n" + $"{this.Code}",
            _ => Code
        };
    }
}
