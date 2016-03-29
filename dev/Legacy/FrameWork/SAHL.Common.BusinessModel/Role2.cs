using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Role : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Role_DAO>, IRole
    {
        /// <summary>
        ///
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IRoleType RoleType
        {
            get
            {
                if (null == _DAO.RoleType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IRoleType, RoleType_DAO>(_DAO.RoleType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.RoleType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.RoleType = (RoleType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool UnderDebtCounselling(bool activeOnly)
        {
            bool rslt = false;

            string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "UnderDebtCounsellingByRoleKey");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountRoleKey", this.Key));
            parameters.Add(new SqlParameter("@activeOnly", activeOnly));

            // execute
            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            rslt = o == null ? false : Convert.ToBoolean(o);

            return rslt;
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("SuretorRemoveCheckConfirmedIncome");
        }
    }
}