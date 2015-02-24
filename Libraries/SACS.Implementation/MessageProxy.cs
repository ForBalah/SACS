using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation
{
    /// <summary>
    /// The proxy class for passing messages between app domains
    /// </summary>
    public class MessageProxy : MarshalByRefObject
    {
        private ServiceAppBase _serviceAppBase;
        private Action<Message> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppMessages" /> class.
        /// </summary>
        /// <param name="serviceAppBase">The service application base.</param>
        /// <param name="callback">The callback.</param>
        public MessageProxy(ServiceAppBase serviceAppBase, Action<Message> callback)
        {
            this._serviceAppBase = serviceAppBase;
            this._callback = callback;
            AddMessageListener();
        }

        /// <summary>
        /// Adds the message listener.
        /// </summary>
        public void AddMessageListener()
        {
            this._serviceAppBase.LogMessage += ServiceAppBase_LogMessage;
        }

        /// <summary>
        /// Removes the message listener.
        /// </summary>
        public void RemoveMessageListener()
        {
            this._serviceAppBase.LogMessage -= ServiceAppBase_LogMessage;
        }

        /// <summary>
        /// Services the application base_ log message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The message to log. Typically a string or an exception.</param>
        private void ServiceAppBase_LogMessage(object sender, MessageEventArgs e)
        {
            if (this._callback != null)
            {
                this._callback(e.Message);
            }
        } 
    }
}
