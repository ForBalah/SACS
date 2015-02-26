using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Security
{
    /// <summary>
    /// The Logon type
    /// </summary>
    public enum LogonType
    {
        /// <summary>
        /// Interactive logon. Used for users on the same machine.
        /// </summary>
        LOGON32_LOGON_INTERACTIVE = 2,

        /// <summary>
        /// Network logon.
        /// </summary>
        LOGON32_LOGON_NETWORK = 3,

        /// <summary>
        /// Batch logon.
        /// </summary>
        LOGON32_LOGON_BATCH = 4,

        /// <summary>
        /// Service logon.
        /// </summary>
        LOGON32_LOGON_SERVICE = 5,

        /// <summary>
        /// Unlock logon.
        /// </summary>
        LOGON32_LOGON_UNLOCK = 7,

        /// <summary>
        /// Cleartext logon. Win2K or higher.
        /// </summary>
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

        /// <summary>
        /// New credentials logon. Use as substitute for network. Win2K or higher.
        /// </summary>
        LOGON32_LOGON_NEW_CREDENTIALS = 9
    }

    /// <summary>
    /// The logon provider.
    /// </summary>
    public enum LogonProvider
    {
        /// <summary>
        /// Default provider.
        /// </summary>
        LOGON32_PROVIDER_DEFAULT = 0,

        /// <summary>
        /// Win NT 3.5 provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT35 = 1,

        /// <summary>
        /// Win NT 4.0 provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT40 = 2,

        /// <summary>
        /// Win NT 5.0 provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT50 = 3
    }

    /// <summary>
    /// The impersonation level.
    /// </summary>
    public enum ImpersonationLevel
    {
        /// <summary>
        /// Anonymous impersonation level.
        /// </summary>
        SecurityAnonymous = 0,

        /// <summary>
        /// Identification impersonation level.
        /// </summary>
        SecurityIdentification = 1,

        /// <summary>
        /// Full impersonation level.
        /// </summary>
        SecurityImpersonation = 2,

        /// <summary>
        /// Delegated impersonation level.
        /// </summary>
        SecurityDelegation = 3
    }

    /// <summary>
    /// Allows code to be executed under the security context of a specified user account.
    /// </summary>
    /// <remarks> 
    /// Implements IDispose, so can be used via a using-directive or method calls;
    /// ...
    /// var imp = new Impersonator( "myUsername", "myDomainname", "myPassword" );
    /// imp.UndoImpersonation();
    /// ...
    /// var imp = new Impersonator();
    /// imp.Impersonate("myUsername", "myDomainname", "myPassword");
    /// imp.UndoImpersonation();
    /// ...
    /// using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
    /// {
    ///  ...
    ///  1
    ///  ...
    /// }
    /// ...
    /// Taken from: http://platinumdogs.me/2008/10/30/net-c-impersonation-with-network-credentials/
    /// </remarks>
    public class Impersonator : IDisposable
    {
        private WindowsImpersonationContext _wic;

        #region Constructors and Destructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.BusinessLayer.BusinessLogic.Security.Impersonator"/> class.
        /// </summary>
        public Impersonator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.BusinessLayer.BusinessLogic.Security.Impersonator"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="System.String"/></param>
        public Impersonator(string userName, string domainName, string password)
        {
            this.Impersonate(userName, domainName, password, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT);
        } 

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.BusinessLayer.BusinessLogic.Security.Impersonator"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="System.String"/></param>
        /// <param name="logonType">Type of the logon.</param>
        /// <param name="logonProvider">The logon provider. <see cref="Mit.Sharepoint.WebParts.EventLogQuery.Network.LogonProvider"/></param>
        public Impersonator(string userName, string domainName, string password, LogonType logonType, LogonProvider logonProvider)
        {
            this.Impersonate(userName, domainName, password, logonType, logonProvider);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.UndoImpersonation();
        }

        /// <summary>
        /// Impersonates the specified user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="System.String"/></param>
        public void Impersonate(string userName, string domainName, string password)
        {
            this.Impersonate(userName, domainName, password, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT);
        }

        /// <summary>
        /// Impersonates the specified user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="System.String"/></param>
        /// <param name="logonType">Type of the logon.</param>
        /// <param name="logonProvider">The logon provider. <see cref="Mit.Sharepoint.WebParts.EventLogQuery.Network.LogonProvider"/></param>
        public void Impersonate(string userName, string domainName, string password, LogonType logonType, LogonProvider logonProvider)
        {
           this.UndoImpersonation();

            IntPtr logonToken = IntPtr.Zero;
            IntPtr logonTokenDuplicate = IntPtr.Zero;
            try
            {
                // revert to the application pool identity, saving the identity of the current requestor
                this._wic = WindowsIdentity.Impersonate(IntPtr.Zero);

                // do logon & impersonate
                if (Win32NativeMethods.LogonUser(
                    userName,
                    domainName,
                    password,
                    (int)logonType,
                    (int)logonProvider,
                    ref logonToken) != 0)
                {
                    if (Win32NativeMethods.DuplicateToken(logonToken, (int)ImpersonationLevel.SecurityImpersonation, ref logonTokenDuplicate) != 0)
                    {
                        var wi = new WindowsIdentity(logonTokenDuplicate);
                        wi.Impersonate(); // discard the returned identity context (which is the context of the application pool)
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (logonToken != IntPtr.Zero)
                {
                    Win32NativeMethods.CloseHandle(logonToken);
                }

                if (logonTokenDuplicate != IntPtr.Zero)
                {
                    Win32NativeMethods.CloseHandle(logonTokenDuplicate);
                }
            }
        }

        /// <summary>
        /// Stops impersonation.
        /// </summary>
        private void UndoImpersonation()
        {
            // restore saved requestor identity
            if (this._wic != null)
            {
                this._wic.Undo();
            }

            this._wic = null;
        } 

        #endregion
    }

    /// <summary>
    /// Handles to the native methods
    /// </summary>
    internal class Win32NativeMethods
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
    }
}
