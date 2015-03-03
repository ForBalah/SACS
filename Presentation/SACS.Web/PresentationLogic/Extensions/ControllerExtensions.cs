using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACS.Web.PresentationLogic.Extensions
{
    /// <summary>
    /// Controller Extension methods
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="e">The e.</param>
        public static void AddError(this Controller controller, Exception e)
        {
            controller.ViewBag.Message = e.Message;
            controller.ViewBag.StackTrace = HttpUtility.HtmlEncode(e.StackTrace).Replace(Environment.NewLine, "<br />");

            if (e.InnerException != null)
            {
                controller.ViewBag.InnerExceptionMessage = e.InnerException.Message;

                if (!string.IsNullOrEmpty(e.InnerException.StackTrace))
                {
                    controller.ViewBag.InnerExceptionStackTrace = HttpUtility.HtmlEncode(e.InnerException.StackTrace).Replace(Environment.NewLine, "<br />");
                }
            }
        }
    }
}