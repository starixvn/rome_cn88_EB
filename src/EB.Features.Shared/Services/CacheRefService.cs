namespace EB.Features.Shared.Services
{
    using System;
    using System.Threading;
    using EB.Features.Shared.Constants;
    using Fanex.Caching;
    using Fanex.RemotingRef;

    public interface ICacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetSportNameByEnglish(int sportTypeId);

        string GetBetTypeName(int betTypeId);

        string GetLeagueName(int leagueId, string languageCode);

        string GetLeagueName(int leagueId);

        string GetTeamName(int teamId);
    }

    public class CacheRefServiceImpl : ICacheRefService
    {
        private const string BetTypeNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.BetTypeName";
        private const string LeagueNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.LeagueName";
        private const string TeamNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.TeamName";
        private const int CachedDay = 1;

        private readonly IRemotingRefService remotingRefService;
        private readonly ICacheService fanexCacheService;

        public CacheRefServiceImpl(IRemotingRefService remotingRefService, ICacheService fanexCacheService)
        {
            this.remotingRefService = remotingRefService;
            this.fanexCacheService = fanexCacheService;
        }

        public string GetBetTypeName(int betTypeId)
            => fanexCacheService.GetOrSet(
                $"{BetTypeNameCacheKey}.{LanguageCode}.{betTypeId}",
                () => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CachedDay)));

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

        public string GetSportNameByEnglish(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageConstants.English);

        public string GetLeagueName(int leagueId, string languageCode)
            => fanexCacheService.GetOrSet(
                $"{LeagueNameCacheKey}.{LanguageCode}.{leagueId}{languageCode}",
                () => remotingRefService.GetLeagueName(leagueId.ToString(), languageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CachedDay)));

        public string GetLeagueName(int leagueId)
            => GetLeagueName(leagueId, LanguageCode);

        public string GetTeamName(int teamId)
            => fanexCacheService.GetOrSet(
                $"{TeamNameCacheKey}.{LanguageCode}.{teamId}",
                () => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CachedDay)));

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
