using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation
{
    /// <summary>
    /// Base class that service apps must implement to make use of SAC Server.
    /// For ease-of-use, the class implementing this must expose a parameter-less
    /// constructor.
    /// </summary>
    [Serializable]
    public abstract class ServiceAppBase : MarshalByRefObject
    {
        #region Events

        public event EventHandler<MessageEventArgs> LogMessage;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppBase"/> class.
        /// </summary>
        public ServiceAppBase()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the ServiceAppBase is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the ServiceAppBase is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the DomainUnload event of the ServiceAppBase object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void ServiceAppBase_DomainUnload(object sender, EventArgs e)
        {
            this.IsLoaded = false;
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.SendError(e.ExceptionObject as Exception);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Helper method for sending info messages to a pre-defined logger.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessage(string message)
        {
            if (message != null && this.LogMessage != null)
            {
                this.LogMessage(null, new MessageEventArgs { Message = new Message(this.GetType().Name , message) });
            }
        }

        /// <summary>
        /// Helper method for sending errors to a pre-defined logger. NOTE: stick to BCL (Base Class Library) exceptions
        /// as there could be potential issues sending custom exceptions from a service app to its SAC
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <remarks>There are a lot of intricacies involved when raising events within a child domain that involves quite
        /// a bit of marshalling and serialization of assemblies between parent and child domains. Because the SAC and the 
        /// Service App could sit in different locations it there can be quite a bit of time spent, by .NET, looking for
        /// the right assemblies.
        /// 
        /// The easiest solution, without having to specify fairly complex "assembly resolving" for the binder, involves 
        /// merely using exception classes which are known to be common to both the service app container, and the service
        /// app. And all the derived exception classes within the BCL (E.g. ArgumentException or InvalidOperationException)
        /// meet the criteria.
        /// 
        /// For an explanation around what communication steps take place between app domains, see:
        /// http://stackoverflow.com/questions/1277346/net-problem-with-raising-and-handling-events-using-appdomains</remarks>
        public void SendError(Exception exception)
        {
            if (exception != null && this.LogMessage != null)
            {
                this.LogMessage(this.GetType(), new MessageEventArgs { Message = new Message(this.GetType().Name, exception) });
            }
        }

        /// <summary>
        /// Initializes this ServiceApp implementation.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Called when the service is being unloaded. this method should contain details on how to free up unmanaged resources.
        /// </summary>
        public abstract void CleanUp();

        #endregion
    }
}
