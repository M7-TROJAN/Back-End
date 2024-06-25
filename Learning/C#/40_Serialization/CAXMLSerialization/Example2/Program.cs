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

            var xmlContent = SerializeToXMLString(emp);

            Console.WriteLine(xmlContent);

            try
            {
                var deserializedEmp = DeserializeFromXMLString("");
                Console.WriteLine(deserializedEmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadKey();

            
        }

        private static string SerializeToXMLString(Employee emp)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer xmlSerializer = new XmlSerializer(emp.GetType());

            using (StringWriter sw = new StringWriter())
            {
                xmlSerializer.Serialize(sw, emp);
                return sw.ToString();
            }
        }

        private static Employee? DeserializeFromXMLString(string xmlContent)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee));

            try
            {
                using (StringReader sr = new StringReader(xmlContent))
                {
                    return xmlSerializer.Deserialize(sr) as Employee;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Deserialization", ex);
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
