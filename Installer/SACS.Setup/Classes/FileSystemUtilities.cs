using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Shell32;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// Combines common file system tasks
    /// </summary>
    public static class FileSystemUtilities
    {
        // TODO: Create a provider so that this class can be used in unit tests.
        ////public static FileSystemProvider Provider { get; set; }

        /// <summary>
        /// Extracts a zip file from the specified embedded resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="pathValidation">The path validation string.</param>
        public static void ExtractFromResource(string resourceName, string destination, string pathValidation)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(resourceName));
            Debug.Assert(!string.IsNullOrWhiteSpace(destination));

            var currentAssembly = Assembly.GetAssembly(typeof(FileSystemUtilities));

            if (destination.Contains(pathValidation))
            {
                if (Directory.Exists(destination))
                {
                    Directory.Delete(destination, true);
                }

                using (var archive = new ZipArchive(currentAssembly.GetManifestResourceStream(resourceName), System.IO.Compression.ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(destination);
                }
            }
            else
            {
                throw new InvalidOperationException("Incorrect path was detected. Aborting delete: " + destination);
            }
        }

        /// <summary>
        /// Backs up the target path into a zip with the specified backup name.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="backupName">Name of the backup.</param>
        public static void BackupDirectory(string targetPath, string backupName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(targetPath));
            Debug.Assert(!string.IsNullOrWhiteSpace(backupName));

            if (!Directory.Exists(targetPath) ||
                Directory.GetFiles(targetPath).Length == 0 ||
                Directory.GetDirectories(targetPath).Length == 0)
            {
                // can't backup a non-existent location.
                return;
            }

            DirectoryInfo parent = Directory.GetParent(targetPath);
            string backupPath = parent.FullName + "\\SACS_Backup\\" + backupName;

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            string archiveName = string.Format("{0}\\{1}_{2}.zip", backupPath, backupName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            ZipFile.CreateFromDirectory(targetPath, archiveName, CompressionLevel.Optimal, false);
        }

        /// <summary>
        /// Directories the copy.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        /// <param name="copySubDirs">if set to <c>true</c> [copy sub dirs].</param>
        /// <param name="excludes">The list of file/directory patterns to not overwrite.</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">Source directory does not exist or could not be found</exception>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, IList<string> excludes = null)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            var excludeDirs = FilterDirectories(dir, excludes);

            int tries = 10;
            string lastFile = null;
            string lastDirectory = null;
            while (tries > 0)
            {
                try
                {
                    if (!Directory.Exists(destDirName))
                    {
                        Directory.CreateDirectory(destDirName);
                    }

                    var files = dir.GetFiles();
                    var excludeFiles = FilterFiles(dir, excludes);
                    foreach (FileInfo file in files)
                    {
                        string temppath = Path.Combine(destDirName, file.Name);

                        if (!excludeFiles.Any(f => f.FullName == file.FullName) || !File.Exists(temppath))
                        {
                            lastFile = temppath;
                            file.CopyTo(temppath, true);
                        }
                    }

                    if (copySubDirs)
                    {
                        foreach (DirectoryInfo subdir in dirs)
                        {
                            string temppath = Path.Combine(destDirName, subdir.Name);

                            if (!excludeDirs.Any(d => d.FullName == subdir.FullName) || !Directory.Exists(temppath))
                            {
                                lastDirectory = subdir.FullName;
                                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                            }
                        }
                    }
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                    tries--;
                }
            }

            if (tries == 0)
            {
                throw new IOException(
                    string.Format("Could not copy files to new destination. Maximum number of tries exceeded. File: {0}. Directory: {1}",
                        lastFile,
                        lastDirectory));
            }
        }

        /// <summary>
        /// Systematically searches the base directory, sub directories and shortcuts for a single file.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="baseDirectory">The starting directory to search in.</param>
        public static FileInfo FindFile(string searchPattern, string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                return null;
            }

            DirectoryInfo dir = new DirectoryInfo(baseDirectory);
            var firstMatchFiles = dir.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

            if (firstMatchFiles.Any())
            {
                return firstMatchFiles.First();
            }

            // no match so manually match the files, including shortcuts.
            FileInfo foundFile = null;
            foreach (var subdir in dir.GetDirectories())
            {
                foundFile = FindFile(searchPattern, subdir.FullName);
                if (foundFile != null)
                {
                    break;
                }
            }

            if (foundFile == null)
            {
                foreach (var file in dir.GetFiles())
                {
                    string targetFile = GetShortcutTargetFile(file.FullName);
                    if (!string.IsNullOrWhiteSpace(targetFile))
                    {
                        FileInfo fileInfo = new FileInfo(targetFile);
                        if (Regex.IsMatch(fileInfo.Name, searchPattern.Replace("*", ".*")))
                        {
                            foundFile = fileInfo;
                            break;
                        }
                    }
                }
            }

            return foundFile;
        }

        /// <summary>
        /// Gets the shortcut target file.
        /// </summary>
        /// <param name="shortcutFilename">The shortcut filename.</param>
        /// <returns></returns>
        private static string GetShortcutTargetFile(string shortcutFilename)
        {
            if ((Path.GetExtension(shortcutFilename) ?? string.Empty).ToLower() != ".lnk")
            {
                return shortcutFilename;
            }

            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        /// <summary>
        /// Filters the directories.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        private static IEnumerable<DirectoryInfo> FilterDirectories(DirectoryInfo source, IList<string> excludes)
        {
            IList<string> finalExcludesList = excludes ?? new List<string>();

            foreach (var pattern in finalExcludesList)
            {
                var matches = source.GetDirectories(pattern);
                foreach (var directory in matches)
                {
                    yield return directory;
                }
            }
        }

        /// <summary>
        /// Filters the files.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        private static IEnumerable<FileInfo> FilterFiles(DirectoryInfo source, IList<string> excludes)
        {
            IList<string> finalExcludesList = excludes ?? new List<string>();

            foreach (var pattern in finalExcludesList)
            {
                var matches = source.GetFiles(pattern);
                foreach (var file in matches)
                {
                    yield return file;
                }
            }
        }

        /// <summary>
        /// Quick class to help with file comparisons (in Linq)
        /// </summary>
        private class FileInfoComparer : IEqualityComparer<FileInfo>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
            /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(FileInfo x, FileInfo y)
            {
                if (x != null && y != null)
                {
                    return x.FullName == y.FullName;
                }

                return false;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
            /// </returns>
            public int GetHashCode(FileInfo obj)
            {
                return obj.FullName.GetHashCode();
            }
        }

        /// <summary>
        /// Quick class to help with directory comparisons (in Linq)
        /// </summary>
        private class DirectoryInfoComparer : IEqualityComparer<DirectoryInfo>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
            /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(DirectoryInfo x, DirectoryInfo y)
            {
                if (x != null && y != null)
                {
                    return x.FullName == y.FullName;
                }

                return false;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
            /// </returns>
            public int GetHashCode(DirectoryInfo obj)
            {
                return obj.FullName.GetHashCode();
            }
        }
    }
}