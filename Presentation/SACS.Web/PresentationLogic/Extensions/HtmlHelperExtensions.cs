using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACS.Web.PresentationLogic.Extensions
{
    /// <summary>
    /// Extension methods for the MVC Html Hepler
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified 
        /// action with an image inside of it.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="action">The name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="imagePath">The image path.</param>
        /// <param name="alt">The alternate text to display.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }
    }
}