namespace BussinessObjects.Configuration
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
        public MailSettings MailSettings { get; set; }
        public FirebaseConfig FirebaseConfig { get; set; }
        public GoogleAuthSettings GoogleAuthSettings { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMinutes { get; set; }
    }
    public class GoogleAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class FirebaseConfig
    {
        public string ApiKey { get; set; }
        public string AuthDomain { get; set; }
        public string ProjectId { get; set; }
        public string StorageBucket { get; set; }
        public string MessagingSenderId { get; set; }
        public string AppId { get; set; }
        public string MeasurementId { get; set; }
        public string FirebaseAdminSdkJsonPath { get; set; }
    }
}
