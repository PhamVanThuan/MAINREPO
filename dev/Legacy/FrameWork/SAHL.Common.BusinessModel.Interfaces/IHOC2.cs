using System;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// IHOC extended Interface (IHOC2)
    /// </summary>
    public partial interface IHOC
    {
        #region properties

        /// <summary>
        ///
        /// </summary>
        double PremiumThatch { get; set; }

        /// <summary>
        ///
        /// </summary>
        double PremiumShingle { get; set; }

        /// <summary>
        ///
        /// </summary>
        double PremiumConventional { get; set; }

        /// <summary>
        /// HOCTotalSumInsured Property
        /// </summary>
        double HOCTotalSumInsured { get; }

        /// <summary>
        /// AnniversaryDate Property
        /// </summary>
        DateTime? AnniversaryDate
        {
            get;
            set;
        }

        #endregion properties

        #region Methods

        /// <summary>
        /// Set HOCTotalSumInsured
        /// </summary>
        /// <param name="HOCTotalSumInsured"></param>
        void SetHOCTotalSumInsured(double HOCTotalSumInsured);

        /// <summary>
        ///
        /// </summary>
        void CalculatePremium();

        void CalculatePremiumForUpdate();

        #endregion Methods

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded.  For new hoc, this will
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        IHOC Original { get; }
    }
}