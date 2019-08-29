namespace EB.Features.Site.Tests.Login.Requests
{
    using EB.Features.Site.Login.Requests;
    using NUnit.Framework;

    [TestFixture]
    public class SSOQueryTests
    {
        [Test]
        public void Properties_Always_ReturnsCorrectValues()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            // Act & Assert
            Assert.IsTrue("GbxI81VVtCZCnZCORy414g==" == message.UserId &&
                "A9Ut41LUDzQ=" == message.OnUserId &&
                "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx" == message.Time &&
                "~/SubAccount/Index" == message.RedirectUrl &&
                "5jkkuqkbs0mrz3ed0xy2qji1" == message.SessionId &&
                105 == message.SiteId &&
                "en-US" == message.Language);
        }
    }
}
