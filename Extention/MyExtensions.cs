using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace taavoni3.Extention
{
    public static class MyExtensions
    {
        public static string PersianToEnglish(this string persianStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };
            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }



        public static string ToPersianDate(this DateTime dateTime)
        {
            string persianDate = "";
            if (dateTime != DateTime.MinValue)
            {
                var persianCalendar = new PersianCalendar();
                var date = dateTime;
                persianDate =
                $"{persianCalendar.GetYear(date):0000}/{persianCalendar.GetMonth(date):00}/{persianCalendar.GetDayOfMonth(date):00}";
            }
            else
            {
                persianDate = "تاریخ معتبر نیست"; // یا می‌توانید تاریخ پیش‌فرض بگذارید
            }
            return persianDate;
        }
        public static string MyUnit(this string str){
        
            return str + " تومان";
        }


public static string ToPersianUnit(this string str){
return str + "تومان ";
}


    }

}