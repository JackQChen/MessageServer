using System;
using System.Globalization;

namespace SystemFramework
{
    public class ChineseDate
    {
        public ChineseDate()
        {
        }

        ///<summary> 
        /// 实例化一个 ChineseLunisolarCalendar 
        ///</summary> 
        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();

        ///<summary> 
        /// 十天干 
        ///</summary> 
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        ///<summary> 
        /// 十二地支 
        ///</summary> 
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        ///<summary> 
        /// 十二生肖 
        ///</summary> 
        private static string[] sx = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        ///<summary> 
        /// 返回农历天干地支年 
        ///</summary> 
        ///<param name="year">农历年</param> 
        ///<returns></returns> 
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tg[tgIndex], dz[dzIndex], "[", sx[dzIndex], "]");
            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }

        ///<summary> 
        /// 农历月 
        ///</summary> 

        ///<returns></returns> 
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

        ///<summary> 
        /// 农历日 
        ///</summary> 
        private static string[] days1 = { "初", "十", "廿", "三" };
        ///<summary> 
        /// 农历日 
        ///</summary> 
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


        ///<summary> 
        /// 返回农历月 
        ///</summary> 
        ///<param name="month">月份</param> 
        ///<returns></returns> 
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }

        ///<summary> 
        /// 返回农历日 
        ///</summary> 
        ///<param name="day">天</param> 
        ///<returns></returns> 
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }



        ///<summary> 
        /// 根据公历获取农历日期 
        ///</summary> 
        ///<param name="datetime">公历日期</param> 
        ///<returns></returns> 
        public static string GetChineseDateTime(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月 
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月 
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            return string.Concat(GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));
        }

    }
}
