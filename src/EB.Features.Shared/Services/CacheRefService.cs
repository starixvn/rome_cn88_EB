namespace EB.Features.Shared.Services
{
    using System;
    using System.Threading;
    using Fanex.Caching;
    using Fanex.RemotingRef;

    public interface CacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetBetTypeName(int betTypeId);

        string GetLeagueName(int leagueId, string languageCode);

        string GetLeagueName(int leagueId);

        string GetTeamName(int teamId);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private const string BetTypeNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.BetTypeName";
        private const string LeagueNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.LeagueName";
        private const string TeamNameCacheKey = "EB.Features.Shared.Services.CacheRefServiceImpl.TeamName";

        private readonly IRemotingRefService remotingRefService;

        public CacheRefServiceImpl(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetBetTypeName(int betTypeId)
            => ConfiguredCacheService.GetOrSet(
                $"{BetTypeNameCacheKey}.{LanguageCode}.{betTypeId}",
                () => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

        public string GetLeagueName(int leagueId, string languageCode)
            => ConfiguredCacheService.GetOrSet(
                $"{LeagueNameCacheKey}.{LanguageCode}.{leagueId}{languageCode}",
                () => remotingRefService.GetLeagueName(leagueId.ToString(), languageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        public string GetLeagueName(int leagueId)
            => GetLeagueName(leagueId, LanguageCode);

        public string GetTeamName(int teamId)
            => ConfiguredCacheService.GetOrSet(
                $"{TeamNameCacheKey}.{LanguageCode}.{teamId}",
                () => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
