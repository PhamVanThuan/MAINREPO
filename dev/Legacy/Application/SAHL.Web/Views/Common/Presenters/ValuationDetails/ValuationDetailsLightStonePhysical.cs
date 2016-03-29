using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Xml;
using System.Xml.Xsl;
using SAHL.Common.UI;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    public class ValuationDetailsLightStonePhysical : SAHLCommonBasePresenter<IValuationSummary>
    {
        public IValuation Valuation { get; set; }

        /// <summary>
        /// Initializes a new Valuation Details Lightstone Physical
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsLightStonePhysical(IValuationSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// On View Initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
        }

        /// <summary>
        /// On View Pre Render
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            int valuationKey = -1;

            if (GlobalCacheData.ContainsKey("ValuationKey"))
            {
                valuationKey = (int)GlobalCacheData["ValuationKey"];
                GlobalCacheData.Remove("ValuationKey");
            }
            else
            {
                CBONode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
				if (node == null)
				{
					return;
				}
                IX2Repository X2Repository = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = X2Repository.GetLatestInstanceForGenericKey(node.GenericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);
                X2Data x2Data = X2Repository.GetX2DataForInstance(instance);

                if (x2Data == null || x2Data.Data == null || x2Data.Data.Rows.Count < 1)
                {
                    throw new Exception("X2Data not found");
                }

                System.Data.DataRow x2Row = x2Data.Data.Rows[0];


                if (x2Data.Data.Columns.Contains("ValuationKey") && x2Row["ValuationKey"] != DBNull.Value)
                    valuationKey = Convert.ToInt32(x2Row["ValuationKey"]);
            }

            if (valuationKey < 1)
            {
                throw new Exception("No valuation exists for this case");
            }

            IPropertyRepository propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
            var valuation = propertyRepository.GetValuationByKey(valuationKey);

            IXSLTRepository xsltRepository = RepositoryFactory.GetRepository<IXSLTRepository>();
            IXSLTransformation xslt = xsltRepository.GetLatestXSLTransformation(GenericKeyTypes.Valuation);

            _view.RenderXml(valuation.Data, xslt.StyleSheet);
        }
    }
}