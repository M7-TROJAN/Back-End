using Microsoft.Extensions.Configuration;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate;

namespace _07_TransferFunds
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var session = CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var idFrom = 4;
                        var idTo = 5;

                        var ammountToTransfer = 2000;

                        var walletFrom = session.Get<Wallet>(idFrom);
                        var walletTo = session.Get<Wallet>(idTo);

                        if (walletFrom is null)
                        {
                            throw new Exception($"Wallet with ID ({idFrom}) not found");
                        }

                        if (walletTo is null)
                        {
                            throw new Exception($"Wallet with ID ({idTo}) not found");
                        }

                        // else transfer funds

                        walletFrom.Balance -= ammountToTransfer;
                        walletTo.Balance += ammountToTransfer;

                        session.Update(walletFrom);

                        session.Update(walletTo);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private static ISession CreateSession()
        {
            // 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetSection("constr").Value;

            var mapper = new ModelMapper();

            // list all of type mappings from the assembly

            mapper.AddMappings(typeof(Wallet).Assembly.ExportedTypes);

            // compile class mapping

            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            // allow the application to specify properties and mapping documents
            // to be used when creating

            var hbconfig = new NHibernate.Cfg.Configuration();


            // settings from app to hibernate
            hbconfig.DataBaseIntegration(c =>
            {
                // strategy to interact with the database
                c.Driver<MicrosoftDataSqlClientDriver>();

                // dialect for the database
                c.Dialect<MsSql2012Dialect>();

                // connection string
                c.ConnectionString = connectionString;

                // log sql statements to console (useful for debugging) optional
                // c.LogSqlInConsole = true;

                // format logged sql statements (optional)
                // c.LogFormattedSql = true;
            });

            // add mapping to NHibernate configuration
            hbconfig.AddMapping(domainMapping);

            // instantiate a new isessionfactory (use properties, settings, and mappings)
            ISessionFactory sessionFactory = hbconfig.BuildSessionFactory();

            // open a new session
            ISession session = sessionFactory.OpenSession();

            return session;

        }
    }
}
