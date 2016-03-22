using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SACS.Common.PInvoke
{
    /// <summary>
    /// contains hooks into the Windows native API
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal class Win32NativeMethods
    {
        internal const short SW_SHOW = 5;
        internal const uint TOKEN_QUERY = 0x0008;
        internal const uint TOKEN_DUPLICATE = 0x0002;
        internal const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        internal const int GENERIC_ALL_ACCESS = 0x10000000;
        internal const int STARTF_USESHOWWINDOW = 0x00000001;
        internal const int STARTF_FORCEONFEEDBACK = 0x00000040;
        internal const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;

        #region Enums

        /// <summary>
        /// Contains values that specify security impersonation levels. Security impersonation levels 
        /// govern the degree to which a server process can act on behalf of a client process.
        /// </summary>
        public enum SECURITY_IMPERSONATION_LEVEL
        {
            /// <summary>
            /// The server process cannot obtain identification information about the client, and it 
            /// cannot impersonate the client. It is defined with no value given, and thus, by ANSI C
            /// rules, defaults to a value of zero.
            /// </summary>
            SecurityAnonymous,

            /// <summary>
            /// The server process can obtain information about the client, such as security identifiers 
            /// and privileges, but it cannot impersonate the client. This is useful for servers that 
            /// export their own objects, for example, database products that export tables and views.
            /// Using the retrieved client-security information, the server can make access-validation
            /// decisions without being able to use other services that are using the client's security 
            /// context.
            /// </summary>
            SecurityIdentification,

            /// <summary>
            /// The server process can impersonate the client's security context on its local system. 
            /// The server cannot impersonate the client on remote systems.
            /// </summary>
            SecurityImpersonation,

            /// <summary>
            /// The server process can impersonate the client's security context on remote systems.
            /// </summary>
            SecurityDelegation
        }

        /// <summary>
        /// Contains values that differentiate between a primary token and an impersonation token
        /// </summary>
        internal enum TOKEN_TYPE
        {
            /// <summary>
            /// Indicates a primary token.
            /// </summary>
            TokenPrimary = 1,

            /// <summary>
            /// Indicates an impersonation token.
            /// </summary>
            TokenImpersonation
        } 

        #endregion

        /// <summary>
        /// Specifies the window station, desktop, standard handles, and appearance of the main window for a 
        /// process at creation time.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        /// <summary>
        /// Contains information about a newly created process and its primary thread. It is used with the 
        /// CreateProcess, CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        /// <summary>
        /// Contains the security descriptor for an object and specifies whether the handle retrieved by 
        /// specifying this structure is inheritable.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        /// <summary>
        /// Creates a new process, using the creditials supplied by hToken. The application opened is 
        /// running under the credentials and authority for the user supplied to LogonUser.
        /// </summary>
        /// <param name="hToken">A handle to the primary token that represents a user.</param>
        /// <param name="lpApplicationName">The name of the module to be executed.</param>
        /// <param name="lpCommandLine">The command line to be executed.</param>
        /// <param name="lpProcessAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies
        /// a security descriptor for the new process object and determines whether child processes can 
        /// inherit the returned handle to the process.</param>
        /// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies 
        /// a security descriptor for the new thread object and determines whether child processes can 
        /// inherit the returned handle to the thread.</param>
        /// <param name="bInheritHandles">If this parameter is <c>true</c>, each inheritable handle in 
        /// the calling process is inherited by the new process</param>
        /// <param name="dwCreationFlags">The flags that control the priority class and the creation of 
        /// the process.</param>
        /// <param name="lpEnvrionment">A pointer to an environment block for the new process.</param>
        /// <param name="lpCurrentDirectory">The full path to the current directory for the process.</param>
        /// <param name="lpStartupInfo">A pointer to a STARTUPINFO or STARTUPINFOEX structure.</param>
        /// <param name="lpProcessInformation">A pointer to a PROCESS_INFORMATION structure that receives
        /// identification information about the new process.</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvrionment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        /// <summary>
        /// Creates a new access token that duplicates an existing token. This function can create either a 
        /// primary token or an impersonation token.
        /// </summary>
        /// <param name="hExistingToken">A handle to an access token opened with TOKEN_DUPLICATE access.</param>
        /// <param name="dwDesiredAccess">Specifies the requested access rights for the new token.</param>
        /// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a
        /// security descriptor for the new token and determines whether child processes can inherit the token. 
        /// If lpTokenAttributes is NULL, the token gets a default security descriptor and the handle cannot be
        /// inherited.</param>
        /// <param name="ImpersonationLevel">Specifies a value from the SECURITY_IMPERSONATION_LEVEL enumeration
        /// that indicates the impersonation level of the new token.</param>
        /// <param name="dwTokenType">Specifies one of the following values from the TOKEN_TYPE enumeration.</param>
        /// <param name="phNewToken">A pointer to a HANDLE variable that receives the new token.</param>
        /// <returns>If the function succeeds, the function returns <c>true</c>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx", SetLastError = true)]
        internal static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            int ImpersonationLevel,
            int dwTokenType,
            ref IntPtr phNewToken);

        /// <summary>
        /// Attempts to log a user on to the local computer.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <param name="domain">The name of the domain or server whose account database contains the username account.</param>
        /// <param name="password">A pointer to a null-terminated string that specifies the plaintext password
        /// for the user account specified by username.</param>
        /// <param name="logonType">The type of logon operation to perform.</param>
        /// <param name="logonProvider">Specifies the logon provider.</param>
        /// <param name="token">A pointer to a handle variable that receives a handle to a token that represents the 
        /// specified user.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool LogonUser(
            string username,
            string domain,
            IntPtr password,
            int logonType,
            int logonProvider,
            ref IntPtr token);

        /// <summary>
        /// Terminates the impersonation of a client application.
        /// </summary>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool RevertToSelf();
    }
}
