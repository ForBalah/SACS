namespace SACS.Common.Security
{
    /// <summary>
    /// The Logon type
    /// </summary>
    public enum LogonType
    {
        /// <summary>
        /// Interactive logon. Used for users on the same machine.
        /// </summary>
        Logon32LogonInteractive = 2,

        /// <summary>
        /// Network logon.
        /// </summary>
        Logon32LogonNetwork = 3,

        /// <summary>
        /// Batch logon.
        /// </summary>
        Logon32LogonBatch = 4,

        /// <summary>
        /// Service logon.
        /// </summary>
        Logon32LogonService = 5,

        /// <summary>
        /// Unlock logon.
        /// </summary>
        Logon32LogonUnlock = 7,

        /// <summary>
        /// Cleartext logon. Win2K or higher.
        /// </summary>
        Logon32LogonNetworkClearText = 8,

        /// <summary>
        /// New credentials logon. Use as substitute for network. Win2K or higher.
        /// </summary>
        Logon32LogonNewCredentials = 9
    }
}
