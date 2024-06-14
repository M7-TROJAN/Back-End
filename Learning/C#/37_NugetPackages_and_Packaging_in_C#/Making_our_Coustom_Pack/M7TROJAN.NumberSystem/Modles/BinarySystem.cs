namespace M7TROJAN.NumberSystem.Modles
{
    /// <summary>
    /// Represents a binary number system.
    /// </summary>
    public class BinarySystem : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySystem"/> class with the specified binary value.
        /// </summary>
        /// <param name="value">The binary value as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the value contains characters not allowed in binary format.</exception>
        public BinarySystem(string value)
        {
            value.Guard("01", EnNumberBase.BINARY);
            Value = value;
        }

        /// <summary>
        /// Implicitly converts a string to a <see cref="BinarySystem"/> instance.
        /// </summary>
        /// <param name="value">The binary value as a string.</param>
        public static implicit operator BinarySystem(string value)
        {
            return new BinarySystem(value);
        }
    }
}
