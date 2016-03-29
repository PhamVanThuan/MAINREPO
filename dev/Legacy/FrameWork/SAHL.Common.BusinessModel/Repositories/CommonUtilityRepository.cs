using System.Data;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICommonUtilityRepository))]
    [Obsolete("This repository should not be used - it will be removed at some stage (see CommonRepository instead) - the methods contained here are not valid for SAHL Repositories", false)]
    public class CommonUtilityRepository : AbstractRepositoryBase, ICommonUtilityRepository
    {

        /// <summary>
        /// This method returns the specified XML file as a dataset
        /// </summary>
        /// <param name="xml"></param>
		internal static DataSet GetDataSetFromXML(string xml)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(xml);

            return ds;
        }

		/// <summary>
		/// This method is used to check if a column exists in a Data Table
		/// </summary>
		/// <param name="Table"></param>
		/// <param name="ColumnName"></param>
        internal static bool ColumnExists(DataTable Table, string ColumnName)
        {
            bool returnVal = false;
            foreach (DataColumn col in Table.Columns)
            {
                if (col.ColumnName == ColumnName)
                {
                    returnVal = true;
                    break;
                }
            }

            return returnVal;
        }

        public TInterface GetObjectByKey<TInterface, TDAO>(int Key) where TDAO : class
        {
            return base.GetByKey<TInterface, TDAO>(Key);
        }
    }
}
