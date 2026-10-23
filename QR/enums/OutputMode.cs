using System;
using System.Collections.Generic;
using System.Text;

namespace QR
{

    /// <summary>
    /// Формат вывода QR-кода
    /// </summary>
    public enum OutputMode
    {
        /// <summary>
        /// Текстовая информация
        /// </summary>
        Text,

        /// <summary>
        /// QR-код
        /// </summary>
        QrCode,

        /// <summary>
        /// Полная информация: QR-код + текст
        /// </summary>
        Full
    }
}
