using System;
using System.Collections;
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
        public void Get_IsHistoryReport_FromDateIsNotChange()
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
        public void Get_IsHistoryReport_ToDateIsNotChange()
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
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
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
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
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
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void Get_SMAIsSettledTicket_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Super
            };


            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void Get_SMATicketStatusIdIsSettled_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.Settled,
                Role = UserLevel.Super
            };

            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void Get_SMAIsUnSettledTicket_CallDynamicRepositoryWithUnsettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.UnSettled,
                Role = UserLevel.Super
            };

            // Act
            reportService.Get(param);

            // Assert
            dynamicRepository.Received().Fetch<HouseWinLossModel>(Arg.Any<Hashtable>());
        }


        [Test]
        public void Get_IsSettled_AlwaysOrderByUserName()
        {
            // Arrange
            var houseWinLoss = new List<HouseWinLossModel>
            {
                new HouseWinLossModel{UserName = "razor"},
                new HouseWinLossModel{UserName = "azor"}
            };
            dynamicRepository.Fetch<HouseWinLossModel>(Arg.Any<Hashtable>())
                .Returns(houseWinLoss);

            // Act
            var act = reportService.Get(new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Headline,
                TicketStatusId = TicketStatusConstants.Settled
            });

            // Assert
            Assert.AreEqual("azor", act.Matches.ToList()[0].UserName);
            Assert.AreEqual("razor", act.Matches.ToList()[1].UserName);
        }

        [Test]
        public void Get_IsUnSettled_AlwaysOrderByUserName()
        {
            // Arrange
            var houseWinLoss = new List<HouseWinLossModel>
            {
                new HouseWinLossModel{UserName = "razor"},
                new HouseWinLossModel{UserName = "azor"}
            };
            dynamicRepository.Fetch<HouseWinLossModel>(Arg.Any<Hashtable>())
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

        ///////

        [Test]
        public void CountUsers_Always_SetMoveBetsDay()
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
            reportService.CountUsers(param);

            // Assert
            Assert.AreEqual(expectedMoveBetsDay, param.MoveBetsDay);
        }


        [Test]
        public void CountUsers_IsHistoryReport_FromDateIsNotChange()
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
            reportService.CountUsers(param);

            // Assert
            Assert.AreEqual(expectedFromDate, param.FromDate);
        }

        [Test]
        public void CountUsers_IsNotHistoryReport_FromDateIsDate()
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
            reportService.CountUsers(param);

            // Assert
            Assert.AreEqual(expectedFromDate, param.FromDate);
        }

        [Test]
        public void CountUsers_IsHistoryReport_ToDateIsNotChange()
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
            reportService.CountUsers(param);

            // Assert
            Assert.AreEqual(expectedToDate, param.ToDate);
        }

        [Test]
        public void CountUsers_IsNotHistoryReport_ToDateIsDate()
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
            reportService.CountUsers(param);

            // Assert
            Assert.AreEqual(expectedToDate, param.ToDate);
        }

        [Test]
        public void CountUsers_IsSettledTicket_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Headline,
                IsCountUser = true
            };

            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void CountUsers_TicketStatusIdIsSettled_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.Settled,
                Role = UserLevel.Headline,
                IsCountUser = true
            };

            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void CountUsers_IsUnSettledTicket_CallDynamicRepositoryWithUnsettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.UnSettled,
                Role = UserLevel.Headline,
                IsCountUser = true
            };

            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }


        [Test]
        public void CountUsers_SMAIsSettledTicket_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = true,
                Role = UserLevel.Super,
                IsCountUser = true
            };


            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void CountUsers_SMATicketStatusIdIsSettled_CallDynamicRepositoryWithSettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.Settled,
                Role = UserLevel.Super,
                IsCountUser = true
            };

            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [Test]
        public void CountUsers_SMAIsUnSettledTicket_CallDynamicRepositoryWithUnsettledHouseWinLossCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam
            {
                IsHistoricReport = false,
                TicketStatusId = TicketStatusConstants.UnSettled,
                Role = UserLevel.Super,
                IsCountUser = true
            };

            // Act
            reportService.CountUsers(param);

            // Assert
            dynamicRepository.Received().Get<CountHouseWinLossModel>(Arg.Any<Hashtable>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Get_Always_SetIsCounterUser_False(bool isCountUser)
        {
            // Arrange
            var param = new HouseWinLossParam { IsCountUser = isCountUser, Role = UserLevel.Super };

            // Act
            reportService.Get(param);

            // Assert
            Assert.IsFalse(param.IsCountUser);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CountUsers_Always_SetIsCounterUser_True(bool isCountUser)
        {
            // Arrange
            var param = new HouseWinLossParam { IsCountUser = isCountUser, Role = UserLevel.Headline };

            // Act
            reportService.CountUsers(param);

            // Assert
            Assert.IsTrue(param.IsCountUser);
        }

        [Test]
        public void GetHeadlineCriteriaByTicketStatus_AbnormalBets_AbnormalBetCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.AbnormalBets };

            // Act
            var act = HouseWinLossService.GetHeadlineCriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(AbnormalWinLossOfHeadlineCriteria), act.GetType());
        }

        [Test]
        public void GetHeadlineCriteriaByTicketStatus_ProblemAccount_ProblemAccountCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.ProblemAccounts };

            // Act
            var act = HouseWinLossService.GetHeadlineCriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(ProblemAccountWinLossOfHeadlineCriteria), act.GetType());
        }

        [Test]
        public void GetHeadlineCriteriaByTicketStatus_Unsettled_UnsettledCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.UnSettled };

            // Act
            var act = HouseWinLossService.GetHeadlineCriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(UnsettledHouseWinLossOfHeadlineCriteria), act.GetType());
        }

        [Test]
        public void GetHeadlineCriteriaByTicketStatus_Settled_SettledCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.Settled };

            // Act
            var act = HouseWinLossService.GetHeadlineCriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(SettledHouseWinLossOfHeadlineCriteria), act.GetType());
        }

        [Test]
        public void GetSMACriteriaByTicketStatus_AbnormalBets_AbnormalBetCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.AbnormalBets };

            // Act
            var act = HouseWinLossService.GetSMACriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(AbnormalWinLossOfSMACriteria), act.GetType());
        }

        [Test]
        public void GetSMACriteriaByTicketStatus_ProblemAccount_ProblemAccountCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.ProblemAccounts };

            // Act
            var act = HouseWinLossService.GetSMACriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(ProblemAccountWinLossOfSMACriteria), act.GetType());
        }

        [Test]
        public void GetSMACriteriaByTicketStatus_Unsettled_UnsettledCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.UnSettled };

            // Act
            var act = HouseWinLossService.GetSMACriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(UnsettledHouseWinLossOfSMACriteria), act.GetType());
        }

        [Test]
        public void GetSMACriteriaByTicketStatus_Settled_SettledCriteria()
        {
            // Arrange
            var param = new HouseWinLossParam { TicketStatusId = TicketStatusConstants.Settled };

            // Act
            var act = HouseWinLossService.GetSMACriteriaByTicketStatus(param);

            // Assert
            Assert.AreEqual(typeof(SettledHouseWinLossOfSMACriteria), act.GetType());
        }

    }
}
