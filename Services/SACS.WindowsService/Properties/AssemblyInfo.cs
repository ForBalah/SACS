using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Owin;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SACS.WindowsService")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("SACS.WindowsService")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("11ad3322-66d4-4957-aa06-af2104c49712")]

[assembly: InternalsVisibleTo("SACS.UnitTests")]

// Needed for unit tests
////[assembly: InternalsVisibleTo("SACS.UnitTests,PublicKey=" +
////    "0024000004800000940000000602000000240000525341310004000001000100c3e27e2912ca3c" +
////    "f45c51cac29ff6a18a26c5fc188f635d6a8a3d4946cb32d2b0195b3408091007501efb14e8fe06" +
////    "b0773cf00d4b990c1b02005cfe461be79efc53718a46213f60a2e86afc86f9a09985bf0b5be97a" +
////    "a59bd1e50c0441502ba7cc34759ed3ed68e30c1ee47cbae5888cc100892709beed88fe17a85000" +
////    "ad628598")]

// OWIN informtion
[assembly: OwinStartup(typeof(SACS.WindowsService.WebAPI.Startup))]

// Log4net configuration
[assembly: log4net.Config.XmlConfigurator(Watch = true)]