using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Configuration;
using System.Configuration;
using SAHL.Common.DomainMessages;


namespace SAHL.Web.Views.Common.Presenters
{
    public class RedirectApplication : SAHLCommonBasePresenter<IRedirect>
    {
        private int _applicationKey, _genericKeyTypeKey;

        public RedirectApplication(IRedirect view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // get current navigate value

            // get current application object
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                // get the application key from the global cache
                _applicationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey]);
                _genericKeyTypeKey = Convert.ToInt32(SAHL.Common.Globals.GenericKeyTypes.Offer);
            }
            else
            {
                // get the application key from the cbo
                CBONode CurrentNode = CBOManager.GetCurrentCBONode(base._view.CurrentPrincipal);
                _applicationKey = CurrentNode.GenericKey;
                _genericKeyTypeKey = CurrentNode.GenericKeyTypeKey;
            }

            ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            ICommonRepository _commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();

            IGenericKeyType GenKey;

            if (_genericKeyTypeKey != -1)
            {
                GenKey = _lookupRepository.GenericKeyType.ObjectDictionary[_genericKeyTypeKey.ToString()];
                if (GenKey.TableName != "[2AM].[dbo].[Offer]")
                {
                    throw new Exception("This presenter only handles application objects.");
                }
                else
                {
                    IApplication App = _commonRepository.GetByKey<IApplication>(_applicationKey);

                    if (App == null)
                    {
                        throw new Exception("Application object not found in database.");
                    }

                    SAHLRedirectionSection RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");
                    if (RedirectionSection != null)
                    {
                        RedirectionElement Redirect = RedirectionSection.GetRedirection(App.GetType(), base._view.ViewName);
                        base._view.Navigator.Navigate(Redirect.NavigationView);
                    }
                    
                }
            }
            else
            {

                throw new Exception("GenericKeyType is not specified.");
            }
                      
        }
    }
}
