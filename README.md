An attempt at creating a modular Windows service manager.

### Features ###

* Contains a single dll that transforms a .NET console application into a Service App.
* Service Apps can be run standalone, as scheduled tasks or in a Service App Container.
* Service App Containers manage the lifetime and scheduling of Service Apps
* Service App Containers are controlled by a Service App Container Server 

### How do I get set up? ###

* Build the entire solution
* Plug the `SACS.Implemenation.dll` into your console application and inherit from `ServiceAppBase`.
* Sign the assemblies if the dll will be us.
* For the server, create a SACS database and run the database creation scripts.
* The server is a Windows service called SACS Agent. Install the service to enable service apps to be loaded into a container.
* The manage service apps, load the Windows management console and add/remove/edit service apps through there
* A web console app is available and can be deployed to allow remote management of services. The web console cannot add/remove services though.

### More information ###

The [SACS Help file](./SolutionItems/SACS%20Help.mht) has all the information on installing and using SACS.