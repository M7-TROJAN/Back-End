using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions.DeliveryExample
{
    internal class AccidentException : Exception
    {
        public string Location {  get; set; } 
        public AccidentException(string location, string message) : base(message)
        {
            Location = location;
        }
    }
}
