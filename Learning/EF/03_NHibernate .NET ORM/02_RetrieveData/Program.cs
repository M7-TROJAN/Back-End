﻿using Microsoft.Extensions.Configuration;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate;

namespace _02_RetrieveData
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
                        var wallets = session.Query<Wallet>();
                        foreach (var wallet in wallets)
                        {
                            Console.WriteLine(wallet);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
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
