namespace TravelMateAPI.Services.FindLocal
{
    public class LocalFeedbackDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public double AverageRate { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
