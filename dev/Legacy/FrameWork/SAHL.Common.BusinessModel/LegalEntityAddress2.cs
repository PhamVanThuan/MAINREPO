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
using SAHL.Common.Globals;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class LegalEntityAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO>, ILegalEntityAddress
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("LegalEntityAddressPostalConditionalAddressFormats");
            Rules.Add("LegalEntityAddressResidentialConditionalAddressFormats");
            Rules.Add("LegalEntityAddressEffectiveDateMinimum");
            Rules.Add("LegalEntityAddressEffectiveDateMaximum");
            Rules.Add("LegalEntityAddressAccountMailingAddressCheck");
            Rules.Add("LegalEntityAddressApplicationMailingAddressCheck");
            Rules.Add("LegalEntityAddressDoNotDeleteOnRole");
        }

        /// <summary>
        /// The date on which the address record should come into effect.  If this is set to a value 
        /// greater than the current date, it will also set the General Status to inactive.  There is 
        /// a database job that sets these to active once the effective date is reached.
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return _DAO.EffectiveDate; }
            set 
            { 
                _DAO.EffectiveDate = value;
                if (value > DateTime.Now)
                {
                    ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
                    GeneralStatus = lookUps.GeneralStatuses[GeneralStatuses.Inactive];
                }
            }
        }

        /// <summary>
        /// Whether or not the address is active.  If the effective date is greater than today's date, this 
        /// will always be inactive.
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
                IGeneralStatus newValue = value;

                // if the effective date is greater than today's date, general status must always be inactive
                if (EffectiveDate > DateTime.Now)
                {
                    ILookupRepository lookUp = RepositoryFactory.GetRepository<ILookupRepository>();
                    newValue = lookUp.GeneralStatuses[GeneralStatuses.Inactive];
                }

                if (newValue == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = newValue as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

		public bool IsActiveDomicilium
		{
			get
			{
				return LegalEntityDomiciliums.FirstOrDefault(x => x.LegalEntityAddress.Key == this.Key && x.GeneralStatus.Key == (int)GeneralStatuses.Active) != null;
			}
			set
			{
				if (value == true)
				{
					foreach (var legalEntityAddress in this.LegalEntity.LegalEntityAddresses)
					{
						foreach (var domicilium in legalEntityAddress.LegalEntityDomiciliums)
						{
							if (domicilium != null && domicilium.Key != 0 && domicilium.GeneralStatus.Key != (int)GeneralStatuses.Pending)
							{
								domicilium.GeneralStatus = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[GeneralStatuses.Inactive];
							}
						}
					}
 				}
				else
				{
                    this.LegalEntityDomiciliums.First(x => x.LegalEntityAddress.Key == this.Key).GeneralStatus = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[GeneralStatuses.Inactive];
				}
			}
		}
    }
}


