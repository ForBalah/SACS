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
                this.LogMessage(null, new MessageEventArgs { Message = new Message(this.GetType().Name, message) });
            }
        }

        /// <summary>
        /// Helper method for sending errors to a pre-defined logger. NOTE: stick to BCL (Base Class Library) exceptions
        /// as there could potentially be issues sending custom exceptions from a service app to its host SAC.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <remarks>
        /// <para>There are a lot of intricacies involved when raising events within a child domain that involves quite
        /// a bit of marshalling and serialization of assemblies between parent and child domains. Because the SAC and the 
        /// Service App could sit in different locations, there can be quite a bit of time spent, by .NET, looking for
        /// the right assemblies. The codename for the assembly loader in .NET is "Fusion" and, if enabled, you can see all
        /// the locations that are checked for the right assembly to load.
        /// </para>
        /// <para>
        /// Because of the way in which the App Domains are setup to contain the service app, it proved to be difficult to
        /// indicate to Fusion where exactly to look for assemblies outside of the directory containing this service app
        /// implementation and the GAC.
        /// </para>
        /// <para>
        /// The easiest solution, without having to specify fairly complex "assembly resolving" for the binder, involves 
        /// sticking to exception classes which are known to be common to both the service app container, and the service
        /// app. And all the derived exception classes within the BCL (E.g. ArgumentException or InvalidOperationException)
        /// meet the criteria.
        /// </para>
        /// <para>
        /// For an explanation around what communication steps take place between app domains, see:
        /// http://stackoverflow.com/questions/1277346/net-problem-with-raising-and-handling-events-using-appdomains.
        /// </para>
        /// </remarks>
        public void SendError(Exception exception)
        {
            if (exception != null && this.LogMessage != null)
            {
                this.LogMessage(this.GetType(), new MessageEventArgs { Message = new Message(this.GetType().Name, exception) });
            }
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy 
        /// for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a 
        /// new lifetime service object initialized to the value of the 
        /// <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This is overridden to prevent the service app from being garbage collected before it executes (typically if 
        /// there is a delay of more than 4 minutes between executions). This happens, even though there is a reference
        /// to the object in the SAC, because this is marked as Marshall By Ref, which means that a proxy is used to
        /// communicate between app domains. The consequence being that the reference to the proxy might still be
        /// legitimate, but the referring object's lifetime is handled completely differently, and the garbage collector
        /// decides to take its liberties.
        /// </para>
        /// <para>
        /// see https://msdn.microsoft.com/en-us/magazine/cc300474.aspx for more information on leases, sponsorship and
        /// object lifetime management.
        /// </para>
        /// </remarks>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null;
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
