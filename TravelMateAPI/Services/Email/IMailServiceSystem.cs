namespace TravelMateAPI.Services.Email
{
    public interface IMailServiceSystem
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
