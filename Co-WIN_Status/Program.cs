﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Co_WIN_Status
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string stInfo = "Status API Program Started.";
            logger.Info(stInfo);
            Console.WriteLine(stInfo);

            UserDetails userDetails = new UserDetails(AppConfig.Email, AppConfig.PinCode, AppConfig.MinAgeLimit)
            {
                FirstName = AppConfig.FirstName,
                LastName = AppConfig.LastName,
                Phone = AppConfig.Phone,
            };

            Console.WriteLine("Pin Code: " + userDetails.PinCode);
            Console.WriteLine("Email: " + userDetails.Email);
            Console.WriteLine("Minimum Age Limit: " + userDetails.AgeCriteria + "+");
            Console.WriteLine("First Name: " + userDetails.FirstName);
            Console.WriteLine("Last Name: " + userDetails.LastName);
            Console.WriteLine("Phone: " + userDetails.Phone);
            Console.WriteLine("Please verify to Proceed: Y/N");
            
            var confirmation = Console.ReadLine();
            while(confirmation.ToLower() != "n" && confirmation.ToLower() != "y")
            {
                stInfo = "Invalid Input. Please Retry.";
                logger.Error(stInfo + ": " + confirmation);
                Console.WriteLine(stInfo);
                Console.WriteLine("Please verify to Proceed: Y/N");
                confirmation = Console.ReadLine();
            }
            if (confirmation.ToLower() == "n")
            {
                Console.WriteLine("Please Enter your Email: ");
                var Email = Console.ReadLine();
                while(!userDetails.IsValidEmail(Email))
                {
                    stInfo = "Invalid Input. Please Retry.";
                    logger.Error(stInfo + ": " + Email);
                    Console.WriteLine(stInfo);
                    Console.WriteLine("Please Enter your Email: ");
                    Email = Console.ReadLine();
                }
                userDetails.Email = Email;

                Console.WriteLine("Please Enter your PinCode: ");
                var PinCode = Console.ReadLine();
                while (!userDetails.isValidPinCode(PinCode))
                {
                    stInfo = "Invalid Input. Please Retry.";
                    logger.Error(stInfo + ": " + PinCode);
                    Console.WriteLine(stInfo);
                    Console.WriteLine("Please Enter your PinCode: ");
                    PinCode = Console.ReadLine();
                }
                userDetails.PinCode = PinCode;

                Console.WriteLine("Please Enter your MinAgeCriteria: ");
                var MinAgeCriteria = Console.ReadLine();
                int age;
                while(!int.TryParse(MinAgeCriteria, out age))
                {
                    stInfo = "Invalid Input. Please Retry.";
                    logger.Error(stInfo + ": " + MinAgeCriteria);
                    Console.WriteLine(stInfo);
                    Console.WriteLine("Please Enter your MinAgeCriteria: ");
                    MinAgeCriteria = Console.ReadLine();
                }
                userDetails.AgeCriteria = age;

                Console.WriteLine("Please Enter your Phone: ");
                var Phone = Console.ReadLine();
                userDetails.Phone = Phone;

                Console.WriteLine("Please Enter your FirstName: ");
                var FirstName = Console.ReadLine();
                userDetails.FirstName = FirstName;
                Console.WriteLine("Please Enter your LastName: ");
                var LastName = Console.ReadLine();
                userDetails.LastName = LastName;

                if (AppConfig.SaveUserDetails)
                {
                    Console.WriteLine("Updating Default Settings");
                    AppConfig.UpdateConfig(userDetails);
                    Console.WriteLine("Updated Default Settings");
                }
            }
            VaccineFinder vf = new VaccineFinder();
            vf.CheckVaccineAvailabilityStatus(userDetails);
        }
    }
}
