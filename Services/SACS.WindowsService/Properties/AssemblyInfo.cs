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

// Needed for unit tests
[assembly: InternalsVisibleTo("SACS.UnitTests")]

// OWIN informtion
[assembly: OwinStartup(typeof(SACS.WindowsService.WebAPI.Startup))]

// Log4net configuration
[assembly: log4net.Config.XmlConfigurator(Watch = true)]