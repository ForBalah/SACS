using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// Handles all the installation aspects
    /// </summary>
    public class InstallationManager
    {
        #region Fields

        private static InstallationManager _Current;
        private ManagementObject _SacsManagementObject;
        private string _ServerInstallLocation;
        private string _WindowsConsoleInstallLocation;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="InstallationManager"/> class from being created.
        /// </summary>
        private InstallationManager()
        {
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets the current installationManager.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static InstallationManager Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new InstallationManager();
                }

                return _Current;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the installer should create a windows shortcut.
        /// </summary>
        /// <value>
        /// <c>true</c> if the installer should create a windows shortcut; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>For now this is read-only and always true.</remarks>
        public bool CreateWindowsShortcut
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the current server location.
        /// </summary>
        /// <returns></returns>
        public string CurrentServerLocation
        {
            get
            {
                Regex pathRegex = new Regex(@"^[^""]*");
                var mo = this.SacsManagementObject;
                if (mo != null)
                {
                    string fullPath = mo.GetPropertyValue("PathName").ToString().Trim('"');
                    return Path.GetDirectoryName(pathRegex.Match(fullPath).Value);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the current windows console location.
        /// </summary>
        /// <value>
        /// The current windows console location.
        /// </value>
        public string CurrentWindowsConsoleLocation
        {
            get
            {
                string commonStartMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");
                FileInfo exeFile = FileSystemUtilities.FindFile("SACS.Windows.exe", commonStartMenuPath);

                if (exeFile == null)
                {
                    return null;
                }

                return exeFile.DirectoryName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this can be a server upgrade.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this can be a server upgrade; otherwise, <c>false</c>.
        /// </value>
        public bool IsServerUpgrade
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.CurrentServerLocation);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this can be a windows console upgrade.
        /// </summary>
        /// <value>
        /// <c>true</c> if this can be a windows console upgrade; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindowsConsoleUpgrade
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.CurrentWindowsConsoleLocation);
            }
        }

        /// <summary>
        /// Gets or sets the server install location.
        /// </summary>
        /// <value>
        /// The new server location.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Cannot overwrite existing installed server location</exception>
        public string ServerInstallLocation
        {
            get
            {
                return this.CurrentServerLocation ?? this._ServerInstallLocation;
            }

            set
            {
                if (this.CurrentServerLocation == null)
                {
                    this._ServerInstallLocation = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot overwrite existing installed server location");
                }
            }
        }

        /// <summary>
        /// Gets the service account.
        /// </summary>
        /// <value>
        /// The service account.
        /// </value>
        public string ServiceAccount
        {
            get
            {
                if (this.SacsManagementObject != null)
                {
                    return this.SacsManagementObject.GetPropertyValue("StartName").ToString();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the windows console install location.
        /// </summary>
        /// <value>
        /// The windows console install location.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Cannot overwrite existing installed Windows console location</exception>
        public string WindowsConsoleInstallLocation
        {
            get
            {
                return this.CurrentWindowsConsoleLocation ?? this._WindowsConsoleInstallLocation;
            }

            set
            {
                if (this.CurrentWindowsConsoleLocation == null)
                {
                    this._WindowsConsoleInstallLocation = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot overwrite existing installed Windows console location");
                }
            }
        }

        /// <summary>
        /// Gets the sacs service controller.
        /// </summary>
        /// <value>
        /// The sacs service controller.
        /// </value>
        protected ServiceController SacsServiceController
        {
            get
            {
                try
                {
                    ServiceController sc = new ServiceController("SACS.Agent");
                    var status = sc.Status; // this should prove that the service exists and is accessible.
                    return sc;
                }
                catch (InvalidOperationException)
                {
                    // service does not exist
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the sacs management object.
        /// </summary>
        /// <value>
        /// The sacs management object.
        /// </value>
        protected ManagementObject SacsManagementObject
        {
            get
            {
                if (this._SacsManagementObject == null)
                {
                    ServiceController sc = this.SacsServiceController;
                    if (sc != null)
                    {
                        ManagementClass mc = new ManagementClass("Win32_Service");
                        foreach (ManagementObject mo in mc.GetInstances())
                        {
                            if (mo.GetPropertyValue("Name").ToString() == sc.ServiceName)
                            {
                                this._SacsManagementObject = mo;
                            }
                        }
                    }
                }

                return this._SacsManagementObject;
            }
        }

        #endregion Properties

        #region Methods

        #region Server

        /// <summary>
        /// Gets the server version from the SACS file, if it can find it.
        /// </summary>
        /// <returns></returns>
        public string GetServerVersion()
        {
            if (this.ServerInstallLocation != null)
            {
                try
                {
                    var fileVersion = FileVersionInfo.GetVersionInfo(Path.Combine(this.ServerInstallLocation, "SACS.WindowsService.exe"));
                    return fileVersion.ProductVersion;
                }
                catch (FileNotFoundException)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Installs the server to the specified path.
        /// </summary>
        /// <param name="completionCallback">The completion callback.</param>
        public void InstallServer(Action<bool> completionCallback)
        {
            if (!this.CanInstallServer(this.ServerInstallLocation))
            {
                return;
            }

            var installTask = Task.Run(() =>
                {
                    WizardManager wizard = WizardManager.Current;
                    string tempPath = Path.GetTempPath() + "SACS.WindowsService";

                    try
                    {
                        wizard.ShowProgressDialog();

                        wizard.UpdateProgressText("Extracting files...");
                        FileSystemUtilities.ExtractFromResource("SACS.Setup.Resources.SACS.WindowsService.zip", tempPath, "SACS.WindowsService");
                        wizard.UpdateProgressValue(0.2m);
                        Thread.Sleep(300);

                        wizard.UpdateProgressText("Stopping agent...");
                        if (!this.TryStopService())
                        {
                            throw new InstallException("Could not stop agent");
                        }

                        wizard.UpdateProgressValue(0.4m);
                        Thread.Sleep(300);

                        wizard.UpdateProgressText("Backing up files...");
                        FileSystemUtilities.BackupDirectory(this.ServerInstallLocation, "SACS.WindowsService");
                        wizard.UpdateProgressValue(0.6m);
                        Thread.Sleep(300);

                        wizard.UpdateProgressText("Copying over files...");
                        this.CopyServerFiles(tempPath, this.ServerInstallLocation);
                        wizard.UpdateProgressValue(0.8m);
                        Thread.Sleep(300);

                        // register SACS if a new installation
                        if (!this.IsServerUpgrade)
                        {
                            wizard.UpdateProgressText("Registering SACS Agent as a Windows service...");
                            FileInfo exeFile = new FileInfo(this.ServerInstallLocation + "\\SACS.WindowsService.exe");
                            if (exeFile.Exists)
                            {
                                Process.Start(new ProcessStartInfo { FileName = exeFile.FullName, Arguments = "install" });
                            }
                            else
                            {
                                MessageBox.Show("Installation corrupt: executable file missing from installation. Aborting.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new InstallException("SACS.WindowsService.exe missing");
                            }
                        }

                        wizard.UpdateProgressValue(1m);
                        Thread.Sleep(1000);

                        wizard.HideProgressDialog();
                        wizard.PerformOnUI(() => completionCallback(true));
                    }
                    catch (InstallException ie)
                    {
                        // TODO: log exception somewhere.
                        wizard.HideProgressDialog();
                        wizard.PerformOnUI(() => completionCallback(false));
                    }
                });
        }

        /// <summary>
        /// Copies the server files.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void CopyServerFiles(string sourcePath, string targetPath)
        {
            var exclusions = new List<string>
            {
                "AppList.xml*",
                "*.mdf",
                "*.ldf",
                "*.config"
            };

            FileSystemUtilities.DirectoryCopy(sourcePath, targetPath, true, exclusions);
        }

        /// <summary>
        /// Determines whether this instance can install the server to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CanInstallServer(string path)
        {
            bool passed = true;

            // name check
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Installation location not specified.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }
            else if (!Path.IsPathRooted(path))
            {
                MessageBox.Show("Location must be an absolute path.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }
            else if (path.Replace('\\', '/').StartsWith("//"))
            {
                MessageBox.Show("Cannot install to network path. Location must be on this machine.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }
            else if (Path.GetPathRoot(path) == path.Trim())
            {
                MessageBox.Show("Cannot install to the root path. Please select a subdirectory.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }

            // directory / file check
            if (passed)
            {
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] directories = Directory.GetDirectories(path);
                    if (files.Length > 0 || directories.Length > 0)
                    {
                        if (files.Length > 0 && files.Any(f => f.Contains("SACS.WindowsService")))
                        {
                            if (!this.IsServerUpgrade)
                            {
                                var useResult = MessageBox.Show("It appears SACS files already exist, but it is not yet installed. Use this location?", "Files already exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (useResult == DialogResult.No)
                                {
                                    passed = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("SACS server cannot be installed as there are other files at this location.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            passed = false;
                        }
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    var createResult = MessageBox.Show("Path does not exist. Create?", "Path not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (createResult == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        passed = false;
                    }
                }
            }

            return passed;
        }

        /// <summary>
        /// Tries to stop the service.
        /// </summary>
        /// <returns></returns>
        private bool TryStopService()
        {
            bool success = true;

            if (this.SacsServiceController != null)
            {
                try
                {
                    this.SacsServiceController.Stop();
                    this.SacsServiceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 15));
                }
                catch (InvalidOperationException)
                {
                    if (this.SacsServiceController.Status != ServiceControllerStatus.Stopped)
                    {
                        MessageBox.Show("SACS Agent failed to stop. Stop the service manually and try again.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        success = false;
                    }
                }
                catch (System.ServiceProcess.TimeoutException)
                {
                    MessageBox.Show("Could not stop SACS agent - timeout has expired.", "Server install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    success = false;
                }
            }

            return success;
        }

        #endregion Server

        #region Windows Console

        /// <summary>
        /// Gets the windows console version number, if it can find it.
        /// </summary>
        /// <returns></returns>
        public string GetWindowsConsoleVersion()
        {
            if (this.WindowsConsoleInstallLocation != null)
            {
                try
                {
                    var fileVersion = FileVersionInfo.GetVersionInfo(Path.Combine(this.WindowsConsoleInstallLocation, "SACS.Windows.exe"));
                    return fileVersion.ProductVersion;
                }
                catch (FileNotFoundException)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Installs the Windows Management Console to the specified path.
        /// </summary>
        /// <param name="completionCallback">The completion callback.</param>
        public void InstallWindowsConsole(Action<bool> completionCallback)
        {
            if (!this.CanInstallWindowsConsole(this.WindowsConsoleInstallLocation))
            {
                return;
            }

            var installTask = Task.Run(() =>
            {
                WizardManager wizard = WizardManager.Current;
                string tempPath = Path.GetTempPath() + "SACS.Windows";

                try
                {
                    wizard.ShowProgressDialog();

                    wizard.UpdateProgressText("Extracting files...");
                    FileSystemUtilities.ExtractFromResource("SACS.Setup.Resources.SACS.Windows.zip", tempPath, "SACS.Windows");
                    wizard.UpdateProgressValue(0.2m);
                    Thread.Sleep(300);

                    wizard.UpdateProgressText("Backing up files...");
                    FileSystemUtilities.BackupDirectory(this.WindowsConsoleInstallLocation, "SACS.Windows");
                    wizard.UpdateProgressValue(0.5m);
                    Thread.Sleep(300);

                    wizard.UpdateProgressText("Copying over files...");
                    this.CopyServerFiles(tempPath, this.WindowsConsoleInstallLocation);
                    wizard.UpdateProgressValue(0.8m);
                    Thread.Sleep(300);

                    // create shortcut if new installation.
                    if (!this.IsWindowsConsoleUpgrade)
                    {
                        wizard.UpdateProgressText("Creating shortcut...");
                        AddWindowsShortcut(Path.Combine(this.WindowsConsoleInstallLocation, "SACS.Windows.exe"));
                    }

                    wizard.UpdateProgressValue(1m);
                    Thread.Sleep(1000);

                    wizard.HideProgressDialog();
                    wizard.PerformOnUI(() => completionCallback(true));
                }
                catch (InstallException ie)
                {
                    // TODO: log exception somewhere.
                    wizard.HideProgressDialog();
                    wizard.PerformOnUI(() => completionCallback(false));
                }
            });
        }

        /// <summary>
        /// Adds the windows shortcut.
        /// </summary>
        /// <param name="pathToExe">The path to executable.</param>
        private static void AddWindowsShortcut(string pathToExe)
        {
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", ConfigurationManager.AppSettings["SacsStartFolder"]);

            if (!Directory.Exists(appStartMenuPath))
            {
                Directory.CreateDirectory(appStartMenuPath);
            }

            string shortcutLocation = Path.Combine(appStartMenuPath, ConfigurationManager.AppSettings["WindowsShortcutName"] + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = ConfigurationManager.AppSettings["WindowsShortcutDescription"];
            //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
            shortcut.TargetPath = pathToExe;
            shortcut.Save();
        }

        /// <summary>
        /// Determines whether this instance can install the windows console to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CanInstallWindowsConsole(string path)
        {
            bool passed = true;

            // name check
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Installation location not specified.", "Windows Management Console install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }
            else if (!Path.IsPathRooted(path))
            {
                MessageBox.Show("Location must be an absolute path.", "Windows Management Console install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }
            else if (Path.GetPathRoot(path) == path.Trim())
            {
                MessageBox.Show("Cannot install to the root path. Please select a subdirectory.", "Windows Management Console install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passed = false;
            }

            // directory / file check
            if (passed)
            {
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] directories = Directory.GetDirectories(path);
                    if (files.Length > 0 || directories.Length > 0)
                    {
                        if (files.Length > 0 && files.Any(f => f.Contains("SACS.Windows.exe")))
                        {
                            if (!this.IsServerUpgrade)
                            {
                                var useResult = MessageBox.Show("It appears Windows Management Console files already exist, but it is not yet installed. Use this location?", "Files already exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (useResult == DialogResult.No)
                                {
                                    passed = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Windows Management Console cannot be installed as there are other files at this location.", "Windows Management Console install error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            passed = false;
                        }
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    var createResult = MessageBox.Show("Path does not exist. Create?", "Path not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (createResult == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        passed = false;
                    }
                }
            }

            // make sure the console is not running at that location
            if (passed)
            {
                var process = Process.GetProcessesByName("SACS.Windows");
                if (process.Any())
                {
                    MessageBox.Show("Please close all instances of the SACS Windows Management Console before continuing.", "Console still running", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    passed = false;
                }
            }

            return passed;
        }

        #endregion Windows Console

        #endregion Methods
    }
}