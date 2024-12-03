namespace BusinessObjects.Utils.Request
{
    public class CreatePaymentLinkRequestBody
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public int Price { get; set; }
        public string CancelUrl { get; set; }
    }
}
