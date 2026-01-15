using Microsoft.AspNetCore.Identity.UI.Services;

namespace Workshop04.Services
{
    public class DevEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For development: just log instead of sending email
            Console.WriteLine($"Email to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
