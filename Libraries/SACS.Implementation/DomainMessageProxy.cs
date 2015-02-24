using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation
{
    /// <summary>
    /// The proxy class for passing domain messages between app domains
    /// </summary>
    public class DomainMessageProxy : MarshalByRefObject
    {
        private AppDomain _appDomain;
        private Action<Message> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainMessageProxy"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="callback">The callback.</param>
        public DomainMessageProxy(AppDomain domain, Action<Message> callback)
        {
            this._appDomain = domain;
            this._callback = callback;
            AddDomainListener();
        }

        /// <summary>
        /// Adds the domain listener.
        /// </summary>
        public void AddDomainListener()
        {
            this._appDomain.UnhandledException += AppDomain_UnhandledException;
        }

        /// <summary>
        /// Removes the domain listener.
        /// </summary>
        public void RemoveDomainListener()
        {
            this._appDomain.UnhandledException -= AppDomain_UnhandledException;
        }

        /// <summary>
        /// Handles the UnhandledException event of the AppDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (this._callback != null)
            {
                this._callback(new Message(this.GetType().Assembly.FullName, e.ExceptionObject as Exception));
            }
        }
    }
}
