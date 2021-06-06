﻿using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VaccineFinder.Models
{
    public class VersionChecker
    {
        public VersionChecker()
        {
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool EvaluateCurrentSoftwareVersion()
        {
            bool allowToRunApp = true;
            string stInfo = "EvaluateCurrentSoftwareVersion call Started.";
            logger.Info(stInfo);
            var latestVersionDto = GetLatestVersionDetails();
            // Allow App to Run if there is any issue in fetching the Relases info from Github for Update Checking so that Core functionality doesn't stop
            if (latestVersionDto is null)
            {
                allowToRunApp = true;
            }
            else
            {
                var serverVersion = GetVersionInfoFromServer(latestVersionDto);
                var localVersion = GetCurrentVersionFromSystem();

                if (IsUpdatedVersionAvailable(serverVersion, localVersion))
                {
                    stInfo = "Update is available";
                    logger.Info(stInfo);
                    if (IsVersionUpdateMandatory(serverVersion.Major, localVersion.Major, serverVersion.Minor, localVersion.Minor))
                    {
                        stInfo = $"[FATAL] Your Software Version is outdated. You MUST update the software, your current version is {localVersion}";
                        ConsoleMethods.PrintError(stInfo);
                        logger.Info(stInfo);
                        ShowLatestVersionFeatureInfo(latestVersionDto, serverVersion);
                        Console.WriteLine($"Please press Y for Downloading the Latest Version {serverVersion}, any other key to exit the app");
                        var input = Console.ReadLine();
                        if (input.ToLower() == "y")
                        {
                            DownloadLatestVersion(latestVersionDto);
                            allowToRunApp = false;
                        }
                        else
                        {
                            allowToRunApp = false;
                        }
                    }
                    else
                    {
                        stInfo = $"[INFO] New Version of the Software is Available, your current version is {localVersion}";
                        ConsoleMethods.PrintError(stInfo);
                        logger.Info(stInfo);

                        ShowLatestVersionFeatureInfo(latestVersionDto, serverVersion);
                        Console.WriteLine($"Please press Y for Downloading the Latest Version {serverVersion}, any other key to continue using the current version the app");
                        var input = Console.ReadLine();
                        if (input.ToLower() == "y")
                        {
                            DownloadLatestVersion(latestVersionDto);
                            allowToRunApp = false;
                        }
                        else
                        {
                            allowToRunApp = true;
                        }
                    }
                }
                else
                {
                    stInfo = "You have the latest Update";
                    logger.Info(stInfo);
                    ConsoleMethods.PrintSuccess(stInfo);
                }
            }

            logger.Info("EvaluateCurrentSoftwareVersion call End.");
            return allowToRunApp;
        }

        private static void DownloadLatestVersion(VersionModel latestVersionDto)
        {
            logger.Info("DownloadLatestVersion call Started.");
            Process.Start(new ProcessStartInfo(latestVersionDto.Assets[0].BrowserDownloadUrl.AbsoluteUri) { UseShellExecute = true });
            logger.Info("DownloadLatestVersion call End.");
        }

        private Version GetVersionInfoFromServer(VersionModel latestVersionDto)
        {
            string processedVersion;
            if (latestVersionDto.TagName.Contains("-"))
            {
                processedVersion = latestVersionDto.TagName[1..latestVersionDto.TagName.IndexOf("-")] + ".0";
            }
            else
            {
                processedVersion = latestVersionDto.TagName[1..] + ".0";
            }
            var lastestVersionOnServer = new Version(processedVersion);
            return lastestVersionOnServer;
        }

        private bool IsUpdatedVersionAvailable(Version serverVersion, Version localVersion)
        {
            return serverVersion.CompareTo(localVersion) > 0;
        }

        public Version GetCurrentVersionFromSystem()
        {
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return localVersion;
        }
        private bool IsVersionUpdateMandatory(int serverMajorVersion, int localMajorVersion, int serverMinorVersion, int localMinorVersion)
        {
            if (serverMajorVersion != localMajorVersion)
                return true;

            if (serverMinorVersion != localMinorVersion)
                return true;

            return false;
        }

        private VersionModel GetLatestVersionDetails()
        {
            logger.Info("GetLatestVersionDetails call Started.");
            var versionResponse = APIs.FetchLatestAppVersion();
            logger.Info("GetLatestVersionDetails call End.");
            return versionResponse;
        }

        private void ShowLatestVersionFeatureInfo(VersionModel latestVersionDto, Version serverVersion)
        {
            string stInfo = $"Latest Version of the Software { latestVersionDto.Name} is { serverVersion }, Downloaded #{latestVersionDto.Assets[0].DownloadCount} times, Released on { latestVersionDto.PublishedAt.LocalDateTime} \n\nFeatures of the Updated Version:\n{latestVersionDto.Body}";
            Console.WriteLine($"*************************************************************************************************************************************************************");
            ConsoleMethods.PrintInfo(stInfo);
            logger.Info(stInfo);
            Console.WriteLine($"*************************************************************************************************************************************************************");
        }
    }
}
