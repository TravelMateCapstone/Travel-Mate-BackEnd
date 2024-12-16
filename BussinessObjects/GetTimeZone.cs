namespace BusinessObjects
{
    public class GetTimeZone
    {
        public static DateTime GetVNTimeZoneNow()
        {
            // Lấy thời gian UTC hiện tại
            DateTime utcNow = DateTime.UtcNow;

            // Lấy thông tin múi giờ Việt Nam (SE Asia Standard Time)
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển từ UTC sang giờ Việt Nam
            DateTime vnTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vnTimeZone);

            return vnTime;
        }
    }
}
