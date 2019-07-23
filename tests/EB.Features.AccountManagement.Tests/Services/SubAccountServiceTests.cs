namespace EB.Features.AccountManagement.Tests.Services
{
    using System.Threading.Tasks;
    using EB.Features.AccountManagement.AppResources;
    using EB.Features.AccountManagement.Criteria.SubAccount;
    using EB.Features.AccountManagement.Models.SubAccount;
    using EB.Features.AccountManagement.Params.SubAccount;
    using EB.Features.AccountManagement.Services;
    using EB.Features.Shared.Model;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubAccountServiceTests
    {
        private IDynamicRepository dynamicRepository;
        private SubAccountService subAccountService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            subAccountService = new SubAccountServiceImpl(dynamicRepository);
        }

        [Test]
        public async Task GetSuperSubAccountAsync_ValidUsername_ReturnsSubAccountItem()
        {
            // Arrange
            var expectedResult = new SubAccountItem("stark", "starknn", 1, 2, "starksub01", "starksub01nn", "starix", "[D],[H],[B],[E]");

            dynamicRepository.GetAsync<SubAccountItem>(
                Arg.Is<GetSuperSubAccountCriteria>(criteria => criteria.Username == "starksub01"))
                .Returns(expectedResult);

            // Act
            var actualResult = await subAccountService.GetSuperSubAccountAsync("starksub01");

            // Assert
            Assert.IsTrue(
                expectedResult.SMAUsername == actualResult.SMAUsername &&
                expectedResult.SMANickname == actualResult.SMANickname &&
                expectedResult.CustId == actualResult.CustId &&
                expectedResult.SubAccId == actualResult.SubAccId &&
                expectedResult.UserName == actualResult.UserName &&
                expectedResult.NickName == actualResult.NickName &&
                expectedResult.FirstName == actualResult.FirstName &&
                expectedResult.PageAccess == actualResult.PageAccess &&
                expectedResult.Permissions.ViewReports == actualResult.Permissions.ViewReports &&
                expectedResult.Permissions.ViewTotalBets == actualResult.Permissions.ViewTotalBets &&
                expectedResult.Permissions.ViewMemberInfo == actualResult.Permissions.ViewMemberInfo &&
                expectedResult.Permissions.UpdateAccount == actualResult.Permissions.UpdateAccount);
        }

        [Test]
        public async Task GetSuperSubAccountAsync_InvalidUsername_ReturnsDefaultSubAccountItem()
        {
            // Arrange
            var expectedResult = new SubAccountItem();

            dynamicRepository.GetAsync<SubAccountItem>(
                Arg.Is<GetSuperSubAccountCriteria>(criteria => criteria.Username == "starksub01"))
                .Returns(expectedResult);

            // Act
            var actualResult = await subAccountService.GetSuperSubAccountAsync("starksub01");

            // Assert
            Assert.IsTrue(
                expectedResult.SMAUsername == actualResult.SMAUsername &&
                expectedResult.SMANickname == actualResult.SMANickname &&
                expectedResult.CustId == actualResult.CustId &&
                expectedResult.SubAccId == actualResult.SubAccId &&
                expectedResult.UserName == actualResult.UserName &&
                expectedResult.NickName == actualResult.NickName &&
                expectedResult.FirstName == actualResult.FirstName &&
                expectedResult.PageAccess == actualResult.PageAccess &&
                expectedResult.Permissions.ViewReports == actualResult.Permissions.ViewReports &&
                expectedResult.Permissions.ViewTotalBets == actualResult.Permissions.ViewTotalBets &&
                expectedResult.Permissions.ViewMemberInfo == actualResult.Permissions.ViewMemberInfo &&
                expectedResult.Permissions.UpdateAccount == actualResult.Permissions.UpdateAccount);
        }

        [Test]
        public async Task UpdateSubAccountAsync_InvalidParam_ReturnsInvalidSMASubAccountMessage()
        {
            // Arrange
            var expectedResult = Result.CreateErrorResult(message: SubAccountResource.InvalidSMASubAccount);
            var param = new UpdateSubAccountParam(1, 2, "starksub01", "[D],[H],[B],[E]", "EN", "::1", 3);
            var subAccountItem = new SubAccountItem();

            dynamicRepository.GetAsync<SubAccountItem>(
                Arg.Is<GetSuperSubAccountCriteria>(criteria => criteria.Username == "starksub01"))
                .Returns(subAccountItem);

            // Act
            var actualResult = await subAccountService.UpdateSubAccountAsync(param);

            // Assert
            Assert.IsTrue(
                expectedResult.Code == actualResult.Code &&
                expectedResult.Message == actualResult.Message);
        }

        [Test]
        public async Task UpdateSubAccountAsync_ValidParam_ReturnsInvalidSMASubAccountMessage()
        {
            // Arrange
            var param = new UpdateSubAccountParam(1, 2, "starksub01", "[D],[H],[B],[E]", "EN", "::1", 3);
            var subAccountItem = new SubAccountItem(
                "stark", "starknn", param.CustId, param.SubAccId, param.Username, "starksub01nn", param.Name, "[D],[H],[B],[E]");

            dynamicRepository.GetAsync<SubAccountItem>(
                Arg.Is<GetSuperSubAccountCriteria>(criteria => criteria.Username == "starksub01"))
                .Returns(subAccountItem);

            // Act
            var actualResult = await subAccountService.UpdateSubAccountAsync(param);

            // Assert
            await dynamicRepository.Received().ExecuteAsync(Arg.Is<UpdateSubAccountCommand>(cmd =>
                cmd.CustId == param.CustId
                && cmd.SubAccId == param.SubAccId
                && cmd.Name == param.Name
                && cmd.PageAccess == param.PageAccess
                && cmd.LanguageKey == param.LanguageKey
                && cmd.LoginIP == param.IP
                && cmd.UserId == param.ActorId));
        }
    }
}
