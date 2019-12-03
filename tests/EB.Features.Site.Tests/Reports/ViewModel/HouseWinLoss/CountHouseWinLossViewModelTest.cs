using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Model;
using EB.Features.Reports.Resources;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.ViewModel.HouseWinLoss
{
    public class CountHouseWinLossViewModelTest
    {
        private CountHouseWinLossModel model;

        [SetUp]
        public void Setup()
        {
            model = new CountHouseWinLossModel
            {
                AgentCount = 1,
                HeadlineCount = 2,
                MasterCount = 3,
                MemberCount = 4,
                SuperCount = 5
            };
        }

        [Test]
        public void Constructor_Headline_ShowFullUser()
        {
            // Arrange
            var viewModel = new CountHouseWinLossViewModel(model, UserLevel.Headline);

            // Act
            var act = viewModel.Users;

            // Assert
            Assert.AreEqual(5, act.Count());
        }

        [Test]
        public void Constructor_Super_ShowNotShowHeadline()
        {
            // Arrange
            var viewModel = new CountHouseWinLossViewModel(model, UserLevel.Super);

            // Act
            var act = viewModel.Users.ToList();

            // Assert
            Assert.AreEqual(4, act.Count);
            Assert.AreEqual(ReportResources.SMA, act[0].Key);
            Assert.AreEqual(ReportResources.MA, act[1].Key);
            Assert.AreEqual(ReportResources.Agent, act[2].Key);
            Assert.AreEqual(AppResources.Member, act[3].Key);
        }

        [Test]
        public void Constructor_Master_ShowMasterAgentMember()
        {
            // Arrange
            var viewModel = new CountHouseWinLossViewModel(model, UserLevel.Master);

            // Act
            var act = viewModel.Users.ToList();

            // Assert
            Assert.AreEqual(3, act.Count);
            Assert.AreEqual(ReportResources.MA, act[0].Key);
            Assert.AreEqual(ReportResources.Agent, act[1].Key);
            Assert.AreEqual(AppResources.Member, act[2].Key);
        }

        [Test]
        public void Constructor_Agent_ShowAgentMember()
        {
            // Arrange
            var viewModel = new CountHouseWinLossViewModel(model, UserLevel.Agent);

            // Act
            var act = viewModel.Users.ToList();

            // Assert
            Assert.AreEqual(2, act.Count);
            Assert.AreEqual(ReportResources.Agent, act[0].Key);
            Assert.AreEqual(AppResources.Member, act[1].Key);
        }

        [Test]
        public void Constructor_Member_ShowMember()
        {
            // Arrange
            var viewModel = new CountHouseWinLossViewModel(model, UserLevel.Member);

            // Act
            var act = viewModel.Users.ToList();

            // Assert
            Assert.AreEqual(1, act.Count);
            Assert.AreEqual(AppResources.Member, act[0].Key);
        }
    }
}
