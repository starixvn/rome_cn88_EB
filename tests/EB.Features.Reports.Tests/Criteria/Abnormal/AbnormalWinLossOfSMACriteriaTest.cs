using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria.Abnormal
{
    public class AbnormalWinLossOfSMACriteriaTest
    {

        [Test]
        public void ReportType_Always_Return2()
        {
            // Arrange
            var criteria = new AbnormalWinLossOfSMACriteria(new HouseWinLossParam());

            // Act
            var act = criteria.ReportType;

            // Assert
            Assert.AreEqual(2, act);
        }

        [TestCase(4, true, "CS_EB_Rpt_Cancelled_Super_CustCount")]
        [TestCase(3, true, "CS_EB_Rpt_Cancelled_Master_CustCount")]
        [TestCase(2, true, "CS_EB_Rpt_Cancelled_Agent_CustCount")]
        [TestCase(1, true, "CS_EB_Rpt_Cancelled_Member_CustCount")]
        [TestCase(4, false, "CS_EB_Rpt_Cancelled_Super")]
        [TestCase(3, false, "CS_EB_Rpt_Cancelled_Master")]
        [TestCase(2, false, "CS_EB_Rpt_Cancelled_Agent")]
        [TestCase(1, false, "CS_EB_Rpt_Cancelled_Member")]
        public void GetSettingKey_Always_ReturnCorrectKey(byte role, bool isCountUser, string expectedKey)
        {
            // Arrange
            var criteria = new AbnormalWinLossOfSMACriteria(new HouseWinLossParam
            {
                Role = Enumeration.FromValue<UserLevel>(role),
                IsCountUser = isCountUser
            });

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
            var criteria = new AbnormalWinLossOfSMACriteria(new HouseWinLossParam { IsHistoricReport = isHistory });

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
            var criteria = new AbnormalWinLossOfSMACriteria(new HouseWinLossParam { MoveBetsDay = expectedMoveBetDay });

            // Act
            var act = criteria.MoveBetsDay;

            // Assert
            Assert.AreEqual(expectedMoveBetDay, act);
        }
    }
}
