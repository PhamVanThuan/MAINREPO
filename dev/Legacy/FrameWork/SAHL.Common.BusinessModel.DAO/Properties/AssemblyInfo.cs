using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SAHL.Common.Logging.Attributes;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SAHL.Common.BusinessModel")]
[assembly: AssemblyDescription("SAHL.Common.BusinessModel")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SA Home Loans")]
[assembly: AssemblyProduct("SAHL.Common.BusinessModel")]
[assembly: AssemblyCopyright("Copyright ©  2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("58f258e6-f986-4085-975f-ad25ca47f354")]

//#if (!DEBUG)
[assembly: SAHLExceptionAspect(AttributeTargetTypes = "SAHL.Common.BusinessModel.DAO.*")]
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
[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.0.0")]
