using System.Xml.Serialization;

namespace CAXMLSerialization
{
    internal class Program
    {
        static void Main()
        {

            Employee emp = new Employee
            {
                Id = 1,
                Fname = "Mahmoud",
                Lname = "Mattar",
                Benefits = new List<string> { "Pension", "Health Insurance", "Vision" }
            };

            SerializeToXml(emp);

            DeserializeFromXml();

            Console.ReadKey();

            
        }

        private static void SerializeToXml(Employee emp)
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));

            try
            {
                // Serialize the employee object to XML
                using (TextWriter writer = new StreamWriter("employee.xml"))
                {
                    xmlSerializer.Serialize(writer, emp);
                    Console.WriteLine("Serialization succeeded.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Serialization failed due to an invalid operation: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Serialization failed due to unauthorized access: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Serialization failed due to an I/O error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Serialization failed due to an unexpected error: " + ex.Message);
            }
        }

        private static void DeserializeFromXml()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to deserialize
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));

            Employee? emp2 = null;
            try
            {
                // Deserialize the XML back to an Employee object
                using (TextReader reader = new StreamReader("employee.xml"))
                {
                    emp2 = xmlSerializer.Deserialize(reader) as Employee;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Deserialization failed due to an invalid operation: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Deserialization failed due to unauthorized access: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Deserialization failed due to an I/O error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization failed due to an unexpected error: " + ex.Message);
            }

            if (emp2 is not null)
            {
                Console.WriteLine("Employee object deserialized from XML");
                Console.WriteLine(emp2);
            }
            else
            {
                throw new InvalidOperationException("Deserialization resulted in a null object.");
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public required string Fname { get; set; }
        public required string Lname { get; set; }
        public List<string> Benefits { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Id: {Id}, Fname: {Fname}, Lname: {Lname}, Benefits: {string.Join(", ", Benefits)}";
        }
    }
}
