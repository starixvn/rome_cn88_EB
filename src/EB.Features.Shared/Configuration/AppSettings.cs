using System;
using System.Collections.Specialized;
using System.Configuration;

namespace EB.Features.Shared.Configuration
{
    public class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string StoredProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static bool IsTest => GetConfigValue("LocalTest") == "1";

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify() => Require(StoredProcedurePath, nameof(StoredProcedurePath));

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }
    }
}
