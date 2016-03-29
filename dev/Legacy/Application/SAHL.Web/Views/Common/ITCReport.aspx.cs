using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using System.IO;
using System.Xml.Xsl;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common
{
    public partial class ITCReport : System.Web.UI.Page
    {
        private IITCRepository _itcRepo;
        private ILegalEntityRepository _leRepo;
        private IAccountRepository _accRepo;
        private IApplicationRepository _appRepo;
        private IITCArchive itcA;
        private IITC itc;
        private XmlReader rXML;
        private XmlReader rXSL;
        private bool _archiveITC;
        private int _itcKey;
        private DateTime _itcDate;
        private int _lekey;
        private int? _accountkey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            bool idChanged = false;
            string sxml = "";
            if (Request.QueryString["History"].ToString().Length > 0) { _archiveITC = Convert.ToBoolean(Request.QueryString["History"].ToString()); }
            if (Request.QueryString["ITCKey"].ToString().Length > 0) { _itcKey = Convert.ToInt32(Request.QueryString["ITCKey"].ToString()); }

            if (_itcKey > 0)
            {
                // Get the xml
                if (_archiveITC)
                {
                    itcA = itcRepo.GetArchivedITCByITCKey(_itcKey);
                    sxml = itcA.ResponseXML;
                    _itcDate = itcA.ChangeDate;
                    _lekey = itcA.LegalEntityKey;
                    _accountkey = itcA.AccountKey;
                }
                else
                {
                    itc = itcRepo.GetITCByKey(_itcKey);
                    sxml = itc.ResponseXML;
                    _itcDate = itc.ChangeDate;
                    _lekey = itc.LegalEntity.Key;
                    _accountkey = itc.ReservedAccount.Key;
                }


                // Get the xsl
                //IITCXSL xsl = itcRepo.GetITCXslByDate(_itcDate);
                
                // Populate the HALO info, simpler to string replace than build xml and append then xsl it...
                ILegalEntityNaturalPerson le = leRepo.GetLegalEntityByKey(_lekey) as ILegalEntityNaturalPerson;

                //IDNUmber #
                if (String.IsNullOrEmpty(le.IDNumber) || !sxml.Contains(le.IDNumber.Replace(" ", String.Empty)))
                    idChanged = true;

                rXML = XmlReader.Create(new StringReader(sxml));

                string leinfo = GetLegalEntityInfo(le, idChanged);
                string sXsl = itcRepo.GetITCXslByDate(_itcDate).Replace("<!-- HALO INFO -->", leinfo);
                rXSL = XmlReader.Create(new StringReader(sXsl));

                XmlTextWriter writer = new XmlTextWriter(Response.Output);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;

                // Load the style sheet
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(rXSL);

                // Execute the transform and output the results
                xslt.Transform(rXML, writer);
            }
        }

        private string GetLegalEntityInfo(ILegalEntityNaturalPerson le, bool idChanged)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //Addresses
            string addStreet = "";
            string addPost = "";

            foreach (ILegalEntityAddress addle in le.LegalEntityAddresses)
            {
                if (addle.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    if (addle.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Residential)
                        addStreet = addle.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.HtmlLineBreak);

                    if (addle.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Postal)
                    addPost = addle.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.HtmlLineBreak);
                }
            }

            sb.AppendLine("<table width=\"100%\">" +
                    "<tr>" +
                      "<td colspan=\"4\" class=\"Header10\" align=\"center\">HALO Origination Information</td>" +
                    "</tr>");

            if (_archiveITC)
                sb.AppendLine("<tr>" +
                      "<td colspan=\"4\" class=\"Header10\" align=\"center\"  style=\"color: Red\">This is an archived enquiry.</td>" +
                    "</tr>");

            if (!_archiveITC && _itcDate < DateTime.Now.AddDays(-62))
                sb.AppendLine("<tr>" +
                      "<td colspan=\"4\" class=\"Header10\" align=\"center\"  style=\"color: Red\">This enquiry is over 62 days old.</td>" +
                    "</tr>");

            if (idChanged)
                sb.AppendLine("<tr>" +
                      "<td colspan=\"4\" class=\"Header10\" align=\"center\"  style=\"color: Red\">The IDNumber for this person has changed since the enquiry was done, or<br/>" +
                      "The IDNumber in HALO is not the same as in the ITC report.</td>" +
                    "</tr>");
            
            if (le != null)
            {
                sb.AppendLine("<tr>" +
                      "<td class=\"Label9\">Client Name</td>" +
                      "<td class=\"Value8\" />" + le.DisplayName +
                      "<td class=\"Label9\">ID Number</td>" +
                      "<td class=\"Value8\" />" + le.IDNumber +
                    "</tr>");

                foreach (IEmployment emp in le.Employment)
                {
                    if (emp.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Current)
                    {
                        string employer = emp.Employer == null ? "Unknown" : emp.Employer.Name;
                        sb.AppendLine("<tr>" +
                                     "<td class=\"Label9\">Employer</td>" +
                                     "<td class=\"Value8\" />" + employer +
                                     "<td class=\"Label9\">Empl Start Date</td>" +
                                     "<td class=\"Value8\" />" + emp.EmploymentStartDate +
                                   "</tr>");
                    }
                }

                sb.AppendLine("<tr>" +
                      "<td class=\"Label9\">Telephone (H)</td>" +
                      "<td class=\"Value8\" />" + le.HomePhoneCode + " " + le.HomePhoneNumber +
                      "<td class=\"Label9\">Telephone (W)</td>" +
                      "<td class=\"Value8\" />" + le.WorkPhoneCode + " " + le.WorkPhoneNumber +
                    "</tr>" +
                    "<tr>" +
                      "<td class=\"Label9\">Cellphone</td>" +
                      "<td class=\"Value8\" />" + le.CellPhoneNumber +
                      "<td class=\"Label9\">Date of enquiry</td>" +
                      "<td class=\"Value8\" />" + _itcDate.ToShortDateString() +
                    "</tr>" +
                    "<tr>" +
                      "<td class=\"Label9\">Physical Address</td>" +
                      "<td class=\"Value8\">" + addStreet +
                      "<td class=\"Label9\" />Postal Address</td>" +
                      "<td class=\"Value8\" />" + addPost +
                    "</tr>");

                // need to get the assosciated le's for display
                // if application, use application roles, else AccountRoles
                bool rolesFound = false;

                foreach (IApplicationRole ar in le.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
                {
                    if (_accountkey.HasValue && ar.Application.ReservedAccount.Key == _accountkey.Value)
                    {
                        rolesFound = true;
                        //ar.Application.ApplicationRoles
                        sb.AppendLine("<tr>" +
                                  "<td class=\"Label9\" colspan=\"4\">All Applicant/s</td>" +
                                "</tr>");

                        foreach (IApplicationRole r in ar.Application.ApplicationRoles)
                        {
                            if (r.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
                            {
                                string idNumber = "";
                                ILegalEntityNaturalPerson lenp = r.LegalEntity as ILegalEntityNaturalPerson;
                                if (lenp != null)
                                    idNumber = lenp.IDNumber;

                                sb.AppendLine("<tr>" +
                                         "<td class=\"Label9\" colspan=\"4\">" + r.ApplicationRoleType.Description + "</td>" +
                                       "</tr>" +
                                       "<tr>" +
                                         "<td class=\"Label9\">Name</td>" +
                                         "<td class=\"Value8\" />" + r.LegalEntity.DisplayName +
                                         "<td class=\"Label9\">ID Number</td>" +
                                         "<td class=\"Value8\" />" + idNumber +
                                       "</tr>");
                            }
                        }
                        break;
                    }
                }

                if (_accountkey.HasValue)
                {
                    if (!rolesFound)
                    {
                        //Check if the account exists
                        IAccount acc = accRepo.GetAccountByKey(_accountkey.Value);
                        if (acc != null)//Account exists, get each role info for the report
                        {
                            if (acc.Roles.Count > 0)
                            {
                                rolesFound = true;

                                sb.AppendLine("<tr>" +
                                          "<td class=\"Label9\" colspan=\"4\">All Applicant/s</td>" +
                                        "</tr>");

                                foreach (IRole r in acc.Roles)
                                {
                                    string idNum = "";
                                    if (r.LegalEntity.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson)
                                        idNum = ((ILegalEntityNaturalPerson)r.LegalEntity).IDNumber;

                                    sb.AppendLine("<tr>" +
                                             "<td class=\"Label9\" colspan=\"4\">" + r.RoleType.Description + "</td>" +
                                           "</tr>" +
                                           "<tr>" +
                                             "<td class=\"Label9\">Name</td>" +
                                             "<td class=\"Value8\" />" + r.LegalEntity.DisplayName +
                                             "<td class=\"Label9\">ID Number</td>" +
                                             "<td class=\"Value8\" />" + idNum +
                                           "</tr>");
                                }
                            }
                        }
                        else //Account does not exist, get the Application roles
                        {
                            //This will only ever be the case for New Business applications, FL will be handled above because there will be an account
                            //This will therefor only return one application
                            IApplication app = appRepo.GetApplicationByReservedAccountKey(_accountkey.Value);

                            if (app != null)
                            {
                                sb.AppendLine("<tr>" +
                                          "<td class=\"Label9\" colspan=\"4\">All Applicant/s</td>" +
                                        "</tr>");

                                if (app.ApplicationRoles.Count > 0)
                                {
                                    rolesFound = true;

                                    foreach (IApplicationRole ar in app.ApplicationRoles)
                                    {
                                        if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
                                        {
                                            string idNum = "";
                                            if (ar.LegalEntity.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson)
                                                idNum = ((ILegalEntityNaturalPerson)ar.LegalEntity).IDNumber;

                                            sb.AppendLine("<tr>" +
                                                     "<td class=\"Label9\" colspan=\"4\">" + ar.ApplicationRoleType.Description + "</td>" +
                                                   "</tr>" +
                                                   "<tr>" +
                                                     "<td class=\"Label9\">Name</td>" +
                                                     "<td class=\"Value8\" />" + ar.LegalEntity.DisplayName +
                                                     "<td class=\"Label9\">ID Number</td>" +
                                                     "<td class=\"Value8\" />" + idNum +
                                                   "</tr>");
                                        }
                                    }
                                }
                            }
                            //else 
                            //should possibly have something here, but will always be in history/archive view, 
                            //so this report will not be used for making credit descisions
                        } 
                    }
                }


                //if (!rolesFound && le.HasActiveAccounts)
                //{
                //    IAccount acc = accRepo.GetAccountByKey(_accountkey);
                //    if (acc.Roles.Count > 0)
                //    {
                //        sb.AppendLine("<tr>" +
                //                  "<td class=\"Label9\" colspan=\"4\">All Applicant/s</td>" +
                //                "</tr>");

                //        foreach (IRole r in acc.Roles)
                //        {
                //            sb.AppendLine("<tr>" +
                //                     "<td class=\"Label9\" colspan=\"4\">" + r.RoleType.Description + "</td>" +
                //                   "</tr>" +
                //                   "<tr>" +
                //                     "<td class=\"Label9\">Name</td>" +
                //                     "<td class=\"Value8\" />" + r.LegalEntity.DisplayName +
                //                     "<td class=\"Label9\">ID Number</td>" +
                //                     "<td class=\"Value8\" />" + ((ILegalEntityNaturalPerson)r.LegalEntity).IDNumber +
                //                   "</tr>");
                //        }
                //    }
                //}

                sb.AppendLine("</table>");
            }

            return sb.ToString().Replace("&", "&amp;").Replace("'", "\'");
        }

        private IITCRepository itcRepo
        {
            get
            {
                if (_itcRepo == null)
                    _itcRepo = RepositoryFactory.GetRepository<IITCRepository>();

                return _itcRepo;
            }
        }

        private ILegalEntityRepository leRepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }

        private IAccountRepository accRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        private IApplicationRepository appRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

    }
}
