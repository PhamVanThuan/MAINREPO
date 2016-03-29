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
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
    public partial class EmploymentSubsidised : Employment, IEmploymentSubsidised
	{

        private ISubsidy _subsidy;
        private IReadOnlyEventList<RemunerationTypes> _supportedRemunerationTypes;

        public override IReadOnlyEventList<RemunerationTypes> SupportedRemunerationTypes
        {
            get
            {
                if (_supportedRemunerationTypes == null)
                    _supportedRemunerationTypes = GetSupportedRemunerationTypes(typeof(EmploymentSubsidisedRemunerationTypes));

                return _supportedRemunerationTypes;
            }
        }


        /// <summary>
        /// Get/sets the subsidy associated with the employment record.  There are a couple of items to note:
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///         For now, only one subsidy can exist per employment record.  However, the underlying DAO object
        ///         does support many - as such this property does the work of ensuring that only one active subsidy 
        ///         can exist for the employment record at one time.
        ///         </description>
        ///         <description>This property can only be set if the underlying collection is 
        ///         empty otherwise an exception will be thrown.</description>
        ///         <description>If the employment status changes, the Subsidy status automatically changes to the same (Active/Inactive).</description>
        ///     </item>
        /// </list>
        /// </summary>
        public ISubsidy Subsidy
        {
            get
            {
                if (_subsidy == null && _DAO.Subsidies != null && _DAO.Subsidies.Count > 0)
                {
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    _subsidy = bmtm.GetMappedType<ISubsidy, Subsidy_DAO>(_DAO.Subsidies[0]);
                }
                return _subsidy;
            }
            set 
            {
                if (_DAO.Subsidies == null)
                    _DAO.Subsidies = new List<Subsidy_DAO>();

                // if there is already a subsidy attached and it's not the same as the one 
                // being set, throw an exception as we don't support this
                if (_DAO.Subsidies.Count > 0)
                    throw new Exception("Employment subsidy cannot be set if there is already a subsidy record attached");

                _subsidy = value;
                if (_subsidy != null)
                {
                    // get the DAO object and add to the underlying list - at the moment there can be only
                    // one subsidy on an employment entitity so we clear the list every time
                    Subsidy_DAO dao = ((_subsidy as IDAOObject).GetDAOObject()) as Subsidy_DAO;
                    if (dao == null)
                        throw new NullReferenceException("Unable to get DAO object.");
                    _DAO.Subsidies.Add(dao);
                }

                RefreshSubsidyStatus();

            }
        }

        /// <summary>
        /// Gets/sets the employment status.  If the status is set to previous, the subsidy attached to the employment 
        /// entity will be updated to have a general status of inactive.
        /// </summary>
        public override IEmploymentStatus EmploymentStatus
        {
            get
            {
                return base.EmploymentStatus;
            }
            set
            {
                base.EmploymentStatus = value;
                RefreshSubsidyStatus();
            }
        }

        /// <summary>
        /// Refreshes the subsidy status - if the the employment status changes, the subsidy status 
        /// must also change.
        /// </summary>
        private void RefreshSubsidyStatus()
        {
            if (EmploymentStatus != null && Subsidy != null)
            {
                ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
                if (EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                    this.Subsidy.GeneralStatus = lookUps.GeneralStatuses[GeneralStatuses.Inactive];
                else
                    this.Subsidy.GeneralStatus = lookUps.GeneralStatuses[GeneralStatuses.Active];
            }

        }
    }
}


