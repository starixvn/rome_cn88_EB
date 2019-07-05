namespace EB.Features.Site.App_Start
{
    using System.Collections.Generic;
    using System.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.RabbitMQ;
    using Fanex.Logging.Sentry.RavenClient;

    public static class LoggingManager
    {
        public static void Init()
        {
            var logEngines = new List<ILogEngine>
            {
                new RabbitMQLogging(ConfigurationManager.AppSettings["Fanex.Logging:RabbitMQConnectionStrings"])
            };

            var sentryUrl = ConfigurationManager.AppSettings["Fanex.Logging:SentryUrl"];
            if (!string.IsNullOrWhiteSpace(sentryUrl))
            {
                logEngines.Add(new SentryLogging(
                    new SentryEngineOptions { Dsn = new SharpRaven.Dsn(sentryUrl) },
                    $"~/assets/common/bundles/raven.min.js"));
            }

            LogManager
                    .SetDefaultLogCategory(ConfigurationManager.AppSettings["Fanex.Logging:DefaultCategory"])
                    .Use(logEngines);

            ExceptionHandler
                .UseConfig(new LoggingConfiguration()
                .GetHttpHeaderInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo));
        }
    }
}