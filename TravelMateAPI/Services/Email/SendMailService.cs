using BusinessObjects.Configuration;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace TravelMateAPI.Services.Email
{
    public class SendMailService : IMailServiceSystem
    {
        private readonly AppSettings _appSettings;

        private readonly ILogger<SendMailService> logger;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public SendMailService(AppSettings appSettings, ILogger<SendMailService> _logger)
        {
            _appSettings = appSettings;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        // Gửi email, theo nội dung trong mailContent
        public async Task SendMail(MailContent mailContent)
        {
            // Lấy giá trị từ Key Vault
            //var keyVaultUrl = new Uri("https://travelmatekeyvault.vault.azure.net/");
            //var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());
            // Lấy Mail settings từ Key Vault
            //var mailAddress = (await client.GetSecretAsync("MailAddress")).Value.Value;
            //var mailDisplayName = (await client.GetSecretAsync("MailDisplayName")).Value.Value;
            //var mailPassword = (await client.GetSecretAsync("MailPassword")).Value.Value;
            //var mailHost = (await client.GetSecretAsync("MailHost")).Value.Value;
            //var mailPortString = (await client.GetSecretAsync("MailPort")).Value.Value;
            //var mailPort = int.Parse(mailPortString); // Chuyển đổi từ string sang int
            var mailAddress = _appSettings.MailSettings.Mail;
            var mailDisplayName = _appSettings.MailSettings.DisplayName;
            var mailPassword = _appSettings.MailSettings.Password;
            var mailHost = _appSettings.MailSettings.Host;
            var mailPort = _appSettings.MailSettings.Port;
            


            var email = new MimeMessage();
            //email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            //email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.Sender = new MailboxAddress(mailDisplayName, mailAddress);
            email.From.Add(new MailboxAddress(mailDisplayName, mailAddress));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                //smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                //smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                smtp.Connect(mailHost, mailPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailAddress, mailPassword);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + mailContent.To);

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }
    }
}
