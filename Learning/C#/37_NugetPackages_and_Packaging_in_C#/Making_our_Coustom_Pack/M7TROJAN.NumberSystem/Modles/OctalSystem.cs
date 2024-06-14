namespace M7TROJAN.NumberSystem.Modles
{
    /// <summary>
    /// Represents an octal number system.
    /// </summary>
    public class OctalSystem : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctalSystem"/> class with the specified octal value.
        /// </summary>
        /// <param name="value">The octal value as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the value contains characters not allowed in octal format.</exception>
        public OctalSystem(string value)
        {
            value.Guard("01234567", EnNumberBase.OCTAL);
            Value = value;
        }
    }
}
