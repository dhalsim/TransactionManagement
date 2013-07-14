using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using TransactionManagement.Entities;

namespace TransactionManagement.Helpers
{
    public class NhibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory ?? (_sessionFactory = InitializeSessionFactory());
            }
        }

        private static ISessionFactory InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(@"Server=.;Database=Test;Integrated Security=True;")
                              .ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Zone>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                .BuildSessionFactory();
            
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}