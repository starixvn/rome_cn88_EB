using System;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.Request.MatchWinLoss
{
    public class MatchWinLossQueryTest
    {
        private UserSession currentSession;

        [SetUp]
        public void Setup()
        {
            currentSession = Substitute.For<UserSession>();
        }


        [Test]
        public void MatchWinLossQuery_SetDefaultValue_InConstructor()
        {
            // Arrange
            SystemTime.Today = DateTime.Now;
            var query = new MatchWinLossQuery(currentSession);            

            // Act


            // Assert
            Assert.AreEqual(SystemTime.Today, query.Date);
            Assert.AreEqual(LiveStatusConstants.AllMarkets, query.UserSelectedLiveStatusId);
            Assert.IsTrue(query.IsTest);
        }
    }
}
