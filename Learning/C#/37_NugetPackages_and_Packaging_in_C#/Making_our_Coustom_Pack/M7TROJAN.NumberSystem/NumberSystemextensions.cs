
using M7TROJAN.NumberSystem.Modles;

namespace M7TROJAN.NumberSystem
{
    /// <summary>
    /// Extension methods for number system conversions.
    /// </summary>
    public static class NumberSystemextensions
    {
        /// <summary>
        /// Validates the source string against allowed characters for the specified number base.
        /// </summary>
        /// <param name="source">The source string to validate.</param>
        /// <param name="allowedCharacters">The allowed characters for the number base.</param>
        /// <param name="numberBase">The number base to validate against.</param>
        /// <exception cref="InvalidOperationException">Thrown when the source contains characters not allowed in the specified number base.</exception>
        public static void Guard(this string source, string allowedCharacters, EnNumberBase numberBase )
        {
            if (source.Any(c => !allowedCharacters.Contains(c)))
            {
                throw new InvalidOperationException($"'{source}' Is invalid {numberBase} format.");
            }
        }


        /// <summary>
        /// Converts the source number system to the specified number base.
        /// </summary>
        /// <typeparam name="T">The type of the source number system.</typeparam>
        /// <param name="source">The source number system.</param>
        /// <param name="numberBase">The target number base to convert to.</param>
        /// <returns>A string representing the converted value in the target number base.</returns>
        public static string To<T>(this T source, EnNumberBase numberBase) where T : Base
        {
            EnNumberBase fromBase;
            switch(source)
            {
                case BinarySystem _:
                    fromBase = EnNumberBase.BINARY;
                    break;
                case DecimalSystem _:
                    fromBase = EnNumberBase.DECIMAL;
                    break;
                case HexadecimalSystem _:
                    fromBase = EnNumberBase.HEXADECIMAL;
                    break;
                case OctalSystem _:
                    fromBase = EnNumberBase.OCTAL;
                    break;
                default:
                    fromBase = EnNumberBase.DECIMAL;
                    break;
            }

            int sourceValue = Convert.ToInt32(source.Value, (int)fromBase);

            return Convert.ToString(sourceValue, (int)numberBase).ToUpper();
        }
    }
}
