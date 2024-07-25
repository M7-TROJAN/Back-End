using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace _06_ExecuteUpdateRawSql
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


            var sql = "Update Wallets Set Holder = @Holder, Balance = @Balance Where Id = @Id";


            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = 3
            };

            SqlParameter holderParam = new SqlParameter
            {
                ParameterName = "@Holder",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = "Mahmoud Adham"
            };

            SqlParameter balanceParam = new SqlParameter
            {
                ParameterName = "@Balance",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = 2000.0
            };

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.Add(idParam);
            command.Parameters.Add(holderParam);
            command.Parameters.Add(balanceParam);

            command.CommandType = CommandType.Text;

            connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Update Done Successfully");
                Console.WriteLine($"Rows Affected: {rowsAffected}");
            }
            else
            {
                Console.WriteLine("Update Failed");
            }

            connection.Close();
        }
    }
}
