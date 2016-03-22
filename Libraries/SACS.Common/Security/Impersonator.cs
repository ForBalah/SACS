using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using SACS.Common.PInvoke;

namespace SACS.Common.Security
{
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
        /// Initializes a new instance of the <see cref="SACS.Common.Security.Impersonator"/> class.
        /// </summary>
        public Impersonator()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this impersonator is busy impersonating a user
        /// </summary>
        public bool IsImpersonating
        {
            get; private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Impersonates the specified user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="SecureString"/></param>
        /// <param name="impersonationCallback">A callback to perform during the impersonation initialization.</param>
        public virtual void Impersonate(string userName, string domainName, SecureString password, Action<IntPtr> impersonationCallback)
        {
            this.Impersonate(userName, domainName, password, LogonType.Logon32LogonNetwork, LogonProvider.Logon32ProviderWINNT50, impersonationCallback);
        }

        /// <summary>
        /// Impersonates the specified user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="password">The password. <see cref="String"/></param>
        /// <param name="logonType">Type of the logon.</param>
        /// <param name="logonProvider">The logon provider.</param>
        /// /// <param name="impersonationAction">A callback to perform during the impersonation initialization.</param>
        public virtual void Impersonate(string userName, string domainName, SecureString password, LogonType logonType, LogonProvider logonProvider, Action<IntPtr> impersonationAction = null)
        {
            UndoImpersonation();

            IntPtr logonToken = IntPtr.Zero;
            IntPtr logonTokenDuplicate = IntPtr.Zero;
            IntPtr passwordPtr = new IntPtr();
            passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
            bool logonValue = false;

            try
            {
                // revert to the application pool identity, saving the identity of the current requestor
                this._wic = WindowsIdentity.Impersonate(IntPtr.Zero);

                // do logon & impersonate
                logonValue = Win32NativeMethods.LogonUser(
                    userName,
                    domainName,
                    passwordPtr,
                    (int)logonType,
                    (int)logonProvider,
                    ref logonToken);

                if (logonValue)
                {
                    Win32NativeMethods.SECURITY_ATTRIBUTES sa = new Win32NativeMethods.SECURITY_ATTRIBUTES();
                    sa.nLength = (uint)Marshal.SizeOf(sa);

                    if (Win32NativeMethods.DuplicateTokenEx(
                        logonToken,
                        Win32NativeMethods.TOKEN_ASSIGN_PRIMARY | Win32NativeMethods.TOKEN_DUPLICATE | Win32NativeMethods.TOKEN_QUERY,
                        ref sa,
                        (int)Win32NativeMethods.SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                        (int)Win32NativeMethods.TOKEN_TYPE.TokenPrimary,
                        ref logonTokenDuplicate))
                    {
                        // Looks like it is working without impersonating. To be revisited.
                        ////var wi = new WindowsIdentity(logonTokenDuplicate);
                        ////wi.Impersonate(); // discard the returned identity context (which is the context of the application)
                        ////this.IsImpersonating = true;
                        if (impersonationAction != null)
                        {
                            impersonationAction(logonTokenDuplicate);
                        }
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
                Marshal.ZeroFreeGlobalAllocUnicode(passwordPtr);

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
        public void UndoImpersonation()
        {
            // restore saved requestor identity
            if (_wic != null)
            {
                _wic.Undo();
            }

            _wic = null;
            IsImpersonating = false;
        }

        /// <summary>
        /// Release all resources used by this process.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UndoImpersonation();
            }
        }

        #endregion
    }
}
