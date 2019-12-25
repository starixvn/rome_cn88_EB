using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria.Abnormal
{
    public class AbnormalWinLossOfHeadlineCriteriaTest
    {

        [Test]
        public void ReportType_Always_Return2()
        {
            // Arrange
            var criteria = new AbnormalWinLossOfHeadlineCriteria(new HouseWinLossParam());

            // Act
            var act = criteria.ReportType;

            // Assert
            Assert.AreEqual(2, act);
        }

        [TestCase(true, "CS_EB_Rpt_Cancelled_Headline_CustCount")]
        [TestCase(false, "CS_EB_Rpt_Cancelled_Headline")]
        public void GetSettingKey_Always_ReturnCorrectKey(bool isCountUser, string expectedKey)
        {
            // Arrange
            var criteria = new AbnormalWinLossOfHeadlineCriteria(new HouseWinLossParam { IsCountUser = isCountUser });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedKey, act);
        }


        [TestCase(true, 2)]
        [TestCase(false, 1)]
        public void Constructor_Always_SetReportMode(bool isHistory, int expectedReportMode)
        {
            // Arrange
            var criteria = new AbnormalWinLossOfHeadlineCriteria(new HouseWinLossParam { IsHistoricReport = isHistory });

            // Act
            var act = criteria.ReportMode;

            // Assert
            Assert.AreEqual(expectedReportMode, act);
        }

        [Test]
        public void Constructor_Always_SetMoveBetDay()
        {
            // Arrange
            var expectedMoveBetDay = DateTime.Now;
            var criteria = new AbnormalWinLossOfHeadlineCriteria(new HouseWinLossParam { MoveBetsDay = expectedMoveBetDay });

            // Act
            var act = criteria.MoveBetsDay;

            // Assert
            Assert.AreEqual(expectedMoveBetDay, act);
        }
    }
}
