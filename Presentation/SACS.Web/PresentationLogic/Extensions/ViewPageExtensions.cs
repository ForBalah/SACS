using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace SACS.Web.PresentationLogic.Extensions
{
    /// <summary>
    /// Extension methods for the WebViewPage class.
    /// </summary>
    public static class ViewPageExtensions
    {
        private const string ScriptBlockBuilder = "ScriptBlockBuilder";

        /// <summary>
        /// Creates a script block on the page
        /// </summary>
        /// <param name="webPage">The web page.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>
        /// This allows javascript to be rendered from a partial view (by capturing it and placing it at the
        /// bottom of the page, as specified by the WriteScriptBlocks method).
        /// </para>
        /// <para>
        /// See: http://blog.logrythmik.com/post/A-Script-Block-Templated-Delegate-for-Inline-Scripts-in-Razor-Partials
        /// for more information.
        /// </para>
        /// </remarks>
        public static MvcHtmlString ScriptBlock(this WebViewPage webPage, Func<dynamic, HelperResult> template)
        {
            if (!webPage.IsAjax)
            {
                var scriptBuilder = webPage.Context.Items[ScriptBlockBuilder] as StringBuilder ?? new StringBuilder();

                scriptBuilder.Append(template(null).ToHtmlString());
                webPage.Context.Items[ScriptBlockBuilder] = scriptBuilder;

                return new MvcHtmlString(string.Empty);
            }

            return new MvcHtmlString(template(null).ToHtmlString());
        }

        /// <summary>
        /// Writes the script blocks to the page
        /// </summary>
        /// <param name="webPage">The web page.</param>
        /// <returns></returns>
        public static MvcHtmlString WriteScriptBlocks(this WebViewPage webPage)
        {
            var scriptBuilder = webPage.Context.Items[ScriptBlockBuilder] as StringBuilder ?? new StringBuilder();

            return new MvcHtmlString(scriptBuilder.ToString());
        }
    }
}