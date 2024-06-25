using System.Runtime.Serialization.Formatters.Binary;

namespace CABinarySerialzation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Employee
            {
                Id = 1,
                Fname = "Mahmoud",
                Lname = "Mattar",
                Benefits = new List<string> { "Pension", "Health Insurance", "Vision" }
            };

            // Note that BinaryFormatter is not secure and should not be used to deserialize untrusted data.
            // It is recommended to use a different serialization format, such as JSON or XML, for untrusted data.
            // BinaryFormatter is obsolete in .NET Core 3.0 and later versions and will be removed in a future release.
            // Use System.Text.Json.JsonSerializer instead.


            string binaryString = SerializeToBinaryString(emp);
            Console.WriteLine(binaryString);

            Employee? deserializedEmp = DeserializeFromBinaryString(binaryString);

            if (deserializedEmp != null)
            {
                Console.WriteLine(deserializedEmp);
            }

            Console.ReadKey();

            
        }

        private static string SerializeToBinaryString(Employee emp)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, emp);
                byte[] bytes = stream.ToArray();
                string base64 = Convert.ToBase64String(bytes);
                return base64;
            }
        }

        private static Employee? DeserializeFromBinaryString(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as Employee;
            }
        }
    }

    [Serializable]
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
