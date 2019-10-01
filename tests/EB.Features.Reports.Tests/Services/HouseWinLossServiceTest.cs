using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Model;
using EB.Features.Shared.Services;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Services
{
    public class HouseWinLossServiceTest
    {
        private IDynamicRepository dynamicRepository;
        private IReportDateService reportDateService;
        private IHouseWinLossService reportService;

        [SetUp]
        public void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            reportDateService = Substitute.For<IReportDateService>();

            reportService = new HouseWinLossService(dynamicRepository, reportDateService);
        }

        [Test]
        public void Get_Always_SetMoveBetsDay()
        {
            // Arrange
            var expectedMoveBetsDay = DateTime.Now;
            reportDateService.GetMoveBetsDay().Returns(expectedMoveBetsDay);
            var param = new HouseWinLossParam
            {
                MoveBetsDay = DateTime.MinValue,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            Assert.AreEqual(expectedMoveBetsDay, param.MoveBetsDay);
        }


        [Test]
        public void void_IsHistoryReport_FromDateIsNotChange()
        {
            // Arrange
            var expectedFromDate = DateTime.Now;
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                FromDate = expectedFromDate,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            Assert.AreEqual(expectedFromDate, param.FromDate);
        }

        [Test]
        public void Get_IsNotHistoryReport_FromDateIsDate()
        {
            // Arrange
            var expectedFromDate = DateTime.Now;
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                Date = expectedFromDate,
                FromDate = DateTime.MinValue,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            Assert.AreEqual(expectedFromDate, param.FromDate);
        }

        [Test]
        public void void_IsHistoryReport_ToDateIsNotChange()
        {
            // Arrange
            var expectedToDate = DateTime.Now;
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                ToDate = expectedToDate,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            Assert.AreEqual(expectedToDate, param.ToDate);
        }

        [Test]
        public void Get_IsNotHistoryReport_ToDateIsDate()
        {
            // Arrange
            var expectedToDate = DateTime.Now;
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                Date = expectedToDate,
                ToDate = DateTime.MinValue,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            Assert.AreEqual(expectedToDate, param.ToDate);
        }


        [Test]
        public void Get_IsSettledTicket_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Headline
            };


            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<SettledHouseWinLossOfHeadlineCriteria>());
        }

        [Test]
        public void Get_TicketStatusIdIsSettled_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.Settled,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<SettledHouseWinLossOfHeadlineCriteria>());
        }

        [Test]
        public void Get_IsUnSettledTicket_CallDynamicRepositoryWithUnsettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.UnSettled,
                Role = UserLevel.Headline
            };

            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<UnsettledHouseWinLossOfHeadlineCriteria>());
        }


        [Test]
        public void void_IsSettled_AlwaysOrderByUserName()
        {
            // Arrange
            var houseWinLoss = new List<HouseWinLossModel>
            {
                new HouseWinLossModel{UserName = "razor"},
                new HouseWinLossModel{UserName = "azor"}
            };
            dynamicRepository.Fetch<HouseWinLossModel>(Arg.Any<SettledHouseWinLossOfHeadlineCriteria>())
                .Returns(houseWinLoss);

            // Act
            var act = reportService.Get(new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Headline
            });

            // Assert
            Assert.AreEqual("azor", act.Matches.ToList()[0].UserName);
            Assert.AreEqual("razor", act.Matches.ToList()[1].UserName);
        }

        [Test]
        public void void_IsUnSettled_AlwaysOrderByUserName()
        {
            // Arrange
            var houseWinLoss = new List<HouseWinLossModel>
            {
                new HouseWinLossModel{UserName = "razor"},
                new HouseWinLossModel{UserName = "azor"}
            };
            dynamicRepository.Fetch<HouseWinLossModel>(Arg.Any<UnsettledHouseWinLossOfHeadlineCriteria>())
                .Returns(houseWinLoss);

            // Act
            var act = reportService.Get(new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.UnSettled,
                Role = UserLevel.Headline
            });

            // Assert
            Assert.AreEqual("azor", act.Matches.ToList()[0].UserName);
            Assert.AreEqual("razor", act.Matches.ToList()[1].UserName);
        }
    }
}
