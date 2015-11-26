using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Export
{
    /// <summary>
    /// The service app exporter to list
    /// </summary>
    public class ServiceAppListExporter
    {
        private static List<string> fieldExclusion = new List<string>
        {
            "Password",
            "CanStart",
            "CanStop",
            "IsRunning",
            "ImagePath",
            "Comparer",
            "CanRun"
        };

        /// <summary>
        /// Exports the tab delimited version of service apps.
        /// </summary>
        /// <param name="serviceAppList">The service application list.</param>
        /// <returns></returns>
        public string ExportTabDelimited(IList<ServiceApp> serviceAppList)
        {
            return this.ExportList(serviceAppList, "\t");
        }

        /// <summary>
        /// Exports the CSV version of service apps.
        /// </summary>
        /// <param name="serviceAppList">The service application list.</param>
        /// <returns></returns>
        public string ExportCsv(IList<ServiceApp> serviceAppList)
        {
            return this.ExportList(serviceAppList, ",");
        }

        /// <summary>
        /// Fixes the string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private static string FixString(string source, string delimiter)
        {
            // CSV has special rules so fix accordingly
            if (delimiter.Contains(","))
            {
                string result = source;
                
                // note order
                result = result.Replace("\"", "\"\""); // double-double quotes
                if (source.Contains(","))
                {
                    result = "\"" + result + "\"";
                }

                return result;
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Exports the list. The field order is based off the position of the properties in the ServiceApp class.
        /// </summary>
        /// <param name="serviceAppList">The service application list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private string ExportList(IList<ServiceApp> serviceAppList, string delimiter)
        {
            StringBuilder listBuilder = new StringBuilder();

            var properties = typeof(ServiceApp).GetProperties().Where(p => !fieldExclusion.Contains(p.Name)); // note order inside class file

            listBuilder.AppendLine(string.Join(delimiter, properties.Select(p => p.Name)));

            foreach (var app in serviceAppList.OrderBy(sa => sa.Name))
            {
                List<string> values = new List<string>();
                foreach (var property in properties)
                {
                    object value = property.GetValue(app);
                    if (value != null)
                    {
                        values.Add(FixString(value.ToString(), delimiter));
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }

                listBuilder.AppendLine(string.Join(delimiter, values));
            }

            return listBuilder.ToString();
        }
    }
}
