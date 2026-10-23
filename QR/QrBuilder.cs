using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QR
{
    internal static class QrBuilder
    {
        #region GET 

        /// <summary> 
        ///  ToDo Необходимо восстановить функцию, она почему то сейчас возвращает не QR-код, а исходный текст
        /// </summary> 
        public static string ToQrCode(
            this string text, 
            ref Version qrCodeVersion, 
            ref EncodingMode codeType, 
            ref ErrorCorrectionLevel? needCorrectionLevel, 
            ref Mask? maskNum, 
            bool invert = false
        )
        {
            // ─── Этап 1 ─────────────────────────────────────────────
            // 1.1 Определяем способ кодирования
            var encode = Magic(text, codeType);

            // 1.2 Определяем подходящий под настройки: уровень коррекции ошибок и версию кода, а так же сам текст для кодирования со служебной информацией
            var (fullText, ecc, version) = Magic(text, encode, codeType, qrCodeVersion, needCorrectionLevel);
            qrCodeVersion = version;
            needCorrectionLevel = ecc;

            // ─── Этап 2 ─────────────────────────────────────────────
            // 2.1 Определяем длину из словаря 
            var dictionaryLen = _maxData[(needCorrectionLevel.Value, qrCodeVersion)];

            // 2.2 На основе текста для кодирования из 1.2 и длины - определяем полный текст для кодирования
            var fullTextEncoding = Magic(fullText, dictionaryLen);

            // 2.3 По версии кода и уровню коррекции ошибок - получаем из словаря количество блоков 
            var blockCount = _correctionLevelBlocksCount[needCorrectionLevel.Value][(int)qrCodeVersion];

            // 2.4 Формируем список блоков из данных 2.2 и 2.3 
            var blocksList = Magic(fullTextEncoding, blockCount, 0);

            // 2.5 Пустой список блоков коррекции
            var eccBlocks = new List<byte[]>();

            // 2.6 В цикле по блокам из 2.4: 
            var eccLen = _countOfErrorCorrectionCodeWords[needCorrectionLevel.Value][(int)qrCodeVersion];
            foreach (var block in blocksList)
            {
                // 2.6.1 Определяем размер из справочнике на основе данных уровня коррекции и версии кода. Размер ECC — eccLen 
                // 2.6.2 Добавляем в список 2.5 блок коррекции, полученный на основе блока 2.6 и размера из 2.6.1
                eccBlocks.Add(Magic(block, (byte)eccLen));
            }

            // 2.7 На основе списка блоков 2.4 и блоков коррекции 2.5 получаем строку с данными
            var interleaved = Magic(blocksList, eccBlocks);

            // ─── Этап 3 ─────────────────────────────────────────────
            // 3.1 Инициализируем блок данных, с версией, уровнем коррекции и данными 2.7 
            var qrData = new QrData
            {
                Version = qrCodeVersion,
                CorrectionLevel = needCorrectionLevel.Value,
                Data = interleaved
            };

            // 3.2 В зависимости от наличии определенной маски формируем матрицу кода, которая будет представлена в виде списка блоков
            List<byte[]> matrix;
            if (maskNum.HasValue)
                matrix = qrData.Magic(maskNum.Value);       // 3.2.1 Если макса задана, передаем ее номер 
            else
                matrix = qrData.Magic(ref maskNum);        // 3.2.2 Если маска не задана, передаем ссылку на этот входной параметр, и функция помимо формирования матрицы определит оптимальный номер

            // ─── Этап 4 ─────────────────────────────────────────────
            // 4.1 На основе данных по матрице и флага инвертирования, формируем готовую строку кода 
            return matrix.Magic(invert);
        }

        #endregion

        #region Magic 

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static string Magic(string a, EncodingMode b)
        {
            var sb = new StringBuilder();
            var tmp = b switch
            {
                EncodingMode.AlphaNumeric => Magic2(a.ToUpper()),
                EncodingMode.Numeric => Magic3(a),
                _ => Magic4(a)
            };
            foreach (var str in tmp)
                sb.Append(str);
            return sb.ToString();
        }

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static (string a, ErrorCorrectionLevel b, Version c) Magic(
            string a, 
            string b, 
            EncodingMode c, 
            Version d,
            ErrorCorrectionLevel? e = null
        )
        {
            if (d == NA)
                throw new NotSupportedException("QR-code version start with 1!");

            var sb = new StringBuilder();
            sb.Magic(c, d, a)
              .Append(b)
              .Magic();

            var length = sb.Length;

            if ((int)d > 20)
                throw new NotSupportedException($"Current QR-code does not support version {d} yet!"); 
    

        if (e.HasValue)
            {
                foreach (var found in _maxData
                    .Where(v => v.Key.version >= d && v.Key.correctionLevel >= e.Value)
  

                .Where(l => length < l.Value)) 
                return (sb.ToString(), found.Key.correctionLevel, found.Key.version);
            }

            foreach (var found in _maxData
                .Where(v => v.Key.version == d)
                .OrderByDescending(x => x.Key.correctionLevel)
                .Where(x => length < x.Value))
                return (sb.ToString(), found.Key.correctionLevel, found.Key.version);

            if ((int)d > 20)
                throw new NotSupportedException($"Current QR-code does not support data length { length } yet!"); 
    

        return Magic(a, b, c, d + 1, e);
        }

        private static readonly char[] letterNumberArray = {
                                                    '0','1','2','3','4','5','6','7','8','9',
                                                    'A','B','C','D','E','F','G','H','I','J',
                                                    'K','L','M','N','O','P','Q','R','S','T',
                                                    'U','V','W','X','Y','Z',' ','$','%','*',
                                                    '+','-','.','/',':'
                                                };

        private static void Magic(List<string> a, int b, byte c = 8)
        {
            var str = Convert.ToString(b, 2);
            a.Add(str.PadLeft(c, '0'));
        }

        private static List<string> Magic2(string a)
        {
            var res = new List<string>();
            var pos = 0;
            while (pos < a.Length - 2)
            {
                var index1 = Array.IndexOf(letterNumberArray, a[pos]);
                var index2 = Array.IndexOf(letterNumberArray, a[pos + 1]);
                Magic(res, index1 * 45 + index2, 11);
                pos += 2;
            }
            if (a.Length % 2 == 1)
            {
                Magic(res, Array.IndexOf(letterNumberArray, a[^1]), 6);
            }
            return res;
        }

        private static List<string> Magic3(string a)
        {
            var res = new List<string>();
            var pos = 0;
            var length = a.Length;
            while (pos < a.Length - 3)
            {
                var number = a.Substring(pos, 3);
                Magic(res, Convert.ToInt32(number));
                pos += 3;
            }
            if (a.Length % 3 == 2)
            {
                Magic(res, Convert.ToInt32(a.Substring(length - 2, 2)), 7);
            }
            else if (a.Length % 3 == 1)
            {
                Magic(res, Convert.ToInt32(a.Substring(length - 1, 1)), 4);
            }
            return res;
        }

        private static List<string> Magic4(string a)
        {
            var res = new List<string>();
            var numbers = Encoding.UTF8.GetBytes(a);
            foreach (var number in numbers)
            {
                Magic(res, Convert.ToInt32(number));
            }
        
        return res;
        }

        private static readonly Dictionary<EncodingMode, string> _codeTypeMode = new()
    {
        {EncodingMode.Numeric,      "0001"},
        {EncodingMode.AlphaNumeric, "0010"},
        {EncodingMode.Binary,       "0100"}
    };

        private static byte Magic(EncodingMode a, Version b)
        {
            return ((int)b, a) switch
            {
                ( < 10, EncodingMode.Numeric) => 10,
                ( < 10, EncodingMode.AlphaNumeric) => 9,
                ( < 10, EncodingMode.Binary) => 8,
                ( < 27, EncodingMode.Numeric) => 12,
                ( < 27, EncodingMode.AlphaNumeric) => 11,
                ( < 27, EncodingMode.Binary) => 16,
                ( < 27, _) => 10,
                (_, EncodingMode.Numeric) => 14,
                (_, EncodingMode.AlphaNumeric) => 13,
                (_, EncodingMode.Binary) => 16,
                (_, _) => 12,
            };
        }

        private static string Magic(EncodingMode a, Version b, string c)
        {
            var length = a switch
            {
                EncodingMode.Binary => Encoding.UTF8.GetBytes(c).Length,
                _ => c.Length,
            };
            var size = Magic(a, b);
            var str = Convert.ToString(length, 2).PadLeft(size, '0');
            return str;
        }

        private static StringBuilder Magic(this StringBuilder a, EncodingMode b, Version c, string d)
        {
            return a.Append(Magic1(b, c, d));
        }

        private static string Magic1(EncodingMode a, Version b, string c)
        {
            return _codeTypeMode[a] + Magic(a, b, c);
        }

        private static readonly string[] _magicTextArray = ["11101100", "00010001"];

        private static StringBuilder Magic(this StringBuilder a)
        {
            var res = string.Empty.PadLeft(a.Length % 8, '0');
            if (res.Length > 0)
                a.Append(res);
            return a;
        }

        #endregion

        #region Magic 

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static readonly Dictionary<(ErrorCorrectionLevel correctionLevel, Version version), int> _maxData =
    new()
        {
        {(ErrorCorrectionLevel.H, Version.V1),    72}, {(ErrorCorrectionLevel.Q, Version.V1),   104}, {(ErrorCorrectionLevel.M, Version.V1),   128},
{(ErrorCorrectionLevel.L, Version.V1),   152},
        {(ErrorCorrectionLevel.H, Version.V2),   128}, {(ErrorCorrectionLevel.Q, Version.V2),   176}, {(ErrorCorrectionLevel.M, Version.V2),   224},
{(ErrorCorrectionLevel.L, Version.V2),   272},
        {(ErrorCorrectionLevel.H, Version.V3),   208}, {(ErrorCorrectionLevel.Q, Version.V3),   272}, {(ErrorCorrectionLevel.M, Version.V3),   352},
{(ErrorCorrectionLevel.L, Version.V3),   440},
        {(ErrorCorrectionLevel.H, Version.V4),   288}, {(ErrorCorrectionLevel.Q, Version.V4),   384}, {(ErrorCorrectionLevel.M, Version.V4),   512},
{(ErrorCorrectionLevel.L, Version.V4),   640},
        {(ErrorCorrectionLevel.H, Version.V5),   368}, {(ErrorCorrectionLevel.Q, Version.V5),   496}, {(ErrorCorrectionLevel.M, Version.V5),   688},
{(ErrorCorrectionLevel.L, Version.V5),   864},
        {(ErrorCorrectionLevel.H, Version.V6),   480}, {(ErrorCorrectionLevel.Q, Version.V6),   608}, {(ErrorCorrectionLevel.M, Version.V6),   864},
{(ErrorCorrectionLevel.L, Version.V6),  1088},
        {(ErrorCorrectionLevel.H, Version.V7),   528}, {(ErrorCorrectionLevel.Q, Version.V7),   704}, {(ErrorCorrectionLevel.M, Version.V7),   992},
{(ErrorCorrectionLevel.L, Version.V7),  1248},
        {(ErrorCorrectionLevel.H, Version.V8),   688}, {(ErrorCorrectionLevel.Q, Version.V8),   880}, {(ErrorCorrectionLevel.M, Version.V8),  1232},
{(ErrorCorrectionLevel.L, Version.V8),  1552},
        {(ErrorCorrectionLevel.H, Version.V9),   800}, {(ErrorCorrectionLevel.Q, Version.V9),  1056}, {(ErrorCorrectionLevel.M, Version.V9),  1456},
{(ErrorCorrectionLevel.L, Version.V9),  1856},
        {(ErrorCorrectionLevel.H, Version.V10),  976}, {(ErrorCorrectionLevel.Q, Version.V10), 1232}, {(ErrorCorrectionLevel.M, Version.V10), 1728},
{(ErrorCorrectionLevel.L, Version.V10), 2192},
        {(ErrorCorrectionLevel.H, Version.V11), 1120}, {(ErrorCorrectionLevel.Q, Version.V11), 1440}, {(ErrorCorrectionLevel.M, Version.V11), 2032},
{(ErrorCorrectionLevel.L, Version.V11), 2592},
        {(ErrorCorrectionLevel.H, Version.V12), 1264}, {(ErrorCorrectionLevel.Q, Version.V12), 1648}, {(ErrorCorrectionLevel.M, Version.V12), 2320},
{(ErrorCorrectionLevel.L, Version.V12), 2960},
        {(ErrorCorrectionLevel.H, Version.V13), 1440}, {(ErrorCorrectionLevel.Q, Version.V13), 1952}, {(ErrorCorrectionLevel.M, Version.V13), 2672},
{(ErrorCorrectionLevel.L, Version.V13), 3424},
        {(ErrorCorrectionLevel.H, Version.V14), 1576}, {(ErrorCorrectionLevel.Q, Version.V14), 2088}, {(ErrorCorrectionLevel.M, Version.V14), 2920},
{(ErrorCorrectionLevel.L, Version.V14), 3688},
        {(ErrorCorrectionLevel.H, Version.V15), 1784}, {(ErrorCorrectionLevel.Q, Version.V15), 2360}, {(ErrorCorrectionLevel.M, Version.V15), 3320},
{(ErrorCorrectionLevel.L, Version.V15), 4184},
        {(ErrorCorrectionLevel.H, Version.V16), 2024}, {(ErrorCorrectionLevel.Q, Version.V16), 2600}, {(ErrorCorrectionLevel.M, Version.V16), 3624},
{(ErrorCorrectionLevel.L, Version.V16), 4712},
        {(ErrorCorrectionLevel.H, Version.V17), 2264}, {(ErrorCorrectionLevel.Q, Version.V17), 2936}, {(ErrorCorrectionLevel.M, Version.V17), 4056},
{(ErrorCorrectionLevel.L, Version.V17), 5176},
        {(ErrorCorrectionLevel.H, Version.V18), 2504}, {(ErrorCorrectionLevel.Q, Version.V18), 3176}, {(ErrorCorrectionLevel.M, Version.V18), 4504},
{(ErrorCorrectionLevel.L, Version.V18), 5768},
        {(ErrorCorrectionLevel.H, Version.V19), 2728}, {(ErrorCorrectionLevel.Q, Version.V19), 3560}, {(ErrorCorrectionLevel.M, Version.V19), 5016},
{(ErrorCorrectionLevel.L, Version.V19), 6360},
        {(ErrorCorrectionLevel.H, Version.V20), 3080}, {(ErrorCorrectionLevel.Q, Version.V20), 3880}, {(ErrorCorrectionLevel.M, Version.V20), 5352},
{(ErrorCorrectionLevel.L, Version.V20), 6888},
        };

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static string Magic(string a, int b)
        {
            var sb = new StringBuilder(a);

            var cnt = (b - a.Length) / 8;
            for (int i = 0; i < cnt; i++)
            {
                sb.Append(_magicTextArray[i % 2]);
            }
            return sb.ToString();
        }

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static readonly Dictionary<ErrorCorrectionLevel, byte[]> _correctionLevelBlocksCount = new()
    {
        {ErrorCorrectionLevel.L, [NA,01,01,01,01,01,02,02,02,02,04,04,04,04,04,06,06,06,06,07,08]},
        {ErrorCorrectionLevel.M, [NA,01,01,01,02,02,04,04,04,05,05,05,08,09,09,10,10,11,13,14,16]},
        {ErrorCorrectionLevel.Q, [NA,01,01,02,02,04,04,06,06,08,08,08,10,12,16,12,17,16,18,21,20]},
        {ErrorCorrectionLevel.H, [NA,01,01,02,04,04,04,05,06,08,08,11,11,16,16,18,16,19,21,25,25]},
    };

        /// <summary> 
        /// Magic 
        /// </summary> 
        public static List<byte[]> Magic(string a, int b, int c)
        {
            List<byte> tmp = [];
            var str = Enumerable.Range(0, a.Length / 8).Select(i => a.Substring(i * 8, 8));

            foreach (var line in str)
            {
                tmp.Add(Convert.ToByte(line, 2));
            }

            var size = a.Length / 8 / b;
            var extraSize = a.Length / 8 % b;

            List<byte[]> list = [];
            for (int i = b - 1; i >= 0; i--)
            {

            var currentSize = size + (extraSize-- > 0 ? 1 : 0);
                list.Insert(0, new byte[currentSize]);
            }

            var index = c;
            foreach (var block in list)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    block[i] = tmp[index++];
                }
            }

            return list;
        }

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static readonly Dictionary<ErrorCorrectionLevel, byte[]> _countOfErrorCorrectionCodeWords = new()
    {
        {ErrorCorrectionLevel.L, [NA,07,10,15,20,26,18,20,24,30,18,20,24,26,30,22,24,28,30,28,28]},
        {ErrorCorrectionLevel.M, [NA,10,16,26,18,24,16,18,22,22,26,30,22,22,24,24,28,28,26,26,26]},
        {ErrorCorrectionLevel.Q, [NA,13,22,18,26,18,24,18,22,20,24,28,26,24,20,30,24,28,28,26,30]},
        {ErrorCorrectionLevel.H, [NA,17,28,22,16,22,28,26,26,24,28,24,28,22,24,24,30,28,28,26,28]},
    };

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static byte[] Magic(byte[] a, byte b)
        {
            var size = Math.Max(a.Length, b);
            var m = new List<byte>(a);
            var g = _correctionLevelGeneratingPolynomial[b];
            var n = g.Length;
            while (m.Count != size)
                m.Add(0);

            for (int i = 0; i < a.Length; i++)
            {
                byte e = m[0];

                m.RemoveAt(0);
                m.Add(0);

                if (e == 0) continue;

                byte bb = _backGaloisField[e];
                for (int x = 0; x < g.Length; x++)
                {
                    var c = (g[x] + bb) % 255;
                    var d = _galoisField[c];
                    m[x] = (byte)(m[x] ^ d);
                }
            }

            return m.Take(n).ToArray();
        }

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static string Magic(List<byte[]> a, List<byte[]> b)
        {
            var sb = new StringBuilder();
            Magic(a, sb);
            Magic(b, sb);
            return sb.ToString();
        }

        private static readonly Dictionary<byte, byte[]> _correctionLevelGeneratingPolynomial = new()
    { 
        // x^7 + α^87x^6 + α^229x^5 + α^146x^4 +  
        // α^149x^3 + α^238x^2 + α^102x^ + α^21 
        {7, [87, 229, 146, 149, 238, 102, 21]},  
        // x^10 + α^251x^9 + α^67x^8 + α^46x^7  + α^61x^6 +  
        // α^118x^5 + α^70x^4 + α^64x^3 + α^94x^2 + α^32x^ + α^45 
        {10, [251, 67, 46, 61, 118, 70, 64, 94, 32, 45]},

 
        // x^13 + α^74x^12 + α^152x^11 + α^176x^10 + α^100x^9 + α^86x^8 +  
        // α^100x^7 + α^106x^6 + α^104x^5 + α^130x^4 + α^218x^3 + α^206x^2 + α^140x^ + α^78 
        {13, [74, 152, 176, 100, 86, 100, 106, 104, 130, 218, 206, 140, 78]},
        {15, [8, 183, 61, 91, 202, 37, 51, 58, 58, 237, 140, 124, 5, 99, 105]},  
        // x^16 + α^120x^15 + α^104x^14 + α^107x^13 + α^109x^12 + α^102x^11 + α^161x^10 + α^76x^9 + 
        // α^3x^8 + α^91x^7 + α^191x^6 +α^147x^5 + α^169x^4 + α^182x^3 + α^194x^2 + α^225x^ + α^120
        {16, [120, 104, 107, 109, 102, 161, 76, 3, 91, 191, 147, 169, 182, 194, 225, 120]},  
        // x^17 + α^43x^16 + α^139x^15 + α^206x^14 + α^78x^13 + α^43x^12 + α^239x^11  + α^123x^10 + α^206x^9 + α^214x^8 + α^147x^7 + α^24x^6 +
        // α^99x^5 + α^150x^4 + α^39x^3 + α^243x^2 + α^163x^ + α^136 
        {17, [43, 139, 206, 78, 43, 239, 123, 206, 214, 147, 24, 99, 150, 39, 243, 163, 136]}, 
        // x^18 + α^215x^17 + α^234x^16 + α^158x^15 + α^94x^14 + α^184x^13 + α^97x^12 + α^118x^11  + α^170x^10 + α^79x^9 + α^187x^8 + α^152x^7 +
        // α^148x^6 + α^252x^5 + α^179x^4 + α^5x^3 + α^98x^2 + α^96x^ + α^153 
        {18, [215, 234, 158, 94, 184, 97, 118, 170, 79, 187, 152, 148, 252, 179, 5, 98, 96, 153]},
        {20, [17, 60, 79, 50, 61, 163, 26, 187, 202, 180, 221, 225, 83, 239, 156, 164, 212, 212, 188, 190]}, 
        // x^22 + α^210x^21 + α^171x^20 + α^247x^19 + α^242x^18 + α^93x^17 + α^230x^16 + α^14x^15 + α^109x^14 + α^221x^13 + α^53x^12 + 
        // α^200x^11 + α^74x^10 + α^8x^9 + α^172x^8 + α^98x^7 + α^80x^6 + α^219x^5 + α^134x^4 + α^160x^3 + α^105x^2 + α^165x^ + α^231
        {22, [210, 171, 247, 242, 93, 230, 14, 109, 221, 53, 200, 74, 8, 172, 98, 80, 219, 134,160, 105, 165, 231]},
        {24, [229, 121, 135, 48, 211, 117, 251, 126, 159, 180, 169, 152, 192, 226, 228, 218, 111,0, 117, 232, 87, 96, 227, 21]},
        {26, [173, 125, 158, 2, 103, 182, 118, 17, 145, 201, 111, 28, 165, 53, 161, 21, 245, 142,13, 102, 48, 227, 153, 145, 218, 70]}, 
        // x^28 + α^168x^27 + α^223x^26 + α^200x^25 + α^104x^24 + α^224x^23 + α^234x^22 + α^108x^21 + α^180x^20 + α^110x^19 + α^190x^18 + α^195x^17 +  
        // α^147x^16 + α^205x^15 + α^27x^14 + α^232x^13 + α^201x^12 + α^21x^11 + α^43x^10 + α^245x^9 + α^87x^8 + α^42x^7 + α^195x^6 + α^212x^5 + α^119x^4 +
        // α^242x^3 + α^37x^2 + α^9x^ + α^123 
        {28, [168, 223, 200, 104, 224, 234, 108, 180, 110, 190, 195, 147, 205, 27, 232, 201, 21,43, 245, 87, 42, 195, 212, 119, 242, 37, 9, 123]},
        {30, [41, 173, 145, 152, 216, 31, 179, 182, 50, 48, 110, 86, 239, 96, 222, 125, 42, 173,226, 193, 224, 130, 156, 37, 251, 216, 238, 40, 192, 180]},
    };

        private static readonly byte[] _galoisField = [1, 2, 4, 8, 16, 32, 64, 128, 29, 58, 116, 232,
205, 135, 19, 38,
                                                  76, 152, 45, 90, 180, 117, 234, 201, 143, 3, 6,
12, 24, 48, 96, 192,
                                                  157, 39, 78, 156, 37, 74, 148, 53, 106, 212,
181, 119, 238, 193, 159, 35,
                                                  70, 140, 5, 10, 20, 40, 80, 160, 93, 186, 105,
210, 185, 111, 222, 161,
                                                  95, 190, 97, 194, 153, 47, 94, 188, 101, 202,
137, 15, 30, 60, 120, 240,
                                                  253, 231, 211, 187, 107, 214, 177, 127, 254,
225, 223, 163, 91, 182, 113, 226,
                                                  217, 175, 67, 134, 17, 34, 68, 136, 13, 26, 52,
104, 208, 189, 103, 206,
                                                  129, 31, 62, 124, 248, 237, 199, 147, 59, 118,
236, 197, 151, 51, 102, 204,
                                                  133, 23, 46, 92, 184, 109, 218, 169, 79, 158,
33, 66, 132, 21, 42, 84,
                                                  168, 77, 154, 41, 82, 164, 85, 170, 73, 146,
57, 114, 228, 213, 183, 115,
                                                  230, 209, 191, 99, 198, 145, 63, 126, 252, 229,
215, 179, 123, 246, 241, 255,
                                                  227, 219, 171, 75, 150, 49, 98, 196, 149, 55,
110, 220, 165, 87, 174, 65,
                                                  130, 25, 50, 100, 200, 141, 7, 14, 28, 56, 112,
224, 221, 167, 83, 166,
                                                  81, 162, 89, 178, 121, 242, 249, 239, 195, 155,
43, 86, 172, 69, 138, 9,
                                                  18, 36, 72, 144, 61, 122, 244, 245, 247, 243,
251, 235, 203, 139, 11, 22,
                                                  44, 88, 176, 125, 250, 233, 207, 131, 27, 54,
108, 216, 173, 71, 142, 1
                                                     ];

        private static readonly byte[] _backGaloisField = [NA, 0, 1, 25, 2, 50, 26, 198, 3, 223, 51,
238, 27, 104, 199, 75,
                                                      4, 100, 224, 14, 52, 141, 239, 129, 28,
193, 105, 248, 200, 8, 76, 113,
                                                      5, 138, 101, 47, 225, 36, 15, 33, 53, 147,
142, 218, 240, 18, 130, 69,
                                                      29, 181, 194, 125, 106, 39, 249, 185, 201,
154, 9, 120, 77, 228, 114, 166,
                                                      6, 191, 139, 98, 102, 221, 48, 253, 226,
152, 37, 179, 16, 145, 34, 136,
                                                      54, 208, 148, 206, 143, 150, 219, 189, 241,
210, 19, 92, 131, 56, 70, 64,
                                                      30, 66, 182, 163, 195, 72, 126, 110, 107,
58, 40, 84, 250, 133, 186, 61,
                                                      202, 94, 155, 159, 10, 21, 121, 43, 78,
212, 229, 172, 115, 243, 167, 87,
                                                      7, 112, 192, 247, 140, 128, 99, 13, 103,
74, 222, 237, 49, 197, 254, 24,
                                                      227, 165, 153, 119, 38, 184, 180, 124, 17,
68, 146, 217, 35, 32, 137, 46,
                                                      55, 63, 209, 91, 149, 188, 207, 205, 144,
135, 151, 178, 220, 252, 190, 97,
                                                      242, 86, 211, 171, 20, 42, 93, 158, 132,
60, 57, 83, 71, 109, 65, 162,
                                                      31, 45, 67, 216, 183, 123, 164, 118, 196,
23, 73, 236, 127, 12, 111, 246,
                                                      108, 161, 59, 82, 41, 157, 85, 170, 251,
96, 134, 177, 187, 204, 62, 90,
                                                      203, 89, 95, 176, 156, 169, 160, 81, 11,
245, 22, 235, 122, 117, 44, 215,
                                                      79, 174, 213, 233, 230, 231, 173, 232, 116,
214, 244, 234, 168, 80, 88, 175
                                                        ];

        private static void Magic(List<byte[]> a, StringBuilder b)
        {
            if (a.Count == 1)
            {
                foreach (var c in a[0])
                {
                    b.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
                }
                return;
            }

            var size = a.Max(x => x.Length);
            for (int i = 0; i < size; i++)
            {
                foreach (var bytes in a)
                {
                    if (i < bytes.Length)
                        b.Append(Convert.ToString(bytes[i], 2).PadLeft(8, '0'));
                }
            }
        }

        #endregion

        #region Magic 

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static List<byte[]> Magic(this QrData a, Mask b)
        {
            var size = Magic((byte)a.Version, BORDER);
            var tmp = Magic(size)
                .Magic(a.Data, a.Version)
                .Magic1(Magic1(b))
                .Magic(a.Version, b, a.CorrectionLevel)
                ;

            int posX1 = BORDER + 3;
            int posX2 = tmp.Count - BORDER - 4;
            int posY = BORDER + 3;

            tmp.Magic0()
               .Magic0(posX1, posY)
               .Magic0(posX1, tmp[0].Length - BORDER - 4)
               .Magic0(posX2, posY);

            foreach (var x in _alignmentsPosition[a.Version])
    
            foreach (var y in _alignmentsPosition[a.Version].Where(y => CanMagic(x + BORDER, y + BORDER, tmp)))
                tmp.Magic(x + BORDER, y + BORDER);

            tmp.Magic(posX2 - 4, posY + 5, 0).Magic(a.Version);

            return tmp;
        }

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static List<byte[]> Magic(this QrData a, ref Mask? b)
        {
            var res = Enumerable.Range(0, 8).Select(maskNumber => (maskNumber, (0, a.Magic((Mask)maskNumber)).Magic().Magic1())).MinBy(x => x.Item2.a);
            b = (Mask)res.maskNumber;
            return res.Item2.b;
        }

        private static readonly Dictionary<(ErrorCorrectionLevel correctionLevel, Mask maskNum), string>
    _masksAndCorrectionLevel = new()
        {
        {(ErrorCorrectionLevel.L, Mask.M_101), "111011111000100"},
        {(ErrorCorrectionLevel.L, Mask.M_100), "111001011110011"},
        {(ErrorCorrectionLevel.L, Mask.M_111), "111110110101010"},
        {(ErrorCorrectionLevel.L, Mask.M_110), "111100010011101"},
        {(ErrorCorrectionLevel.L, Mask.M_001), "110011000101111"},
        {(ErrorCorrectionLevel.L, Mask.M_000), "110001100011000"},
        {(ErrorCorrectionLevel.L, Mask.M_011), "110110001000001"},
        {(ErrorCorrectionLevel.L, Mask.M_010), "110100101110110"},
        {(ErrorCorrectionLevel.M, Mask.M_101), "101010000010010"},
        {(ErrorCorrectionLevel.M, Mask.M_100), "101000100100101"},
        {(ErrorCorrectionLevel.M, Mask.M_111), "101111001111100"},
        {(ErrorCorrectionLevel.M, Mask.M_110), "101101101001011"},
        {(ErrorCorrectionLevel.M, Mask.M_001), "100010111111001"},
        {(ErrorCorrectionLevel.M, Mask.M_000), "100000011001110"},
        {(ErrorCorrectionLevel.M, Mask.M_011), "100111110010111"},
        {(ErrorCorrectionLevel.M, Mask.M_010), "100101010100000"},
        {(ErrorCorrectionLevel.Q, Mask.M_101), "011010101011111"},
        {(ErrorCorrectionLevel.Q, Mask.M_100), "011000001101000"},
        {(ErrorCorrectionLevel.Q, Mask.M_111), "011111100110001"},
        {(ErrorCorrectionLevel.Q, Mask.M_110), "011101000000110"},
        {(ErrorCorrectionLevel.Q, Mask.M_001), "010010010110100"},
        {(ErrorCorrectionLevel.Q, Mask.M_000), "010000110000011"},
        {(ErrorCorrectionLevel.Q, Mask.M_011), "010111011011010"},
        {(ErrorCorrectionLevel.Q, Mask.M_010), "010101111101101"},
        {(ErrorCorrectionLevel.H, Mask.M_101), "001011010001001"},
        {(ErrorCorrectionLevel.H, Mask.M_100), "001001110111110"},
        {(ErrorCorrectionLevel.H, Mask.M_111), "001110011100111"},
        {(ErrorCorrectionLevel.H, Mask.M_110), "001100111010000"},
        {(ErrorCorrectionLevel.H, Mask.M_001), "000011101100010"},
        {(ErrorCorrectionLevel.H, Mask.M_000), "000001001010101"},
        {(ErrorCorrectionLevel.H, Mask.M_011), "000110100001100"},
        {(ErrorCorrectionLevel.H, Mask.M_010), "000100000111011"},
        };

        private static List<byte[]> Magic(this List<byte[]> a, Version b, Mask c, ErrorCorrectionLevel d)
        {
            var maskNumAndCorrectionLevel = _masksAndCorrectionLevel[(d, c)];
            Magic(a, b, maskNumAndCorrectionLevel);
            return a;
        }

        private static readonly Pair[] _masksAndCorrectionLevelTopLeftTemplate = [
        (02, 10), (03, 10), (04, 10),
        (05, 10), (06, 10), (07, 10),
        (09, 10), (10, 10), (10, 09),
        (10, 07), (10, 06), (10, 05),
        (10, 04), (10, 03), (10, 02), 
        ];

        private static Pair[] Magic(this Version a, int b)
        {
            var offset = 11 + (int)a * 4;
            return [(b, offset + 7), (b, offset +6 ), (b, offset + 5),
                (b, offset + 4), (b, offset + 3), (b, offset + 2),
                (b, offset + 1), (b, offset), (offset + 1, b),
                (offset + 2, b), (offset + 3, b), (offset + 4, b),
                (offset + 5, b), (offset + 6, b), (offset + 7, b)];
        }

        private static void Magic(Pair[] a, List<byte[]> b, int c, char d)
        {
            (var x, var y) = (a[c].X, a[c].Y);
            b[y][x] = d != '1' ? ACTIVE : ZERO;
        }

        private static List<byte[]> Magic(this List<byte[]> a, Version b, string c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                char letter = c[i];
                Magic(_masksAndCorrectionLevelTopLeftTemplate, a, i, letter);
                Magic(b.Magic(10), a, i, letter);
            }
            return a;
        }

        private static List<byte[]> Magic0(this List<byte[]> a, int b, int c)
        {
            Magic(a, b - 4, c - 4, 9, 1);
            Magic(a, b - 3, c - 3, 7, 0);
            Magic(a, b - 2, c - 2, 5, 1);
            Magic(a, b - 1, c - 1, 3, 0);
            return a;
        }

        public static List<byte[]> InvertMatrix(this List<byte[]> a)
        {
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < a.Count; j++)
                {
                    a[i][j] = (byte)(ACTIVE - a[i][j]);
                }
            }
            return a;
        }

        private static List<byte[]> Magic(this List<byte[]> a, byte b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < a.Count; j++)
                {
                    a[i][j] = b;
                }
            }
            return a;
        }

        private static List<byte[]> Magic(this List<byte[]> a, int b, int y)
        {
            Magic(a, b - 2, y - 2, 5, 0);
            Magic(a, b - 1, y - 1, 3, 1);
            a[b][y] = 0;
            return a;
        }

        private static List<byte[]> Magic0(this List<byte[]> a, bool b = false)
        {
            for (int i = BORDER; i < a.Count - BORDER - 6; i++)
            {
                a[i][BORDER + 6] = !b ? (byte)(i % 2) : ZERO;
        }
            for (int i = BORDER; i < a[0].Length - BORDER - 6; i++)
            {
                a[BORDER + 6][i] = !b ? (byte)(i % 2) : ZERO;
            }
            return a;
        }

        private static List<byte[]> Magic(this List<byte[]> a, int b, int c, int d, byte e)
        {
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    a[i + b][j + c] = e;
                }
            }
            return a;
        }

        private static List<byte[]> Magic(this List<byte[]> a, int b, int c, byte d)
        {
            a[b][c] = d;
            return a;
        }

        private static readonly Dictionary<Version, int[]> _alignmentsPosition = new()
    {
        {Version.V1, []},
        {Version.V2, [18]},
        {Version.V3, [22]},
        {Version.V4, [26]},
        {Version.V5, [30]},
        {Version.V6, [34]},
        {Version.V7, [6, 22, 38]},
        {Version.V8, [6, 24, 42]},
        {Version.V9, [6, 26, 46]},
        {Version.V10, [6, 28, 50]},
        {Version.V11, [6, 30, 54]},
        {Version.V12, [6, 32, 58]},
        {Version.V13, [6, 34, 62]},
        {Version.V14, [6, 26, 46, 66]},
        {Version.V15, [6, 26, 48, 70]},
        {Version.V16, [6, 26, 50, 74]},
        {Version.V17, [6, 30, 54, 78]},
        {Version.V18, [6, 30, 56, 82]},
        {Version.V19, [6, 30, 58, 86]},
        {Version.V20, [6, 34, 62, 90]},
    };

        private static readonly Dictionary<Version, string> _versionCodes = new()
    { 
        //                    000010    ▄ 
        //                    111101 █▀▀▄▀ 
        {Version.V7,  "000010011110100110"}, 
        //                    010001  ▄   ▄ 
        //                    011100 ▄██▀  
        {Version.V8,  "010001011100111000"}, 
        //                    110111 ▄▄ ▄▄▄ 
        //                    011000  ▀▀▄ 
        {Version.V9,  "110111011000000100"}, 
        //                    101001 ▄ ▄  ▄ 
        //                    111110 ▀▀▀▀▀ 
        {Version.V10, "101001111110000000"}, 
        //                    001111   ▄▄▄▄ 
        //                    111010 ███▄▀ 
        {Version.V11, "001111111010111100"}, 
        //                    001101   ▄▄ ▄ 
        //                    100100 ▀▄▄▀▄ 
        {Version.V12, "001101100100011010"}, 
        //                    101011 ▄ ▄ ▄▄  
        //                    100000 █  ▄▄ 
        {Version.V13, "101011100000100110"}, 
        //                    110101 ▄▄ ▄ ▄ 
        //                    000110 ▄▀ █▄▀ 
        {Version.V14, "110101000110100010"}, 
        //                    010011  ▄  ▄▄ 
        //                    000010  ▄▄▄█ 
        {Version.V15, "010011000010011110"},
        //                    011100  ▄▄▄ 
        //                    010001 █▄▄ ▀ 
        {Version.V16, "011100010001011100"}, 
        //                    111010 ▄▄▄ ▄ 
        //                    010101 ▄▀ ▀ ▀ 
        {Version.V17, "111010010101100000"}, 
        //                    100100 ▄  ▄ 
        //                    110011 █▀ ▄▀▀ 
        {Version.V18, "100100110011100100"}, 
        //                    000010     ▄ 
        //                    110111 ▀█▄▀▀▀ 
        {Version.V19, "000010110111011000"}, 
        //                    000000      
        //                    101001 █▄█▄▄▀ 
        {Version.V20, "000000101001111110"},
    };

        private static List<byte[]> Magic(this List<byte[]> a, Version b, bool c = false)
        {
            if ((byte)b < 7) return a;

            int pos = 0;
            var version = _versionCodes[b];
            int offsetColumn = BORDER;
            int offsetRow = a.Count - BORDER - POSITION_DETECTION - 3;

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    byte value = c || version[pos++] == '1' ? ZERO : ACTIVE;
                    a[offsetColumn + column][offsetRow + row] = value;
                    a[offsetRow + row][offsetColumn + column] = value;
                }
            }
            return a;
        }

        private static List<byte[]> Magic(Version a)
        {
            int matrixSize = Magic((byte)a, BORDER);
            var size = matrixSize - BORDER * 2;

            var tmp = Magic(matrixSize, ZERO)
                .Magic(BORDER, BORDER, size, ACTIVE);

            int cubeSize = 9;
            int posX1 = BORDER;
            int posY1 = BORDER;
            int posX2 = BORDER + cubeSize + (int)a * 4;
            int posY2 = BORDER + cubeSize + (int)a * 4;

            tmp.Magic(posX1, posY1, cubeSize, ZERO)
               .Magic(posX1, posY2, cubeSize, ZERO)
               .Magic(posX2, posY1, cubeSize, ZERO);

            foreach (var x in _alignmentsPosition[a])
                foreach (var y in _alignmentsPosition[a].Where(y => CanMagic(x + BORDER, y + BORDER, tmp)))
                    tmp.Magic(x + BORDER - 2, y + BORDER - 2, 5, 0);

            tmp.Magic0(true)
               .Magic(a, true);

            return tmp;
        }

        private static bool CanMagic(int x, int y, List<byte[]> matrix)
            => !(x < POSITION_DETECTION + BORDER + 1 && y < POSITION_DETECTION + BORDER + 1 ||
                    x < POSITION_DETECTION + BORDER + 1 && y > matrix.Count - POSITION_DETECTION - BORDER || x > matrix.Count - POSITION_DETECTION - BORDER && y < POSITION_DETECTION + BORDER + 1);

        private static List<byte[]> Magic(this List<byte[]> a, string b, Version c)
        {
            var blockedModules = Magic(c);

            var size = a.Count - BORDER * 2;
            var up = true;
            var index = 0;
            var count = b.Length;

            for (var column = size + BORDER - 1; column >= BORDER; column -= 2)
            {
                if (column == 8) column--;

                for (var i = 0; i < size; i++)
                {
                    var row = up ? size + BORDER - i - 1 : i + BORDER;

                    if (index < count && !blockedModules.IsMagic(row, column))
                        Magic(a, blockedModules, row, column, b[index++]);

                    if (index < count && column > 0 && !blockedModules.IsMagic(row, column - 1))
                        Magic(a, blockedModules, row, column - 1, b[index++]);
                }
                up = !up;
            }


            return a;
        }

        private static bool IsMagic(this List<byte[]> a, int b, int c)
        {
            return a[b][c] == ZERO;
        }

        private static void Magic(List<byte[]> a, List<byte[]> b, int c, int d, char e)
        {
            b[c][d] = ZERO;
            a[c][d] = e != '1' ? ACTIVE : ZERO;
        }

        private static bool Magic0((int x, int y) m) => (m.x + m.y) % 2 == 0;
        private static bool Magic1((int x, int y) m) => m.y % 2 == 0;
        private static bool Magic2((int x, int y) m) => m.x % 3 == 0;
        private static bool Magic3((int x, int y) m) => (m.x + m.y) % 3 == 0;
        private static bool Magic4((int x, int y) m) => (m.x / 3 + m.y / 2) % 2 == 0;
        private static bool Magic5((int x, int y) m) => (m.x * m.y) % 2 + (m.x * m.y) % 3 == 0;
        private static bool Magic6((int x, int y) m) => ((m.x * m.y) % 2 + (m.x * m.y) % 3) % 2 == 0;
        private static bool Magic7((int x, int y) m) => ((m.x * m.y) % 3 + (m.x + m.y) % 2) % 2 == 0;

        private static Predicate<(int, int)> Magic1(Mask a = Mask.M_111)
         => a switch
         {
             Mask.M_101 => Magic0,
             Mask.M_100 => Magic1,
             Mask.M_111 => Magic2,
             Mask.M_110 => Magic3,
             Mask.M_001 => Magic4,
             Mask.M_000 => Magic5,
             Mask.M_011 => Magic6,
             Mask.M_010 => Magic7,
                                   
             _ => throw new ArgumentOutOfRangeException("Incorrect Mask Number")
         };

        private static List<byte[]> Magic1(this List<byte[]> a, Predicate<(int, int)> b)
        {
            for (int x = BORDER; x < a.Count - BORDER; x++)
            {
                for (int y = BORDER; y < a.Count - BORDER; y++)
                {
                    if (b((y - BORDER, x - BORDER)))
                        a[x][y] = (byte)(ACTIVE - a[x][y]);
                }
            }
            return a;
        }

        private static (int a, List<byte[]> b) Magic(this (int a, List<byte[]> b) a)
        {
            var length = 5;
            var s = 0;

            int cnt;
            int current;
        for (int x = BORDER; x < a.b.Count - BORDER; x++)
            {
                cnt = 0;
                current = 3;
                for (int y = BORDER; y < a.b.Count - BORDER; y++)
                {
                    if (a.b[x][y] == current) cnt++;
                    else
                    {
                        if (cnt >= length)
                            s += cnt - 2;
                        current = a.b[x][y];
                        cnt = 0;
                    }
                }
            }

            for (int y = BORDER; y < a.b.Count - BORDER; y++)
            {
                cnt = 0;
                current = 3;
                for (int x = BORDER; x < a.b.Count - BORDER; x++)
                {
                    if (a.b[x][y] == current) cnt++;
                    else
                    {
                        if (cnt >= length)
                            s += cnt - 2;
                        current = a.b[x][y];
                        cnt = 0;
                    }
                }
            }

            return (s + a.a, a.b);
        }

        private static (int a, List<byte[]> b) Magic1(this (int a, List<byte[]> b) a)
        {
            var cntActive = 0.0;
            var cntTotal = 0.0;
            for (int x = BORDER; x < a.b.Count - BORDER; x++)
            {
                for (int y = BORDER; y < a.b.Count - BORDER; y++)
                {
                    if (a.b[x][y] == ACTIVE) cntActive++;
                    cntTotal++;
                }
            }

            double s = cntActive / cntTotal;
            s = s * 100 - 50;
            return (Math.Abs((int)s) * BORDER + a.a, a.b);
        }

        #endregion

        #region Magic 

        /// <summary> 
        /// Magic 
        /// </summary> 
        private static string Magic(this List<byte[]> a, bool b)
        {
            var sb = new StringBuilder();
            var length = a.Count % 2 == 1 ? a.Count + 1 : a.Count;

            for (int row = 0; row < length; row += 2)
            {
                for (int column = 0; column < a[0].Length; column++)
                {
                    byte scanModule1 = a[row][column];
                    byte scanModule2 = row < a.Count - 1 ? a[row + 1][column] : ACTIVE;

                    var c = b
                        ? Magic((scanModule1, scanModule2))
                        : Magic([scanModule1, scanModule2]);

                    sb.Append(c);
            }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary> 
        /// Граница в два модуля вокруг QR-кода 
        /// </summary> 
        private const int BORDER = 2;

        /// <summary> 
        /// Размер рамки для позиционирования QrVersion кода при сканировании 
        /// </summary> 
        private const int POSITION_DETECTION = 8;

        /// <summary> 
        /// Активный модуль 
        /// </summary> 
        private const byte ACTIVE = 1;

        /// <summary> 
        /// Неактивный модуль 
        /// </summary> 
        private const byte ZERO = 0;

        /// <summary> 
        /// Недопустимый параметр 
        /// </summary> 
        private const byte NA = 0;

        private static char Magic(byte[] a)
            => (a[0], a[1]) switch
            {
                (0, 0) => ' ',
                (0, 1) => '▄',
                (1, 0) => '▀',
                (1, 1) => '█',
                _ => throw new NotImplementedException(),
            };

        private static char Magic((byte a, byte b) c)
            => (c.a, c.b) switch
            {
                (1, 1) => ' ',
                (1, 0) => '▄',
                (0, 1) => '▀',
                (0, 0) => '█',
                _ => throw new NotImplementedException(),
            };

        private static List<byte[]> Magic(int a, byte b = ACTIVE)
        {
            List<byte[]> qrCodeMatrix = [];
            for (int i = 0; i < a; i++)
            {
                qrCodeMatrix.Add(new byte[a]);
            }
            return qrCodeMatrix.Magic(b);
        }

        private static int Magic(byte a, int b)
        {
            return 17 + 4 * a + b * 2;
        }

        #endregion
    }
}
