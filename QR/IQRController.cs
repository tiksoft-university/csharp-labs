namespace QR
{
    /// <summary>
    /// Интерфейс, декларирующий функциональность QR-кода
    /// </summary>
    public interface IQRController
    {
        /// <summary>
        /// Способ вывода информации
        /// </summary>
        public static OutputMode OUTPUT_MODE = OutputMode.Full;

        /// <summary>
        /// Текст для кодирования
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Строчное представление QR
        /// </summary>
        public string? Code { get; }
    }
}
