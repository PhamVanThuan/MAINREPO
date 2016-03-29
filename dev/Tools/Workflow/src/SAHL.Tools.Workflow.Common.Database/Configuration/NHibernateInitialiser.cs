using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SAHL.Tools.Workflow.Common.Database.Conventions;
using SAHL.Tools.Workflow.Common.Database.Mappings;

namespace SAHL.Tools.Workflow.Common.Database
{
    public class NHibernateInitialiser
    {
        private string connectionString;

        public NHibernateInitialiser(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ISessionFactory InitialiseNHibernate()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2005
                            .ConnectionString(c => c.Is(this.connectionString)).AdoNetBatchSize(20)
                            )
                .Mappings(m =>
                    {
                        m.FluentMappings
                            .AddFromAssemblyOf<ActivityMap>()
                            .Conventions.AddFromAssemblyOf<EnumerationTypeConvention>();
                    })
                    
                .BuildSessionFactory();
        }
    }
}