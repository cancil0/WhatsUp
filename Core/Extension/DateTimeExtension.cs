namespace Core.Extension
{
    public static class DateTimeExtension
    {
        public static int DateToInt(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd").ToInt();
        }

        public static int TimeToInt(this DateTime dateTime)
        {
            return dateTime.ToString("HHmmss").ToInt();
        }
    }
}
