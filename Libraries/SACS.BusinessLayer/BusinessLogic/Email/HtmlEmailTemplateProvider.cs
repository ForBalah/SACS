using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    public class HtmlEmailTemplateProvider : EmailTemplateProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlEmailTemplateProvider"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public HtmlEmailTemplateProvider(string path) : base(path) { }

        /// <summary>
        /// Gets the formatted value equivalent
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetFormattedValue(string value)
        {
            return HttpUtility.HtmlEncode(value).Replace("\r\n", "\n").Replace("\n", "<br/>");
        }
    }
}
