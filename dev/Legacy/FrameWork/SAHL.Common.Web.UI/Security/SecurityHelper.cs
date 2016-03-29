using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Configuration.Security;
using System.Web.UI.WebControls;
using SAHL.Common.Security;
using SAHL.Common.CacheData;
using System.Configuration;

namespace SAHL.Common.Web.UI.Security
{
    /// <summary>
    /// Helper class for getting security information from the configuration file and mapping it to the current principal.
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// Checks the security for a given security tag and view/presenter, and determines if the current principal is authenticated 
        /// against the tag.
        /// </summary>
        /// <param name="securityTag"></param>
        /// <param name="view"></param>
        /// <returns>A true if the user is not prohibited by the tag, otherwise false.</returns>
        public static bool CheckSecurity(string securityTag, IViewBase view)
        {
            SecuritySection section = GetConfigSection();
            TagElement tagElement = section.Tags[securityTag];
            if (tagElement == null)
                throw new Exception(String.Format("Security tag {0} not found", securityTag));

            SAHLPrincipal principal = SAHLPrincipal.GetCurrent();

            // first, run through the views and ensure the view/presenter is supported, otherwise exit
            bool foundViewOrPresenter = false;
            foreach (ViewElement viewElement in tagElement.Views)
            {
                if (viewElement.Value == view.ViewName)
                {
                    foundViewOrPresenter = true;
                    break;
                }
            }
            if (!foundViewOrPresenter)
            {
                foreach (PresenterElement presenterElement in tagElement.Presenters)
                {
                    if (presenterElement.Value == view.CurrentPresenter)
                    {
                        foundViewOrPresenter = true;
                        break;
                    }
                }
            }

            // if we haven't found the view or presenter, return true - security has not been specified
            if (!foundViewOrPresenter)
                return true;

            // check all the ad groups
            if (tagElement.ADGroups.Count > 0)
            {
                SecurityElementRestrictions groupRestriction = tagElement.ADGroups.Restriction;

                if (groupRestriction == SecurityElementRestrictions.Include)
                {
                    // if exclusion is "Include", we must loop through ALL the groups listed 
                    // to see if we are in one of them - if we're not in one of them then 
                    // we exit
                    bool found = false;
                    foreach (ADGroupElement adGroup in tagElement.ADGroups)
                    {
                        bool isInRole = principal.IsInRole(adGroup.Value);
                        if (isInRole)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return false;
                }
                else if (groupRestriction == SecurityElementRestrictions.Exclude)
                {
                    // if exlcusion is "Exclude", we can just loop through the groups and if we 
                    // find one we're a member of, we need to exit immediately
                    foreach (ADGroupElement adGroup in tagElement.ADGroups)
                    {
                        bool isInRole = principal.IsInRole(adGroup.Value);
                        if (isInRole)
                            return false;
                    }
                }

            }

            // check all the ad users
            if (tagElement.ADUsers.Count > 0)
            {
                SecurityElementRestrictions usersRestriction = tagElement.ADUsers.Restriction;
                string principalName = principal.Identity.Name.ToLower();
                bool found = false;

                foreach (ADUserElement adUser in tagElement.ADUsers)
                {
                    string adUserName = adUser.Value.ToLower();
                    if (principalName != adUserName)
                        continue;

                    if (usersRestriction == SecurityElementRestrictions.Include)
                    {
                        found = true;
                        break;
                    }
                    else if (usersRestriction == SecurityElementRestrictions.Exclude)
                    {
                        // explicitly excluded - we can just return false
                        return false;
                    }
                }
                if (!found)
                    return false;
            }

            // check all the features
            if (tagElement.Features.Count > 0)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

                // if there are features, we're going to fail by default, and only if a 
                // feature is found that the user has will cancel be set to false again
                bool found = false;
                // build up the list of allowed features
                foreach (FeatureElement feature in tagElement.Features)
                {
                    if (feature.Value.HasValue && spc.FeatureKeys.Contains(feature.Value.Value))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }

            // if we've got here, either there were no security restrictions set up or the user 
            // has passed one of the tests, so we can return true
            return true;
        }

        /// <summary>
        /// Helper method to handle the results of an ISAHLSecurityControl authentication for a web control.  If 
        /// the SecurityHandler is Automatic, it will also check the config file and run security checks.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventArgs"></param>
        internal static void DoSecurityCheck(ISAHLSecurityControl control, SAHLSecurityControlEventArgs eventArgs)
        {
            bool cancel = eventArgs.Cancel;
            WebControl webControl = (WebControl)control;
            IViewBase view = webControl.Page as IViewBase;

            if (view == null)
                return;

            // if args have already cancelled, the control doesn't have a security tag, the page isn't an IViewBase 
            // or the control has been labelled as custom, we don't do the automatic work - the implementor needs to 
            // do that themselves
            if (!eventArgs.Cancel && !String.IsNullOrEmpty(control.SecurityTag) && 
                control.SecurityHandler != SAHLSecurityHandler.Custom)
            {
                cancel = !CheckSecurity(control.SecurityTag, view);
            }

            if (cancel)
            {
                if (control.SecurityDisplayType == SAHLSecurityDisplayType.Hide)
                    webControl.Visible = false;
                else if (control.SecurityDisplayType == SAHLSecurityDisplayType.Disable)
                    webControl.Enabled = false;
            }

        }

        /// <summary>
        /// Gets a reference to the configuration section storing security.
        /// </summary>
        /// <returns></returns>
        private static SecuritySection GetConfigSection()
        {
            SecuritySection section = ConfigurationManager.GetSection("SAHLSecurity") as SecuritySection;
            if (section == null)
                throw new Exception("Configuration section SAHLSecurity does not exist in the application config file.");
            return section;
        }
    }
}
