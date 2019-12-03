using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AutoFixture;
using EB.Features.Reports;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Resources;
using EB.Features.Reports.Services;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Configuration;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace EB.Features.Site.Tests.Reports.Handler
{
    public class HouseWinLossExcelHandlerTest
    {
        private IHouseWinLossService houseWinLossService;
        private IHouseWinLossWorkbook workbook;
        private ICacheRefService cacheRefService;
        private HouseWinLossExcelHandler handler;
        private UserSession currentSession;
        private Fixture fixture;
        private Workbook<HouseWinLossModel> onebookWorkbook;
        private ISportGroupService sportGroupService;

        [SetUp]
        public void Setup()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            houseWinLossService = Substitute.For<IHouseWinLossService>();
            workbook = Substitute.For<IHouseWinLossWorkbook>();
            cacheRefService = Substitute.For<ICacheRefService>();
            currentSession = Substitute.For<UserSession>();
            handler = new HouseWinLossExcelHandler(houseWinLossService, workbook, cacheRefService, sportGroupService);
            fixture = new Fixture();
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;
            onebookWorkbook = Substitute.For<Workbook<HouseWinLossModel>>();
        }


        [Test]
        public void Handle_Always_CallGetOfMatchWinLossService()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            houseWinLossService.Received().Get(Arg.Any<HouseWinLossParam>());
        }


        [Test]
        public void Handle_Always_CallSetDataOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>());
        }


        [Test]
        public void Handle_Always_CallSetRowMappersOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }


        [Test]
        public void Handle_Always_CallUseTemplateOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }


        [Test]
        public void Handle_Always_CallSheetOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is<string>(i => i == "ReportData"));
        }

        [Test]
        public void Handle_Always_CallTitleOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Title(Arg.Any<string>());
        }

        [Test]
        public void Handle_Always_CallHeaderOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Test]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(Arg.Any<HouseWinLossModel>(), Arg.Any<Mapper<HouseWinLossModel>>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Test]
        public void Handle_Number_MustBeDivide100WhenExport()
        {
            // Arrange            
            var match = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                SuperComm = 5,
                HouseWinLoss = 6,
                HouseComm = 7,
                HouseIncome = 8,
                HeadlineStock = 9
            };
            var matches = new List<HouseWinLossModel>
            {
                match
            };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "100" }
            };
            AppSettings.AppSettingValues = appSettingValues;

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[C](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.MemberWinLoss / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.MemberComm / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[G](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[H](match) == match.SuperComm / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[I](match) == match.HouseWinLoss / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[J](match) == match.HouseComm / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[K](match) == match.HouseIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[L](match) == match.HouseTurnover / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[M](match) == match.HeadlineResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[N](match) == match.RebateRoundDown / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[O](match) == match.HeadlineResultNet / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[P](match) == match.HeadlineStock / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[Q](match) == 100m), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }


        [Test]
        public void Handle_WagerColumn_MustNotDevide100WhenExport()
        {
            // Arrange            
            var match = new HouseWinLossModel { Wagers = 1, Stake = 2, ValidStake = 3, MemberIncome = 4, HouseIncome = 5, HeadlineStock = 6 };
            var matches = new List<HouseWinLossModel>
            {
                match
            };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[B](match) == 1), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }


        [Test]
        public void Handle_NormalRow_ColumnAIsBetTypeGroupName()
        {
            // Arrange           
            var match = new HouseWinLossModel { UserName = "username", Wagers = 1, Stake = 2, ValidStake = 3, MemberIncome = 4, HouseIncome = 5, HeadlineStock = 6 };
            var matches = new List<HouseWinLossModel>
            {
                match
            };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (string)i.Mappings[A](match) == "username")
                , Arg.Any<Mapper<HouseWinLossModel>>()
                , Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_GrandTotalRow_ColumnAIEmpty()
        {
            // Arrange           
            var match = new HouseWinLossModel();
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>()
                , Arg.Is<Mapper<HouseWinLossModel>>(i => (string)i.Mappings[A](match) == string.Empty)
                , Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void GetReportTitle_IsHistoryMode_CorrectFormat()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession)
            {
                Date = new DateTime(2019, 1, 1),
                IsHistoricReport = true
            };
            var countUserModel = new CountHouseWinLossModel
            {
                AgentCount = 10,
                HeadlineCount = 11,
                MasterCount = 12,
                MemberCount = 13,
                SuperCount = 14
            };

            var countUsers = new CountHouseWinLossViewModel(countUserModel,UserLevel.Headline);

            var expectedTitle = $"{ReportResources.HouseWinLossReport} {query.FromDate.ToShortDate()} --> {query.ToDate.ToShortDate()}";
            expectedTitle += $" - {AppResources.Settled} - Headline - {AppResources.AllSports} - {AppResources.AllBetTypes} - {AppResources.AllMarkets} {Environment.NewLine}";
            expectedTitle += $"{ ReportResources.Headline}: { countUserModel.HeadlineCount} | { ReportResources.SMA}: { countUserModel.SuperCount}";
            expectedTitle += $" | { ReportResources.MA}: { countUserModel.MasterCount} | { ReportResources.Agent}: { countUserModel.AgentCount}";
            expectedTitle += $" | { AppResources.Member}: { countUserModel.MemberCount}";

            // Act
            var act = handler.GetReportTitle(query, countUsers);

            // Assert
            Assert.AreEqual(expectedTitle, act);
        }

        [Test]
        public void GetExcelFileName_IsHistoryMode_CorrectFormat()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession)
            {
                Date = new DateTime(2019, 1, 1),
                IsHistoricReport = true
            };
            var expectedFileName = $"Wager Report - {query.FromDate.ToShortDate()}-{query.ToDate.ToShortDate()} - Headline";

            // Act
            var act = handler.GetExcelFileName(query);

            // Assert
            Assert.AreEqual(expectedFileName, act);
        }

        [Test]
        public void GetExcelFileName_IsCurrentMode_CorrectFormat()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession)
            {
                Date = new DateTime(2019, 1, 1),
                IsHistoricReport = false
            };
            var expectedFileName = $"Wager Report - {query.Date.ToShortDate()} - Headline";

            // Act
            var act = handler.GetExcelFileName(query);

            // Assert
            Assert.AreEqual(expectedFileName, act);
        }

        [Test]
        public void Handle_Super_MapCorrectColumn()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession) { RoleId = UserLevel.Super.Value };
            var match = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                Agent = 5,
                AgentResult = 6,
                MasterResult = 7,
                Super = 8,
                SuperResult = 9,
                HeadlineIncome = 10,
                HeadlineResult = 11,
                HeadlineStock = 12
            };
            var matches = new List<HouseWinLossModel> { match };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "100" }
            };
            AppSettings.AppSettingValues = appSettingValues;

            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[C](match) == match.Wagers), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[G](match) == match.Agent / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[H](match) == match.AgentResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[I](match) == match.MasterResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[J](match) == match.Super / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[K](match) == match.SuperResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[L](match) == match.HeadlineIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[M](match) == match.HeadlineResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[N](match) == match.HeadlineStock / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[O](match) == 100m), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_Master_MapCorrectColumn()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession) { RoleId = UserLevel.Master.Value };
            var match = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                Agent = 5,
                AgentResult = 6,
                MasterResult = 7,
                Super = 8,
                SuperResult = 9,
                HeadlineIncome = 10,
                HeadlineResult = 11,
                HeadlineStock = 12,
                SuperStock = 13,
            };
            var matches = new List<HouseWinLossModel> { match };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "100" }
            };
            AppSettings.AppSettingValues = appSettingValues;

            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[B](match) == match.Wagers), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[C](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.Agent / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[G](match) == match.AgentResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[H](match) == match.MasterResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[I](match) == match.Super / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[J](match) == match.SuperResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[K](match) == match.SuperStock / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_Agent_MapCorrectColumn()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession) { RoleId = UserLevel.Agent.Value };
            var match = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                Agent = 5,
                AgentResult = 6,
                MasterResult = 7,
                Super = 8,
                SuperResult = 9,
                HeadlineIncome = 10,
                HeadlineResult = 11,
                HeadlineStock = 12,
                AgentStock = 13
            };
            var matches = new List<HouseWinLossModel> { match };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "1" }
            };
            AppSettings.AppSettingValues = appSettingValues;
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[B](match) == match.Wagers), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[C](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.Agent / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[G](match) == match.AgentResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[H](match) == match.MasterResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[I](match) == match.AgentStock / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_Member_MapCorrectColumn()
        {
            // Arrange
            var query = new HouseWinLossExcelQuery(currentSession) { RoleId = UserLevel.Member.Value };
            var match = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                Agent = 5,
                AgentResult = 6,
                MasterResult = 7,
                Super = 8,
                SuperResult = 9,
                HeadlineIncome = 10,
                HeadlineResult = 11,
                HeadlineStock = 12
            };
            var matches = new List<HouseWinLossModel> { match };
            var viewModel = new HouseWinLossViewModel(matches);
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            houseWinLossService.CountUsers(Arg.Any<HouseWinLossParam>()).Returns(new CountHouseWinLossModel());
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>(), Arg.Any<bool>())
                .Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "100" }
            };
            AppSettings.AppSettingValues = appSettingValues;

            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[B](match) == match.Wagers), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[C](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.AgentResult / AppSettings.ReduceNumber), Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }
    }
}
