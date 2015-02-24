using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Enums
{
    public class EnumWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumWrapper"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public EnumWrapper(Enum source)
        {
            this.Source = source;
            this.Value = Convert.ToInt32(source);
            this.Name = Enum.GetName(source.GetType(), source);
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public Enum Source { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
    }
}
