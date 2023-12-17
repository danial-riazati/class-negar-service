using System;
namespace ClassNegarService.Utils
{
    public class StringUtils
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static DateTime ConvertTimeStrigToDateTime(string timeString)
        {
            var time = new DateTime();
            try
            {
                var splitted = timeString.Split(':');
                if (splitted.Length != 2) throw new Exception();
                var arr = new int[] { int.Parse(splitted[0]), int.Parse(splitted[1]) };
                if (arr[0] < 0 || arr[0] > 23 || arr[0] < 0 || arr[0] > 60) throw new Exception();
                time.AddHours(arr[0]);
                time.AddMinutes(arr[1]);
                return time;
            }
            catch (Exception)
            {
                throw new Exception("time_string is not in currect format");
            }
        }
        public static string ConvertDateTimeToTimeStrig(DateTime time)
        {
            try
            {
                var timeString = $"{time.Hour}:{time.Minute}";
                return timeString;
            }
            catch (Exception)
            {
                throw new Exception("date_time is not in currect format");
            }
        }

    }
}

