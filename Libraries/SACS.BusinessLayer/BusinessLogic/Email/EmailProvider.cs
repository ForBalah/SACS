namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// Class providing base email sending capabilities
    /// </summary>
    public abstract class EmailProvider
    {
        /// <summary>
        /// Sends the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>true</c> if the mail sent successfully, else, <c>false</c>.</returns>
        public abstract bool Send(EmailMessage email);
    }
}