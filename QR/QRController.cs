namespace QR
{
    public class QRController : IQRController
    {     
        /// <summary>
        /// Версия QR-кода
        /// </summary>
        private Version version = Version.V1;

        /// <summary>
        /// Режим кодирования
        /// </summary>
        private EncodingMode encodingMode = EncodingMode.Binary;

        /// <summary>
        /// Уровень коррекции ошибки. Может быть null
        /// </summary>
        private ErrorCorrectionLevel? errorCorrectionLevel = ErrorCorrectionLevel.M;
        
        /// <summary>
        /// Маска QR-кода. Может быть null
        /// </summary>
        private Mask? mask = Mask.M_001;

        /// <summary>
        /// Строковое представление QR-кода
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Текст, который необходимо закодировать
        /// </summary>
        /// 
        private string _text = string.Empty;
        public string Text { 
            get => _text; 
            set {
                if (value == null || value == _text) return;

                _text = value;
                Code = QrBuilder.ToQrCode(
                    _text, 
                    ref this.version, 
                    ref this.encodingMode, 
                    ref this.errorCorrectionLevel, 
                    ref this.mask
                );
            }                           
        }

        /// <summary>
        /// Получение информации о QR-коде
        /// </summary>        
        public string GetInfoByQR() => $"{this.version}, {this.mask}, {this.errorCorrectionLevel}";

        /// <summary>
        /// Вывод информации
        /// </summary>      
        public override string ToString() => IQRController.OUTPUT_MODE switch
        {
            OutputMode.Text => Text,
            OutputMode.Full => $"{this.Text}\n" + $"{this.GetInfoByQR()}\n" + $"{this.Code}",
            _ => Code
        };

    }
}
