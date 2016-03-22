namespace SACS.Common.Security
{
    /// <summary>
    /// The logon provider.
    /// </summary>
    public enum LogonProvider
    {
        /// <summary>
        /// Default provider.
        /// </summary>
        Logon32ProviderDefault = 0,

        /// <summary>
        /// Win NT 3.5 provider.
        /// </summary>
        Logon32ProviderWINNT35 = 1,

        /// <summary>
        /// Win NT 4.0 provider.
        /// </summary>
        Logon32ProviderWINNT40 = 2,

        /// <summary>
        /// Win NT 5.0 provider.
        /// </summary>
        Logon32ProviderWINNT50 = 3
    }
}
