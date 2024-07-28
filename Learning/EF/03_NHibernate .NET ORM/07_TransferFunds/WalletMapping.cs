using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace _07_TransferFunds
{
    public class WalletMapping : ClassMapping<Wallet>
    {
        public WalletMapping()
        {
            Id(x => x.Id, configuration => {
                configuration.Generator(Generators.Identity);
                configuration.Type(NHibernateUtil.Int32);
                configuration.Column("Id");
                configuration.UnsavedValue(0);
            });
            
            Property(x => x.Holder, c =>
            {
                c.Length(100);
                c.Type(NHibernateUtil.AnsiString);
                c.NotNullable(true);
            });

            Property(x => x.Balance, c =>
            {
                c.Type(NHibernateUtil.Decimal);
                c.NotNullable(true);
            });

            Table("Wallets");
        }

    }
}
