namespace M7TROJAN.NumberSystem.Modles
{
    /// <summary>
    /// Represents a decimal number system.
    /// </summary>
    public class DecimalSystem : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalSystem"/> class with the specified decimal value.
        /// </summary>
        /// <param name="value">The decimal value as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the value contains characters not allowed in decimal format.</exception>
        public DecimalSystem(string value)
        {
            value.Guard("0123456789", EnNumberBase.DECIMAL);
            Value = value;
        }
    }
}
