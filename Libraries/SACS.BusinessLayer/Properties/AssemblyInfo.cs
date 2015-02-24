using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SACS.BusinessLayer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("SACS.BusinessLayer")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b6d964ff-f0b9-464a-be01-85de164de44c")]

// Needed for unit tests
[assembly: InternalsVisibleTo("SACS.UnitTests")]
[assembly: InternalsVisibleTo("SACS.IntegrationTests")]

// Since the WCF components especially will need access to the internals
[assembly: InternalsVisibleTo("SACS.WindowsService")]