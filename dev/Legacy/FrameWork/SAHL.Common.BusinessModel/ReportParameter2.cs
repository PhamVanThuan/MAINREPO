using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
    public partial class ReportParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportParameter_DAO>, IReportParameter
	{
        //public static IReadOnlyEventList<IReportParameter> GetReportParametersByReportStatementKey(IDomainMessageCollection Messages, int ReportStatementKey)
        //{
        //    ReportStatement_DAO dao = ReportStatement_DAO.Find(ReportStatementKey);

        //    if (dao == null)
        //        return null;

        //    ReportStatement rp = new ReportStatement(dao);
        //    IReadOnlyEventList<IReportParameter> list = new ReadOnlyEventList<IReportParameter>(rp.ReportParameters);
        //    return list;
        //}

       

        public Dictionary<string, object> ValidValues
        {
            get
            {
                if (this.StatementName == null)
                {
                    return null;
                }

                if (_validValues == null)
                {
                    _validValues = new Dictionary<string, object>();
                    
                    IDbConnection con = Helper.GetSQLDBConnection();
                    DataTable DT = new DataTable();
                    Helper.Fill(DT, "COMMON", this.StatementName, con, null);

                    int idx = DT.Columns.Count == 1 ? 0 : 1;

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (!_validValues.ContainsKey(DT.Rows[i][idx].ToString()))
                            _validValues.Add(DT.Rows[i][idx].ToString(), DT.Rows[i][0]);
                    }
                }

                return _validValues;
            }
        }
    }
}


