using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using EB.Features.Reports.Extension;
using EB.Features.Reports.Model;
using EB.Features.Reports.ViewModel;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.ViewModel
{
    public class HouseWinLossViewModelTest
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
        }

        [Test]
        public void Total_Always_SetInConstructor()
        {
            // Arrange
            var matches = fixture.CreateMany<HouseWinLossModel>();
            var expectedTotal = matches.Sum();
            var viewModel = new HouseWinLossViewModel(matches);

            // Act
            var act = viewModel.Total;

            // Assert
            Assert.AreEqual(expectedTotal.Wagers, act.Wagers);
            Assert.AreEqual(expectedTotal.Stake, act.Stake);
            Assert.AreEqual(expectedTotal.ValidStake, act.ValidStake);
            Assert.AreEqual(expectedTotal.MemberIncome, act.MemberIncome);
            Assert.AreEqual(expectedTotal.SuperComm, act.SuperComm);
            Assert.AreEqual(expectedTotal.HouseWinLoss, act.HouseWinLoss);
            Assert.AreEqual(expectedTotal.HouseComm, act.HouseComm);
            Assert.AreEqual(expectedTotal.HouseIncome, act.HouseIncome);
            Assert.AreEqual(expectedTotal.HeadlineStock, act.HeadlineStock);
        }


        [Test]
        public void IsShowTotalRow_HasMatchs_ReturnTrue()
        {
            // Arrange
            var matches = fixture.CreateMany<HouseWinLossModel>();
            var viewModel = new HouseWinLossViewModel(matches);

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void IsShowTotalRow_NoMatchs_ReturnFalse()
        {
            // Arrange
            var viewModel = new HouseWinLossViewModel(new List<HouseWinLossModel>());

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsFalse(act);
        }
    }
}
