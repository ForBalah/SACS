using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using SACS.Common.PInvoke;
using SACS.Common.Security;

namespace SACS.Common.Runtime
{
    /// <summary>
    /// Wraps the <see cref="Process" /> class, providing means of 
    /// running a process as a different user from a Windows service.
    /// </summary>
    public class ProcessWrapper : IDisposable
    {
        #region Fields

        private static object createProcessLock = new object();
        private Process _process;
        private StreamWriter _standardInput;
        private StreamReader _standardOutput;
        private StreamReader _standardError;
        private AnonymousPipeServerStream _pipeServerToClient;
        private AnonymousPipeServerStream _pipeServerFromClient;
        private AnonymousPipeServerStream _pipeServerErrorFromClient;
        private bool _enableCustomUserLogin;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessWrapper"/> class.
        /// </summary>
        /// <param name="enableCustomUserLogin">Whether the new custom user login code should be used.</param>
        public ProcessWrapper(bool enableCustomUserLogin)
        {
            ArgumentObject = new Dictionary<string, object>();
            _process = new Process();
            _enableCustomUserLogin = enableCustomUserLogin;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command arguments object that will be serialized when the process is started.
        /// </summary>
        public IDictionary<string, object> ArgumentObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value that the associated process specified when it terminated.
        /// </summary>
        /// <returns>
        /// The code that the associated process specified when it terminated.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The process has not exited.-or-
        /// The process <see cref="P:System.Diagnostics.Process.Handle" /> is not valid. 
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">You are trying to access the
        /// <see cref="P:System.Diagnostics.Process.ExitCode" /> property for a process that
        /// is running on a remote computer. This property is available only for processes 
        /// that are running on the local computer.
        /// </exception>
        public virtual int ExitCode
        {
            get
            {
                try
                {
                    return _process.ExitCode;
                }
                catch (InvalidOperationException)
                {
                    // can be any reason why the process no longer exists. so just assume it's valid
                    // and return 0.
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the associated process has been terminated.
        /// </summary>
        /// <returns>
        /// true if the operating system process referenced by the <see cref="T:System.Diagnostics.Process" /> 
        /// component has terminated; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// There is no process associated with the object.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// The exit code for the process could not be retrieved.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">You are trying to access the 
        /// <see cref="P:System.Diagnostics.Process.HasExited" />
        /// property for a process that is running on a remote computer. This property is 
        /// available only for processes that are running on the local computer.
        /// </exception>
        public virtual bool HasExited
        {
            get
            {
                return _process.HasExited;
            }
        }

        /// <summary>
        /// Gets a stream used to read the error output of the application.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard error 
        /// stream of the application.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has not been defined
        /// for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" />
        /// is set to true and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> 
        /// is set to false.- or - The <see cref="P:System.Diagnostics.Process.StandardError" /> stream 
        /// has been opened for asynchronous read operations with 
        /// <see cref="M:System.Diagnostics.Process.BeginErrorReadLine" />. 
        /// </exception>
        public virtual StreamReader StandardError
        {
            get
            {
                if (_standardError != null)
                {
                    return _standardError;
                }

                return _process.StandardError;
            }
        }

        /// <summary>
        /// Gets a stream used to write the input of the application.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IO.StreamWriter" /> that can be used to write the standard input
        /// stream of the application.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.Diagnostics.Process.StandardInput" /> stream has not been 
        /// defined because <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" /> 
        /// is set to false.
        /// </exception>
        public virtual StreamWriter StandardInput
        {
            get
            {
                if (_standardInput != null)
                {
                    return _standardInput;
                }

                return _process.StandardInput;
            }
        }

        /// <summary>
        /// Gets a stream used to read the output of the application.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard output
        /// stream of the application.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has not been defined
        /// for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />
        /// is set to true and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> is
        /// set to false.- or - The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream 
        /// has been opened for asynchronous read operations with <see cref="M:System.Diagnostics.Process.BeginOutputReadLine" />.
        /// </exception>
        public virtual StreamReader StandardOutput
        {
            get
            {
                if (_standardOutput != null)
                {
                    return _standardOutput;
                }

                return _process.StandardOutput;
            }
        }

        /// <summary>
        /// Gets or sets the properties to pass to the <see cref="M:System.Diagnostics.Process.Start" />
        /// method of the <see cref="T:System.Diagnostics.Process" />.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that represents the data with
        /// which to start the process. These arguments include the name of the executable file or
        /// document used to start the process.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The value that specifies the <see cref="P:System.Diagnostics.Process.StartInfo" /> 
        /// is null. 
        /// </exception>
        public virtual ProcessStartInfo StartInfo
        {
            get
            {
                return _process.StartInfo;
            }

            set
            {
                _process.StartInfo = value;
            }
        }

        /// <summary>
        /// Gets the time that the associated process was started.
        /// </summary>
        /// <returns>A <see cref="T:System.DateTime" /> that indicates when the process started. 
        /// This only has meaning for started processes.
        /// </returns>
        /// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or
        /// Windows Millennium Edition (Windows Me), which does not support this property. 
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">You are attempting to access the 
        /// <see cref="P:System.Diagnostics.Process.StartTime" /> property for a process that is 
        /// running on a remote computer. This property is available only for processes that are 
        /// running on the local computer. 
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The process has exited.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// An error occurred in the call to the Windows function.
        /// </exception>
        public virtual DateTime StartTime
        {
            get
            {
                return _process.StartTime;
            }
        }

        /// <summary>
        /// Gets the unique identifier for the associated process.
        /// </summary>
        /// <returns>The system-generated unique identifier of the process that is referenced by
        /// this <see cref="T:System.Diagnostics.Process" /> instance.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// The process's <see cref="P:System.Diagnostics.Process.Id" /> property has not been 
        /// set.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> 
        /// object.
        /// </exception>
        /// <exception cref="T:System.PlatformNotSupportedException">
        /// The platform is Windows 98 or Windows Millennium Edition (Windows Me); set the 
        /// <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to 
        /// false to access this property on Windows 98 and Windows Me.
        /// </exception>
        public virtual int Id
        {
            get
            {
                return _process.Id;
            }
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
        /// Immediately stops the associated process.
        /// </summary>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// The associated process could not be terminated. -or-The process is terminating.-or-
        /// The associated process is a Win16 executable.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// You are attempting to call <see cref="M:System.Diagnostics.Process.Kill" /> for a 
        /// process that is running on a remote computer. The method is available only for processes
        /// running on the local computer.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The process has already exited. -or-There is no process associated with this 
        /// <see cref="T:System.Diagnostics.Process" /> object.
        /// </exception>
        public virtual void Kill()
        {
            _process.Kill();
        }

        /// <summary>
        /// Starts (or reuses) the process resource that is specified by the <see cref="P:System.Diagnostics.Process.StartInfo" />
        /// property of this <see cref="T:System.Diagnostics.Process" /> component and associates it with the component.
        /// </summary>
        /// <returns>true if a process resource is started; false if no new process resource is started 
        /// (for example, if an existing process is reused).</returns>
        /// <exception cref="T:System.InvalidOperationException">No file name was specified in the 
        /// <see cref="T:System.Diagnostics.Process" /> component's <see cref="P:System.Diagnostics.Process.StartInfo" />.
        /// -or- The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> member of the
        /// <see cref="P:System.Diagnostics.Process.StartInfo" /> property is true while 
        /// <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, 
        /// <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, 
        /// or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is true. 
        /// </exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
        public virtual bool Start()
        {
            // temporary switch until code is proven to be stable.
            if (_enableCustomUserLogin)
            {
                if (!string.IsNullOrWhiteSpace(StartInfo.Arguments))
                {
                    throw new ArgumentException("StartInfo.Arguments is not allowed when starting a process as a user. use ArgumentObject.");
                }
            }

            bool successfulStart = false;
            if (_enableCustomUserLogin && StartInfo != null && !string.IsNullOrWhiteSpace(StartInfo.UserName))
            {
                using (Impersonator imp = new Impersonator())
                {
                    imp.Impersonate(StartInfo.UserName, StartInfo.Domain, StartInfo.Password, StartWithCreateProcessAsUser);
                    successfulStart = true;
                }
            }
            else
            {
                _process.StartInfo.Arguments = JsonConvert.SerializeObject(new
                    {
                        name = ArgumentObject["name"],
                        owner = ArgumentObject["owner"],
                        parameters = ArgumentObject.ContainsKey("parameters") ? ArgumentObject["parameters"] : string.Empty
                    });
                successfulStart = _process.Start();
            }

            return successfulStart;
        }

        /// <summary>Instructs the <see cref="T:System.Diagnostics.Process" /> component to wait the
        /// specified number of milliseconds for the associated process to exit.</summary>
        /// <returns>true if the associated process has exited; otherwise, false.</returns>
        /// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated
        /// process to exit. The maximum is the largest possible value of a 32-bit integer, which 
        /// represents infinity to the operating system. </param>
        /// <exception cref="T:System.ComponentModel.Win32Exception">The wait setting could not be accessed. </exception>
        /// <exception cref="T:System.SystemException">No process <see cref="P:System.Diagnostics.Process.Id" /> 
        /// has been set, and a <see cref="P:System.Diagnostics.Process.Handle" /> from which the
        /// <see cref="P:System.Diagnostics.Process.Id" /> property can be determined does not exist.
        /// -or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
        /// -or- You are attempting to call <see cref="M:System.Diagnostics.Process.WaitForExit(System.Int32)" /> for 
        /// a process that is running on a remote computer. This method is available only for processes that are 
        /// running on the local computer. </exception>
        public bool WaitForExit(int milliseconds)
        {
            try
            {
                return _process.WaitForExit(milliseconds);
            }
            catch (InvalidOperationException)
            {
                // the process is not running so there's nothing to wait for.
                return true;
            }
        }

        /// <summary>
        /// Release all resources used by this process.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _process.Close();
                _pipeServerToClient.Close();
                _pipeServerFromClient.Close();
                _pipeServerErrorFromClient.Close();
            }
        }

        /// <summary>
        /// Uses P/Invoke to start the process using the login token obtained for the user.
        /// </summary>
        /// <param name="token">The login token to use to create the process with.</param>
        private void StartWithCreateProcessAsUser(IntPtr token)
        {
            var startinfo = _process.StartInfo;
            var path = Path.GetFullPath(startinfo.FileName);
            var dir = Path.GetDirectoryName(path);
            bool processCreated = false;

            Win32NativeMethods.PROCESS_INFORMATION pi = new Win32NativeMethods.PROCESS_INFORMATION();
            Win32NativeMethods.SECURITY_ATTRIBUTES saProcess = new Win32NativeMethods.SECURITY_ATTRIBUTES();
            saProcess.bInheritHandle = true;
            saProcess.nLength = (uint)Marshal.SizeOf(saProcess);

            Win32NativeMethods.SECURITY_ATTRIBUTES saThread = new Win32NativeMethods.SECURITY_ATTRIBUTES();
            saThread.bInheritHandle = true;
            saThread.nLength = (uint)Marshal.SizeOf(saThread);

            Win32NativeMethods.STARTUPINFO si = new Win32NativeMethods.STARTUPINFO();
            si.lpDesktop = string.Empty;
            si.cb = (uint)Marshal.SizeOf(si);

            lock (createProcessLock)
            {
                _pipeServerToClient = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
                _pipeServerFromClient = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
                _pipeServerErrorFromClient = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);

                try
                {
                    // although these are separate if statements, in reality all standard streams must be
                    // redirected otherwise it just won't work in SACS.
                    if (startinfo.RedirectStandardInput)
                    {
                        _standardInput = new StreamWriter(_pipeServerToClient);
                        _standardInput.AutoFlush = true;
                    }

                    if (startinfo.RedirectStandardOutput)
                    {
                        Encoding encoding = (startinfo.StandardOutputEncoding != null) ? startinfo.StandardOutputEncoding : Console.OutputEncoding;
                        _standardOutput = new StreamReader(_pipeServerFromClient);
                    }

                    if (startinfo.RedirectStandardError)
                    {
                        Encoding encoding2 = (startinfo.StandardErrorEncoding != null) ? startinfo.StandardErrorEncoding : Console.OutputEncoding;
                        _standardError = new StreamReader(_pipeServerErrorFromClient);
                    }

                    ArgumentObject["pipeIn"] = _pipeServerToClient.GetClientHandleAsString();
                    ArgumentObject["pipeOut"] = _pipeServerFromClient.GetClientHandleAsString();
                    ArgumentObject["pipeErr"] = _pipeServerErrorFromClient.GetClientHandleAsString();

                    processCreated = Win32NativeMethods.CreateProcessAsUser(
                            token,
                            path,
                            string.Format("\"{0}\" {1}", startinfo.FileName.Replace("\"", "\"\""), JsonConvert.SerializeObject(this.ArgumentObject)),
                            ref saProcess,
                            ref saThread,
                            true,
                            0,
                            IntPtr.Zero,
                            dir,
                            ref si,
                            out pi);

                    if (!processCreated)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }

                    // For the last step, since we're not using the in-build methods to start the process, 
                    // we need to replace the now outdated process
                    _process = Process.GetProcessById((int)pi.dwProcessId);
                }
                finally
                {
                    _pipeServerToClient.DisposeLocalCopyOfClientHandle();
                    _pipeServerFromClient.DisposeLocalCopyOfClientHandle();
                    _pipeServerErrorFromClient.DisposeLocalCopyOfClientHandle();

                    if (pi.hProcess != IntPtr.Zero)
                    {
                        Win32NativeMethods.CloseHandle(pi.hProcess);
                    }

                    if (pi.hThread != IntPtr.Zero)
                    {
                        Win32NativeMethods.CloseHandle(pi.hThread);
                    }
                }
            }
        }

        #endregion
    }
}
