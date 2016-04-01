using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SACS.Common.Extensions;

namespace SACS.BusinessLayer.BusinessLogic.Conversions
{
    /// <summary>
    /// Converts between MTH and HTML
    /// </summary>
    public class MhtToHtml
    {
        /// <summary>
        /// Takes MHT content and converts it into an HTML document
        /// </summary>
        /// <param name="mhtContent">The mht content to convert.</param>
        /// <returns></returns>
        public string ConvertToHTMLDocument(string mhtContent)
        {
            Dictionary<string, ContentDetails> splitContent = new Dictionary<string, ContentDetails>();

            if (string.IsNullOrWhiteSpace(mhtContent))
            {
                return null;
            }

            var boundary = GetBoundary(mhtContent);

            string[] parts = mhtContent.Split(new string[] { boundary }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                ContentDetails contentDetails = ContentDetails.ExtractContentDetails(part);
                if (contentDetails.HasAllContentHeaders)
                {
                    splitContent.Add(contentDetails.ContentLocation, contentDetails);
                }
            }

            // WORK IN PROGRESS
            return splitContent.First(k => k.Key.Contains(".htm")).Value.Content;
        }

        /// <summary>
        /// Gets the text that represents the boundary in a mht doc
        /// </summary>
        /// <param name="contents">The contents to extract the boundary from.</param>
        /// <returns></returns>
        internal static string GetBoundary(string contents)
        {
            if (contents != null)
            {
                using (StringReader reader = new StringReader(contents))
                {
                    var boundRegex = new Regex(@"Content-Type:.* (boundary=""(.*?)"")");
                    var found = false;
                    var line = string.Empty;
                    var result = string.Empty;

                    while (!found && (line = reader.ReadLine()) != null)
                    {
                        var match = boundRegex.Match(line);
                        if (match.Success)
                        {
                            result = match.Groups[2].Value;
                            found = true;
                        }
                    }

                    if (found)
                    {
                        return result;
                    }
                }
            }

            throw new InvalidOperationException("boundary not found");
        }

        /// <summary>
        /// Wrapper class for the content fields in a mht part
        /// </summary>
        internal class ContentDetails
        {
            private static Regex ContentLocationRegex = new Regex("Content-Location: ?(.*)");
            private static Regex ContentTransferEncodingRegex = new Regex("Content-Transfer-Encoding: ?(.*)");
            private static Regex ContentTypeRegex = new Regex("Content-Type: ?(.*)");
            private static Regex ContentRegex = new Regex(@"(Content-Type: ?.*\s)([\s\S]+)");

            /// <summary>
            /// Gets or sets the Content-Location
            /// </summary>
            public string ContentLocation { get; set; }

            /// <summary>
            /// Gets or sets the Content-Transfer-Encoding
            /// </summary>
            public string ContentTransferEncoding { get; set; }

            /// <summary>
            /// Gets or sets the Content-Type
            /// </summary>
            public string ContentType { get; set; }

            /// <summary>
            /// Gets or sets the actual content
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// Gets a value indicating whether this instance has all the content properties filled in.
            /// </summary>
            public bool HasAllContentHeaders
            {
                get
                {
                    return !string.IsNullOrWhiteSpace(ContentLocation) &&
                           !string.IsNullOrWhiteSpace(ContentTransferEncoding) &&
                           !string.IsNullOrWhiteSpace(ContentType);
                }
            }

            /// <summary>
            /// Extracts the content into a new ContentDetails object.
            /// </summary>
            /// <param name="content">The content to extract from.</param>
            /// <returns></returns>
            public static ContentDetails ExtractContentDetails(string content)
            {
                ContentDetails details = new ContentDetails();
                Match match;
                match = ContentLocationRegex.Match(content);
                if (match.Success)
                {
                    details.ContentLocation = match.Groups[1].Value.RemoveNewLines(false);
                }

                match = ContentTransferEncodingRegex.Match(content);
                if (match.Success)
                {
                    details.ContentTransferEncoding = match.Groups[1].Value.RemoveNewLines(false);
                }

                match = ContentTypeRegex.Match(content);
                if (match.Success)
                {
                    details.ContentType = match.Groups[1].Value.RemoveNewLines(false);
                }

                match = ContentRegex.Match(content);
                if (match.Success)
                {
                    details.Content = match.Groups[2].Value.RemoveNewLines(false);
                }

                return details;
            }
        }
    }
}
