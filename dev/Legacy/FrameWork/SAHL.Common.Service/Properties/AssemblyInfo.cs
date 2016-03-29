using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SAHL.Common.Logging.Attributes;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SAHL.Common.Service")]
[assembly: AssemblyDescription("SAHL.Common.Service")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SA Home Loans")]
[assembly: AssemblyProduct("SAHL.Common.Service")]
[assembly: AssemblyCopyright("Copyright ©  2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("af865643-7c46-4041-bbbf-8b30304b8e3d")]

//#if (!DEBUG)
[assembly: SAHLExceptionAspect(AttributeTargetTypes = "SAHL.Common.Service.*")]
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
