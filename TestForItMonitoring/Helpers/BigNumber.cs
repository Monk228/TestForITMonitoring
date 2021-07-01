using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForItMonitoring.Helpers
{
    public class BigNumber
    {
      
        public readonly List<byte> digits = new List<byte>();
        public int Size => digits.Count;

        //знак числа
        public Sign Sign { get; private set; } = Sign.Plus;

        //получение цифры по индексу
        //если индекс больше длины возвращает 0
        public byte GetByte(int i) => i < Size ? digits[i] : (byte)0;

        //установка цифры по индексу
        public void SetByte(int i, byte b)
        {
            while (digits.Count <= i)
            {
                digits.Add(0);
            }

            digits[i] = b;
        }

        /// <summary>
        /// метод для получения больших чисел формата valEexp(пример 1E3 = 1000, 5E2 = 500)
        /// </summary>
        /// <param name="val">значение числа</param>
        /// <param name="exp">экспонента(количество нулей после значения val)</param>
        /// <returns></returns>
        public static BigNumber Exp(byte val, int exp)
        {
            var bigInt = Zero;
            bigInt.SetByte(exp, val);
            bigInt.RemoveNulls();
            return bigInt;
        }

        //преобразование длинного числа в строку
        public override string ToString()
        {
            if (this == Zero) return "0";
            var s = new StringBuilder(Sign == Sign.Plus ? "" : "-");

            for (var i = digits.Count - 1; i >= 0; i--)
            {
                s.Append(Convert.ToString(digits[i]));
            }

            return s.ToString();
        }
        public BigNumber(List<byte> bytes)
        {
            digits = bytes.ToList();
            RemoveNulls();
        }

        public BigNumber(Sign sign, List<byte> bytes)
        {
            Sign = sign;
            digits = bytes;
            RemoveNulls();
        }

        public BigNumber(string s)
        {
            if (s.StartsWith("-"))
            {
                Sign = Sign.Minus;
                s = s.Substring(1);
            }

            foreach (var c in s.Reverse())
            {
                digits.Add(Convert.ToByte(c.ToString()));
            }

            RemoveNulls();
        }

        public BigNumber(uint x) => digits.AddRange(GetBytes(x));

        public BigNumber(int x)
        {
            if (x < 0)
            {
                Sign = Sign.Minus;
            }

            digits.AddRange(GetBytes((uint)Math.Abs(x)));
        }

        /// <summary>
        /// метод для получения списка цифр из целого беззнакового числа
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private List<byte> GetBytes(uint num)
        {
            var bytes = new List<byte>();
            do
            {
                bytes.Add((byte)(num % 10));
                num /= 10;
            } while (num > 0);

            return bytes;
        }

        /// <summary>
        /// метод для удаления лидирующих нулей длинного числа
        /// </summary>
        private void RemoveNulls()
        {
            for (var i = digits.Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)
                {
                    digits.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        public static BigNumber Add(BigNumber a, BigNumber b)
        {
            var digits = new List<byte>();

            var maxLength = Math.Max(a.Size, b.Size);
            byte t = 0;
            for (int i = 0; i < maxLength; i++)
            {
                byte sum = (byte)(a.GetByte(i) + b.GetByte(i) + t);
                if (sum > 10)
                {
                    sum -= 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }

                digits.Add(sum);
            }

            if (t > 0)
            {
                digits.Add(t);
            }

            return new BigNumber(a.Sign, digits);
        }
        public static BigNumber Substract(BigNumber a, BigNumber b)
        {
            var digits = new List<byte>();

            BigNumber max = Zero;
            BigNumber min = Zero;

            //сравниваем числа игнорируя знак
            var compare = Comparison(a, b, ignoreSign: true);

            switch (compare)
            {
                case -1:
                    min = a;
                    max = b;
                    break;
                case 0:
                    //если числа равны возвращаем 0
                    return Zero;
                case 1:
                    min = b;
                    max = a;
                    break;
            }

            //из большего вычитаем меньшее
            var maxLength = Math.Max(a.Size, b.Size);

            var t = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var s = max.GetByte(i) - min.GetByte(i) - t;
                if (s < 0)
                {
                    s += 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }

                digits.Add((byte)s);
            }

            return new BigNumber(max.Sign, digits);
        }
        private static int Comparison(BigNumber a, BigNumber b, bool ignoreSign = false)
        {
            return CompareSign(a, b, ignoreSign);
        }

        private static int CompareSign(BigNumber a, BigNumber b, bool ignoreSign = false)
        {
            if (!ignoreSign)
            {
                if (a.Sign < b.Sign)
                {
                    return -1;
                }
                else if (a.Sign > b.Sign)
                {
                    return 1;
                }
            }

            return CompareSize(a, b);
        }

        private static int CompareSize(BigNumber a, BigNumber b)
        {
            if (a.Size < b.Size)
            {
                return -1;
            }
            else if (a.Size > b.Size)
            {
                return 1;
            }

            return CompareDigits(a, b);
        }

        private static int CompareDigits(BigNumber a, BigNumber b)
        {
            var maxLength = Math.Max(a.Size, b.Size);
            for (var i = maxLength; i >= 0; i--)
            {
                if (a.GetByte(i) < b.GetByte(i))
                {
                    return -1;
                }
                else if (a.GetByte(i) > b.GetByte(i))
                {
                    return 1;
                }
            }

            return 0;
        }
        public static BigNumber Zero => new BigNumber(0);

        public static BigNumber One => new BigNumber(1);
    }
    public enum Sign
    {
        Minus = -1,
        Plus = 1
    }
}
