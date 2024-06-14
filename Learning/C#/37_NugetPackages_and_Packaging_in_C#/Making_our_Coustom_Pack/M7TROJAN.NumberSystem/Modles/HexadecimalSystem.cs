namespace M7TROJAN.NumberSystem.Modles
{
    /// <summary>
    /// Represents a hexadecimal number system.
    /// </summary>
    public class HexadecimalSystem : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HexadecimalSystem"/> class with the specified hexadecimal value.
        /// </summary>
        /// <param name="value">The hexadecimal value as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the value contains characters not allowed in hexadecimal format.</exception>
        public HexadecimalSystem(string value)
        {
            value = value.ToUpper();
            value.Guard("0123456789ABCDEF", EnNumberBase.HEXADECIMAL);
            Value = value;
        }
    }
}
