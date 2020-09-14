namespace EB.Features.Shared.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class AppSettings
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

        public static string InternalLogInURLHeadline => GetConfigValue("InternalLogInURLHeadline");

        public static string InternalLogInURL => GetConfigValue("InternalLogInURL");

        public static string FeaturesAppPath => GetConfigValue("FeaturesAppPath");

        public static int ReduceNumber => int.Parse(GetConfigValue("ReduceNumber"));

        public static string GetConfigValue(string configName) =>  string.IsNullOrEmpty(AppSettingValues[configName]) ? "": AppSettingValues[configName];

        public static void Verify() => Require(StoredProcedurePath, nameof(StoredProcedurePath));

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }

        public static string MatomoSiteId => GetConfigValue("MatomoSiteId");
    }
}
