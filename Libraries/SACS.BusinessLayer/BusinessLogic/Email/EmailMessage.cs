using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// Class that wraps MailMessage (so that System.Net doesn't need to be included everywhere an email is sent)
    /// </summary>
    [Serializable]
    public class EmailMessage
    {
        private IList<string> _ToAddresses;
        private IList<string> _CCAddresses;

        /// <summary>
        /// Gets to addresses.
        /// </summary>
        /// <value>
        /// To addresses.
        /// </value>
        public IList<string> ToAddresses
        {
            get
            {
                if (this._ToAddresses == null)
                {
                    this._ToAddresses = new List<string>();
                }

                return this._ToAddresses;
            }
        }

        /// <summary>
        /// Gets the cc addresses.
        /// </summary>
        /// <value>
        /// The cc addresses.
        /// </value>
        public IList<string> CCAddresses
        {
            get
            {
                if (this._CCAddresses == null)
                {
                    this._CCAddresses = new List<string>();
                }

                return this._CCAddresses;
            }
        }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Adds the to addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        public void AddToAddresses(string addresses)
        {
            GetSplitAddresses(addresses).ToList().ForEach(a =>
            {
                this.ToAddresses.Add(a);
            });
        }

        /// <summary>
        /// Adds the cc addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        public void AddCCAddresses(string addresses)
        {
            GetSplitAddresses(addresses).ToList().ForEach(a =>
            {
                this.ToAddresses.Add(a);
            });
        }

        /// <summary>
        /// Gets the split addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        private static string[] GetSplitAddresses(string addresses)
        {
            return addresses.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
