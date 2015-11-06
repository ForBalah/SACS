using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.TestApp
{
    [Serializable]
    public class TestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TestException(string message)
            : base(message)
        {
        }
    }
}