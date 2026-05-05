namespace Proyecto_H.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo ZonaBolivia =
            TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time");

        public static DateTime ToBolivia(DateTime utcDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, ZonaBolivia);
        }
    }
}
