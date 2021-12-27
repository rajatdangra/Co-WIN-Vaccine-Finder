﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VaccineFinder.Enums;

namespace VaccineFinder
{
    public class UserDetails
    {
        public UserDetails(string phone, List<string> emailIds, List<string> pinCodes, int ageCriteria, List<string> beneficiaryIds, int dose, int slotPreference, int pollingTime, bool autoPickCenter, bool includePaidService, string vaccine, string telegramChatID)
        {
            EmailIDs = new List<string>();
            EmailIDs.AddRange(emailIds);
            Phone = phone;
            TelegramChatID = telegramChatID;
            UserPreference = new UserPreference(pinCodes, ageCriteria, beneficiaryIds, dose, slotPreference, pollingTime, autoPickCenter, includePaidService, vaccine);
        }
        public UserPreference UserPreference { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + (string.IsNullOrWhiteSpace(FirstName) ? "" : " ") + LastName; } }
        public List<string> EmailIDs { get; set; }
        public string EmailIdsString { get { return string.Join(", ", EmailIDs); } }
        public string Phone { get; set; }
        public string TelegramChatID { get; set; }

        public bool IsValidEmailIds(List<string> emailList)
        {
            bool isValid = true;
            foreach (var email in emailList)
            {
                if (!IsValidEmail(email))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        public bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }
        public bool IsValidMobileNumber(string phone)
        {
            //for Indian Mobile Number
            Regex regex = new Regex(@"^[6-9]\d{9}$");
            Match match = regex.Match(phone);
            return match.Success;
        }
    }

    public class UserPreference
    {
        public UserPreference(List<string> pinCodes, int ageCriteria, List<string> beneficiaryIds, int dose, int slotPreference, int pollingTime, bool autoPickCenter, bool includePaidService, string vaccine)
        {
            BeneficiaryIds = new List<string>();
            BeneficiaryIds.AddRange(beneficiaryIds);
            PinCodes = new List<string>();
            PinCodes.AddRange(pinCodes);
            AgeCriteria = ageCriteria;
            Dose = dose;
            SlotPreference = slotPreference;
            PollingTime = pollingTime;
            AutoPickCenter = autoPickCenter;
            IncludePaidService = includePaidService;
            Vaccine = vaccine;
        }
        public List<string> BeneficiaryIds { get; set; }
        public string BeneficiaryIdsString { get { return string.Join(", ", BeneficiaryIds); } }
        public List<string> PinCodes { get; set; }
        public string PinCodeString { get { return string.Join(", ", PinCodes); } }
        //public string District { get; set; }
        public int AgeCriteria { get; set; }
        public int Dose { get; set; }
        public int SlotPreference { get; set; }
        public int PollingTime { get; set; }
        public bool AutoPickCenter { get; set; }
        public bool IncludePaidService { get; set; }
        public string Vaccine { get; set; }
        public IEnumerable<VaccineName> vaccines { get { return Enum.GetValues(typeof(VaccineName)).Cast<VaccineName>(); } }
        public bool IsSpecificVaccine { get { return VaccineName.Any.GetDescription().ToUpper() != Vaccine.ToUpper(); } }

        public bool IsValidPinCodes(List<string> pinCodeList)
        {
            bool isValid = true;
            foreach (var pinCode in pinCodeList)
            {
                if (!IsValidPinCode(pinCode))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        // Function to validate the pin code of India.
        public bool IsValidPinCode(String pinCode)
        {
            Regex regex = new Regex(@"^[1-9]{1}[0-9]{2}\s{0,1}[0-9]{3}$");
            Match match = regex.Match(pinCode);
            return match.Success;
        }
        // Function to validate Vaccine.
        public bool IsValidVaccine(String vaccine)
        {
            return vaccines.Select(a => a.GetDescription().ToUpper()).Contains(vaccine.ToUpper());
        }

        public static List<string> GetBeneficiaryIds(string beneficiaryIds)
        {
            return beneficiaryIds.Trim().Replace(" ", "").Split(',').ToList();
        }
        public static List<string> GetPincodes(string pinCodes)
        {
            return pinCodes.Trim().Replace(" ", "").Split(',').ToList();
        }

        public string GetVaccineNames()
        {
            return string.Join(", ", vaccines.Select(a => a.GetDescription()));
        }
        public bool IsPreferenceVaccine(string vaccine)
        {
            return !IsSpecificVaccine ? true : (vaccine.ToUpper().Trim() == Vaccine.ToUpper().Trim());
        }
    }
}
