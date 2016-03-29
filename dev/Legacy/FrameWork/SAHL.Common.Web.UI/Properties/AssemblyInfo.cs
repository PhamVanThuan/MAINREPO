using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using SAHL.Common.Logging.Attributes;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SAHL.Common.Web.UI")]
[assembly: AssemblyDescription("SAHL.Common.Web.UI")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SA Home Loans")]
[assembly: AssemblyProduct("SAHL.Common.Web.UI")]
[assembly: AssemblyCopyright("Copyright ©  2006")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLDropDownList.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLTabStrip.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLTabStrip.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.BalloonHints.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLValidators.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLAutoComplete.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLAutoComplete.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLAutoFillCombobox.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLAutoFillCombobox.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLSuburbCombobox.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLSuburbCombobox.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLCityCombobox.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLCityCombobox.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLPhone.js", "text/javascript")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLInputDate.js", "text/javascript")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLInputDate.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLTextBox.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.ValidatorExclaim.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.ValidatorError.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.ValidatorInvalid.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.aNorthEast.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.aNorthWest.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.aSouthEast.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.aSouthWest.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightTop.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftTop.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightBottom.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftBottom.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.close.jpg", "image/jpg")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.closeDown.jpg", "image/jpg")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.closeActive.jpg", "image/jpg")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.exclaim.ico", "image/ico")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.help.ico", "image/ico")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.helpbubble.ico", "image/ico")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.info.ico", "image/ico")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tableft.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tabright.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.Calendar.jpg", "image/jpg")]

// files and images for the SAHLButton
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLButton.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLImageButton.js", "text/javascript")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLButton.css", "text/css", PerformSubstitution = true)]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button1.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button1dis.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button2.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button2dis.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button3.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button3dis.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button4.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button4dis.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button5.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button5dis.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button6.gif", "image/gif")]
//[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.button6dis.gif", "image/gif")]

// SAHLCurrencyBox
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLCurrencyBox.js", "text/javascript")]

// SAHLDateBox
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLDateBox.js", "text/javascript")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLDateBox.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.calendar.png", "image/png")]

// SAHLGridView
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLGridView.js", "text/javascript")]

// files and images for the SAHLTreeView
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLTreeView.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLTreeView.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_blank.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_line_vert.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_bottom.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_middle.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_single.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_top.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_bottom.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_middle.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_single.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_top.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_bottom.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_middle.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_single.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_top.gif", "image/gif")]

// SAHLValidationSummary
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLValidationSummary.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.SAHLValidationSummary.css", "text/css")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.minimise_arrow.gif", "image/gif")]
[assembly: WebResource("SAHL.Common.Web.UI.Controls.Resources.maximise_arrow.gif", "image/gif")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("064b4e15-4267-4f25-966d-245561d68b83")]

//#if (!DEBUG)
[assembly: SAHLExceptionAspect(AttributeTargetTypes = "SAHL.Common.Web.UI.*")]
//#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:
//[assembly: AssemblyKeyFileAttribute("..\\..\\..\\KeyFile\\SAHL.Common.snk")]
[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.0.0")]
