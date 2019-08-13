using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using EB.Features.Reports.Extension;
using EB.Features.Reports.Model;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Constants;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.ViewModel
{
    public class MatchWinLossViewModelTest
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
        }


        [TestCase(BetTypeIdConstants.Handicap)]
        [TestCase(BetTypeIdConstants.OverUnder)]
        [TestCase(BetTypeIdConstants.CorrectScore)]
        [TestCase(BetTypeIdConstants.OneXTwo)]
        [TestCase(BetTypeIdConstants.Moneyline)]
        [TestCase(BetTypeIdConstants.FirstCorrectScore)]
        [TestCase(BetTypeIdConstants.FirstHandicap)]
        [TestCase(BetTypeIdConstants.FirstMoneyline)]
        [TestCase(BetTypeIdConstants.FirstOneXTwo)]
        [TestCase(BetTypeIdConstants.FirstOverUnder)]

        public void NormalMatchesHasMajorBetType_Always_ContaintMajorBetType(int majorBettype)
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = majorBettype },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.SystemParlay },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Outright },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.SportbookMatchesHasMajorBetType;

            // Assert
            Assert.AreEqual(1, act.Count(i => i.BetTypeGroup == majorBettype));
        }

        [Test]
        public void NormalMatchesHasMajorBetType_Always_OrderById()
        {
            // Arrange            
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.CorrectScore },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Handicap },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.SportbookMatchesHasMajorBetType.ToList();

            // Assert
            Assert.AreEqual(BetTypeIdConstants.Handicap, act[0].BetTypeGroup);
            Assert.AreEqual(BetTypeIdConstants.CorrectScore, act[1].BetTypeGroup);
        }


        [Test]
        public void NormalMatchesHasMinorBettype_Always_DontContainMajorBetType()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Handicap },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Outright },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.SportbookMatchesHasMinorBetType;

            // Assert
            Assert.IsEmpty(act);
        }


        [Test]
        public void NormalMatchesHasOutright_Always_ContainOutrightBettype()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Handicap },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Outright },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.SportbookMatchesHasOutright;

            // Assert
            Assert.IsTrue(act.Any(i => i.BetTypeGroup == BetTypeIdConstants.Outright));
            Assert.AreEqual(1, act.Count());
        }


        [Test]
        public void IsShowTotalRow_NoMatch_ReturnFalse()
        {
            // Arrange
            var viewModel = new MatchWinLossViewModel(new List<MatchWinLossModel>());

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsFalse(act);
        }


        [Test]
        public void IsShowTotalRow_HasParlay_ReturnTrue()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.SystemParlay }
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void IsShowTotalRow_HasNonSportProduct_ReturnTrue()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = 1, IsNonSports = true }
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsTrue(act);
        }

        [TestCase(BetTypeIdConstants.Handicap)]
        [TestCase(BetTypeIdConstants.Outright)]
        public void IsShowTotalRow_HasNormal_ReturnTrue(int normalBetType)
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = normalBetType }
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.IsShowTotalRow;

            // Assert
            Assert.IsTrue(act);
        }


        [Test]
        public void Total_Always_SumInConstructor()
        {
            // Arrange
            var matches = fixture.CreateMany<MatchWinLossModel>();
            var viewModel = new MatchWinLossViewModel(matches);
            var expectedTotal = matches.Sum();

            // Act
            var act = viewModel.Total;

            // Assert
            Assert.AreEqual(expectedTotal.Wagers, act.Wagers);
            Assert.AreEqual(expectedTotal.Stake, act.Stake);
            Assert.AreEqual(expectedTotal.ValidStake, act.ValidStake);
            Assert.AreEqual(expectedTotal.MemberIncome, act.MemberIncome);
            Assert.AreEqual(expectedTotal.HouseIncome, act.HouseIncome);
            Assert.AreEqual(expectedTotal.HeadlineStock, act.HeadlineStock);
        }


        [Test]
        public void NoSportMatches_IsNonSports_AlwaysIsTrue()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Handicap, IsNonSports = true },
                new MatchWinLossModel{ BetTypeGroup = BetTypeIdConstants.Outright },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.NonSportMatches;

            // Assert
            Assert.IsTrue(act.Any(i => i.BetTypeGroup == BetTypeIdConstants.Handicap));
            Assert.AreEqual(1, act.Count());
        }


        public void NoSportMatches_Always_OrderByBetTypeGroupNameByEnglish()
        {
            // Arrange
            var matches = new List<MatchWinLossModel>
            {
                new MatchWinLossModel{ BetTypeGroup = 12, IsNonSports = true },
                new MatchWinLossModel{ BetTypeGroup = 11, IsNonSports = true },
                new MatchWinLossModel{ BetTypeGroup = 13, IsNonSports = true },
            };
            var viewModel = new MatchWinLossViewModel(matches);

            // Act
            var act = viewModel.NonSportMatches.ToList();

            // Assert
            Assert.AreEqual(11, act[0].BetTypeGroup);
            Assert.AreEqual(12, act[1].BetTypeGroup);
            Assert.AreEqual(13, act[2].BetTypeGroup);
        }
    }
}
