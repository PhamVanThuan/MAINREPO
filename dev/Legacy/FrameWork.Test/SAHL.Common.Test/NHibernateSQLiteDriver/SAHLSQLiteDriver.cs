using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Driver;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteDriver : ReflectionBasedDriver
    {
        public SAHLSQLiteDriver()
            : base(
              "SAHL.Common.Test",
              "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteConnection",
              "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteCommand")
        {
        }

        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        public override string NamedPrefix
        {
            get { return "@"; }
        }

        public override bool SupportsMultipleOpenReaders
        {
            get { return false; }
        }

        public override bool SupportsMultipleQueries
        {
            get { return true; }
        }
    }
}