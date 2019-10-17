using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using Fanex.Data.Repository;

namespace EB.Features.Reports.Services
{
    public interface IPerformanceBonusService
    {
        string CheckValidSuper(string userNames);

        IEnumerable<PerformanceBonusModel> Get(PerformanceBonusParam param);
    }

    public class PerformanceBonusService : IPerformanceBonusService
    {
        private readonly decimal SecretNumber = 1.035m;
        private readonly IDynamicRepository dynamiceRepository;

        public PerformanceBonusService(IDynamicRepository dynamiceRepository)
        {
            this.dynamiceRepository = dynamiceRepository;
        }

        public string CheckValidSuper(string userNames)
        {
            var criteria = new CheckValidSuperUserNameCommand(userNames);
            dynamiceRepository.Execute(criteria);

            return criteria.InvalidUsernames;
        }


        public IEnumerable<PerformanceBonusModel> Get(PerformanceBonusParam param)
        {
            var reports = dynamiceRepository.Fetch<PerformanceBonusModel>(new PerformanceBonusCriteria(param))
            .OrderBy(i => i.HeadlineUserName)
            .ThenBy(i => i.UserName);

            return reports.Select(i =>
            {
                i.BonusRate = param.BonusRate;
                i.PerformanceBonus = i.HeadlineStock * i.BonusRate / 100;
                i.PerformanceBonus1035 = i.PerformanceBonus * SecretNumber;
                i.RoundDown = Math.Floor((i.PerformanceBonus1035 / 100)) * 100;
                return i;
            });
        }
    }
}
