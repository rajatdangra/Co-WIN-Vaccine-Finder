﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineFinder
{
    public static class DateTimeExtensions
    {
        public static bool IsInPeriod(this DateTime date, DateTime startDate, DateTime endDate, bool compareStartAndEOD = true)
        {
            if (compareStartAndEOD)
            {
                startDate = startDate.StartOfDay();
                endDate = endDate.EndOfDay();
            }
            return date >= startDate && date <= endDate;
        }

        public static bool IsDefault(this DateTime date)
        {
            return date == default(DateTime);
        }
        public static string ToDetailString(this DateTime date)
        {
            return date.ToString("dd/MMM/yyyy hh:mm:ss tt");
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(
             dateTime.Year,
             dateTime.Month,
             dateTime.Day,
             0, 0, 0, 0);
        }
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(
             dateTime.Year,
             dateTime.Month,
             dateTime.Day,
             23, 59, 59, 999);
        }
    }
}
