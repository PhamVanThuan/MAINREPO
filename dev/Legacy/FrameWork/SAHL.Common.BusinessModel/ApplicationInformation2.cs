using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationInformation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformation_DAO>, IApplicationInformation
    {
        private IApplicationInformationType _applicationInformationTypePrevious;

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ApplicationInformationAcceptedSave");
        }

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();
            _applicationInformationTypePrevious = this.ApplicationInformationType;
        }

        public virtual IApplicationProduct ApplicationProduct
        {
            get
            {
                IApplicationProduct _ApplicationProduct = null;

                switch (this._DAO.Product.Key)
                {
                    // variable loan
                    case (int)SAHL.Common.Globals.Products.VariableLoan: //  1:
                        _ApplicationProduct = new ApplicationProductVariableLoan(this);
                        break;
                    // varifix loan
                    case (int)SAHL.Common.Globals.Products.VariFixLoan: //  2:
                        _ApplicationProduct = new ApplicationProductVariFixLoan(this);
                        break;
                    // Home Owners Cover(HOC)
                    case (int)SAHL.Common.Globals.Products.HomeOwnersCover: //  3:

                        break;
                    // Life Policy
                    case (int)SAHL.Common.Globals.Products.LifePolicy: //  4:

                        break;
                    // Super Lo
                    case (int)SAHL.Common.Globals.Products.SuperLo: //  5:
                        _ApplicationProduct = new ApplicationProductSuperLoLoan(this);
                        break;
                    // Defending Discount Rate
                    case (int)SAHL.Common.Globals.Products.DefendingDiscountRate: //  6:
                        _ApplicationProduct = new ApplicationProductDefendingDiscountLoan(this);
                        break;
                    // New Variable Loan
                    case (int)SAHL.Common.Globals.Products.NewVariableLoan: //  9:
                        _ApplicationProduct = new ApplicationProductNewVariableLoan(this);
                        break;
                    // Quick Cash
                    case (int)SAHL.Common.Globals.Products.QuickCash: //  10:
                        throw new Exception("Quickcash product should not be instantiated.");
                    // Orange Home Loan
                    case (int)SAHL.Common.Globals.Products.Edge: //  9:
                        _ApplicationProduct = new ApplicationProductEdge(this);
                        break;
					// Personal Loan
					case (int)SAHL.Common.Globals.Products.PersonalLoan: // 11:
						_ApplicationProduct = new ApplicationProductPersonalLoan(this);
						break;
                }
                return _ApplicationProduct;
            }
        }

        /// <summary>
        /// Gets the ApplicationInformationType when the object is first loaded.  This will not change during
        /// the lifetime of the object.  If you wish to change the status of the application and
        /// persist, use <see cref="ApplicationInformationType"/>.  For newly created applications, this will
        /// always be null.
        /// </summary>
        public IApplicationInformationType ApplicationInformationTypePrevious
        {
            get
            {
                return _applicationInformationTypePrevious;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationRateOverrides_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationRateOverrides_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationRateOverrides_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationRateOverrides_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationFinancialAdjustments_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationFinancialAdjustments_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationFinancialAdjustments_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnApplicationInformationFinancialAdjustments_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}