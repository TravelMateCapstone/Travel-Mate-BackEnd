namespace BusinessObjects
{
    public class GetTimeZone
    {
        public static DateTime GetVNTimeZoneNow()
        {
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, vnTimeZone);
        }
    }
}
