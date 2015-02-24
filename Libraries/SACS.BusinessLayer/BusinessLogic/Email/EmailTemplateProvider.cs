using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// Provides means for templating emails
    /// </summary>
    public abstract class EmailTemplateProvider
    {
        private string _template;
        protected Dictionary<string, string> values = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateProvider"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public EmailTemplateProvider(string path)
        {
            this._template = File.ReadAllText(Path.GetFullPath(path));
        }

        /// <summary>
        /// Adds the template value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddTemplateValue(string key, string value)
        {
            this.values.Add(key, value);
        }

        /// <summary>
        /// Gets the email message from the template
        /// </summary>
        /// <returns></returns>
        public EmailMessage GetEmailMessage()
        {
            EmailMessage message = new EmailMessage();
            string body = this._template;

            this.values.ToList().ForEach(kv =>
            {
                string directive = "#{" + kv.Key + "}#";
                body = body.Replace(directive, this.GetFormattedValue(kv.Value ?? string.Empty));
            });

            message.Body = body;

            return message;
        }

        /// <summary>
        /// Gets the formatted value equivalent
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract string GetFormattedValue(string value);
    }
}
