using System;

namespace Exceptions.DeliveryExample
{
    internal class InvalidAddressException : Exception
    {
        public string Address { get; private set; }
        public InvalidAddressException(string address, string message) : base(message) 
        {
            Address = address;
        }
    }
}
