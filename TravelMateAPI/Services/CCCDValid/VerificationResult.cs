namespace TravelMateAPI.Services.CCCDValid
{
    public class VerificationResult
    {
        public bool IsVerified { get; set; }
        public string Message { get; set; }

        public bool IsPublicSignatureVerified { get; set; }
        public string PublicSignatureMessage { get; set; }

        public bool IsFullyVerified => IsVerified  && IsPublicSignatureVerified;
    }

}
