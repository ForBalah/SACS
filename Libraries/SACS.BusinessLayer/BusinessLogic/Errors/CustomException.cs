using System;
using System.Runtime.Serialization;

namespace SACS.BusinessLayer.BusinessLogic.Errors
{
    /// <summary>
    /// A custom-built exception, designed to override the stacktrace
    /// </summary>
    [Serializable]
    public class CustomException : Exception
    {
        private string _stackTrace;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        public CustomException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="stackTrace">The stack trace to override the exception with.</param>
        public CustomException(string message, string stackTrace) : base(message)
        {
            _stackTrace = stackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        /// <param name="stackTrace">The stack trace to override the exception with.</param>
        public CustomException(string message, Exception inner, string stackTrace) : base(message, inner)
        {
            _stackTrace = stackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.</param>
        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                return _stackTrace ?? base.StackTrace;
            }
        }
    }
}
