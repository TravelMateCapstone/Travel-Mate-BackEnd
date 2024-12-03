namespace TravelMateAPI.Services.CCCDValid
{
    public class VerifySignatureRequest
    {
        public int UserId { get; set; }
        public string PublicKey { get; set; }
    }
}
