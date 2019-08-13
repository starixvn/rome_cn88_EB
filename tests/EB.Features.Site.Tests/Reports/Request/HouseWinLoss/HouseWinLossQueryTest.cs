using System;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.Request.HouseWinLoss
{
    public class HouseWinLossQueryTest
    {
        private UserSession currentSession;
        [SetUp]
        public void Setup()
        {
            currentSession = Substitute.For<UserSession>();
        }

        [Test]
        public void HouseWinLossQuery_SetDefaultValue_InConstructor()
        {
            // Arrange
            SystemTime.Today = DateTime.Now;
            var query = new HouseWinLossQuery(currentSession);

            // Act


            // Assert
            Assert.AreEqual(LiveStatusConstants.AllMarkets, query.UserSelectedLiveStatusId);
            Assert.AreEqual(SystemTime.Today, query.Date);
            Assert.AreEqual(SystemTime.Today, query.FromDate);
            Assert.AreEqual(SystemTime.Today, query.ToDate);
            Assert.IsTrue(query.IsTest);
        }

    }
}
