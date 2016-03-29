using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Security;
using SAHL.Common.CacheData;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
    public partial class DetailType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DetailType_DAO>, IDetailType, IComparable
	{
        /// <summary>
        /// 
        /// </summary>
        public IDetailClass DetailClass
        {
            get
            {
                if (null == _DAO.DetailClass) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDetailClass, DetailClass_DAO>(_DAO.DetailClass);
                }
            }

            set
            {
                // only allow updates on new objects, not existing objects
                if (this.Key <= 0)
                {
                    if (value == null)
                    {
                        _DAO.DetailClass = null;
                        return;
                    }
                    IDAOObject obj = value as IDAOObject;

                    if (obj != null)
                        _DAO.DetailClass = (DetailClass_DAO)obj.GetDAOObject();
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
                else
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    IDomainMessageCollection dmc = spc.DomainMessages;
                    dmc.Add(new Error("Detail class cannot be updated on existing detail type records.", ""));
                }
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            DetailType g = (DetailType)obj;
            return string.Compare(this.Description, g.Description);
        }

        #endregion
    }
}


