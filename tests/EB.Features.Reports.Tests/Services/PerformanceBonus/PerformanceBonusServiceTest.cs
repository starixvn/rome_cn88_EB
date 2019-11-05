using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Services.PerformanceBonus
{
    class PerformanceBonusServiceTest
    {
        private IDynamicRepository dynamicRepository;
        private IPerformanceBonusService service;
        [SetUp]
        public void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            service = new PerformanceBonusService(dynamicRepository);
        }


        [Test]
        public void CheckValidSuper_Always_CallDynamicRepository()
        {
            // Arrange


            // Act
            service.CheckValidSuper(string.Empty);

            // Assert
            dynamicRepository.Received().Execute(Arg.Any<CheckValidSuperUserNameCommand>());
        }


        [Test]
        public void CheckValidSuper_ReturnEmpty_WhenValidSuper()
        {
            // Arrange
            dynamicRepository.Execute(Arg.Any<CheckValidSuperUserNameCommand>()).Returns(i =>
            {
                ((CheckValidSuperUserNameCommand)i[0]).InvalidUsernames = string.Empty;
                return 1;
            });

            // Act
            var act = service.CheckValidSuper("razor");

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(act));
        }

        [Test]
        public void CheckValidSuper_ReturnErrorMessage_WhenInvalidSuper()
        {
            // Arrange
            dynamicRepository.Execute(Arg.Any<CheckValidSuperUserNameCommand>()).Returns(i =>
            {
                ((CheckValidSuperUserNameCommand)i[0]).InvalidUsernames = "razor";
                return 1;
            });

            // Act
            var act = service.CheckValidSuper("razor,abc");

            // Assert
            Assert.AreEqual("razor", act);
        }

        [Test]
        public void Get_Always_OrderHeadlineUserNameAndUserName()
        {
            // Arrange
            var models = new List<PerformanceBonusModel>
            {
                new PerformanceBonusModel{HeadlineUserName="xyz",UserName="bca"},
                new PerformanceBonusModel{HeadlineUserName="xyz",UserName="abca"},
                new PerformanceBonusModel{HeadlineUserName="abc",UserName="bca"},
            };
            dynamicRepository.Fetch<PerformanceBonusModel>(Arg.Any<PerformanceBonusCriteria>())
                .Returns(models);

            // Act
            var act = service.Get(new PerformanceBonusParam()).ToList();

            // Assert
            Assert.AreEqual("abc", act[0].HeadlineUserName);
            Assert.AreEqual("xyz", act[1].HeadlineUserName);
            Assert.AreEqual("abca", act[1].UserName);
            Assert.AreEqual("bca", act[2].UserName);
        }

        [Test]
        public void Get_Always_SetDividendRate()
        {
            // Arrange
            var models = new List<PerformanceBonusModel>
            {
                new PerformanceBonusModel{HeadlineUserName="xyz",UserName="bca"},
                new PerformanceBonusModel{HeadlineUserName="xyz",UserName="abca"},
                new PerformanceBonusModel{HeadlineUserName="abc",UserName="bca"},
            };
            dynamicRepository.Fetch<PerformanceBonusModel>(Arg.Any<PerformanceBonusCriteria>())
                .Returns(models);

            // Act
            var act = service.Get(new PerformanceBonusParam { DividendRate = 2 }).ToList();


            // Assert
            Assert.AreEqual(2, act[0].DividendRate);
            Assert.AreEqual(2, act[1].DividendRate);
            Assert.AreEqual(2, act[2].DividendRate);
        }

        [Test]
        public void Get_Calcuator_Dividend()
        {
            // Arrange
            var models = new List<PerformanceBonusModel>
            {
                new PerformanceBonusModel{HeadlineStock=200}
            };
            dynamicRepository.Fetch<PerformanceBonusModel>(Arg.Any<PerformanceBonusCriteria>())
                .Returns(models);

            // Act
            var act = service.Get(new PerformanceBonusParam { DividendRate = 10 }).ToList();


            // Assert
            Assert.AreEqual(20, act[0].Dividend);
        }

        [Test]
        public void Get_Calcuator_Bonus()
        {
            // Arrange
            var models = new List<PerformanceBonusModel>
            {
                new PerformanceBonusModel{HeadlineStock=200}
            };
            dynamicRepository.Fetch<PerformanceBonusModel>(Arg.Any<PerformanceBonusCriteria>())
                .Returns(models);

            // Act
            var act = service.Get(new PerformanceBonusParam { DividendRate = 10 }).ToList();


            // Assert
            Assert.AreEqual(20.7, act[0].Bonus);
        }

        [Test]
        public void Get_Calcuator_BonusRoundDown()
        {
            // Arrange
            var models = new List<PerformanceBonusModel>
            {
                new PerformanceBonusModel{HeadlineStock=200}
            };
            dynamicRepository.Fetch<PerformanceBonusModel>(Arg.Any<PerformanceBonusCriteria>())
                .Returns(models);

            // Act
            var act = service.Get(new PerformanceBonusParam { DividendRate = 10 }).ToList();


            // Assert
            Assert.AreEqual(0, act[0].BonusRoundDown);
        }
    }
}
