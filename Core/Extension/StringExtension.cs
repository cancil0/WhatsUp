namespace Core.Extension
{
    public static class StringExtension
    {
        public static bool ToBoolean(this string item)
        {
            return bool.TryParse(item, out var result) && result;
        }
        public static short ToShort(this string item)
        {
            return (short)(short.TryParse(item, out var result) ? result : 0);
        }
        public static int ToInt(this string item)
        {
            return int.TryParse(item, out var result) ? result : 0;
        }
        public static long ToLong(this string item)
        {
            return long.TryParse(item, out var result) ? result : 0;
        }

        public static string Nullify(this string item)
        {
            return string.IsNullOrEmpty(item) ? null : item;
        }
    }
}
