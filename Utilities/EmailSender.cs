using Microsoft.AspNetCore.Identity.UI.Services;

namespace Utilities
{
    /// <summary>
    /// Provides an implementation of the IEmailSender interface for sending emails.
    /// This class currently does not implement actual email sending but returns a completed task.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// Currently, this method does not send an actual email but completes the task immediately.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlMessage">The HTML body content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implementation for email sending will go here
            return Task.CompletedTask;
        }
    }
}
