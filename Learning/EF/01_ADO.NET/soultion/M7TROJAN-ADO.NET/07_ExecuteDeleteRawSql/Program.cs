using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _07_ExecuteDeleteRawSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new SqlConnection(configuration.GetSection("constr").Value);


            var sql = "DELETE FROM Wallets WHERE Id = @Id";


            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = 3
            };

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.Add(idParam);

            command.CommandType = CommandType.Text;

            connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected > 0)
            {
                Console.WriteLine("Wallet Deleted successfully");
            }
            else
            {
                Console.WriteLine("Delete failed");
            }
            connection.Close();
        }
    }
}
