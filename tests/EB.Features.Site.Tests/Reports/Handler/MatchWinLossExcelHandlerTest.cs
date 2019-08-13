using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AutoFixture;
using EB.Features.Reports;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Configuration;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace EB.Features.Site.Tests.Reports.Handler
{
    public class MatchWinLossExcelHandlerTest
    {
        private IMatchWinLossService matchWinLossService;
        private IMatchWinLossWorkbook workbook;
        private ICacheRefService cacheRefService;
        private MatchWinLossExcelHandler handler;
        private UserSession currentSession;
        private Fixture fixture;
        private Workbook<MatchWinLossModel> onebookWorkbook;
        private ISportGroupService sportGroupService;

        [SetUp]
        public void Setup()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            matchWinLossService = Substitute.For<IMatchWinLossService>();
            workbook = Substitute.For<IMatchWinLossWorkbook>();
            cacheRefService = Substitute.For<ICacheRefService>();
            currentSession = Substitute.For<UserSession>();
            handler = new MatchWinLossExcelHandler(matchWinLossService, workbook, cacheRefService, sportGroupService);
            fixture = new Fixture();
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;
            onebookWorkbook = Substitute.For<Workbook<MatchWinLossModel>>();
        }


        [Test]
        public void Handle_Always_CallGetOfMatchWinLossService()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            matchWinLossService.Received().Get(Arg.Any<MatchWinLossParam>());
        }


        [Test]
        public void Handle_Always_CallSetDataOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetData(Arg.Any<MatchWinLossViewModel>());
        }


        [Test]
        public void Handle_Always_CallSetRowMappersOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
        }


        [Test]
        public void Handle_Always_CallUseTemplateOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }


        [Test]
        public void Handle_Always_CallSheetOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is<string>(i => i == "ReportData"));
        }

        [Test]
        public void Handle_Always_CallTitleOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Title(Arg.Any<string>());
        }

        [Test]
        public void Handle_Always_CallHeaderOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Test]
        public void Handle_Always_CallFooterOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Footer(Arg.Any<MatchWinLossModel>(), Arg.Any<Mapper<MatchWinLossModel>>());
        }

        [Test]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(Arg.Any<MatchWinLossModel>(), Arg.Any<Mapper<MatchWinLossModel>>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }


        [Test]
        public void Handle_Number_MustBeDivideReduceNumberInAppSettingWhenExport()
        {
            // Arrange            
            var match = new MatchWinLossModel { Wagers = 1, Stake = 2, ValidStake = 3, MemberIncome = 4, HouseIncome = 5, HeadlineStock = 6 };
            var matches = new List<MatchWinLossModel>
            {
                match
            };
            var viewModel = new MatchWinLossViewModel(matches);
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);
            var appSettingValues = new NameValueCollection
            {
                { "ReduceNumber", "100" }
            };
            AppSettings.AppSettingValues = appSettingValues;

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[C](match) == match.Stake / AppSettings.ReduceNumber), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[D](match) == match.ValidStake / AppSettings.ReduceNumber), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[E](match) == match.MemberIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[F](match) == match.HouseIncome / AppSettings.ReduceNumber), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[G](match) == match.HeadlineStock / AppSettings.ReduceNumber), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (decimal)i.Mappings[H](match) == 100m), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
        }


        [Test]
        public void Handle_WagerColumn_MustNotDevide100WhenExport()
        {
            // Arrange            
            var match = new MatchWinLossModel { Wagers = 1, Stake = 2, ValidStake = 3, MemberIncome = 4, HouseIncome = 5, HeadlineStock = 6 };
            var matches = new List<MatchWinLossModel>
            {
                match
            };
            var viewModel = new MatchWinLossViewModel(matches);
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (int)i.Mappings[B](match) == 1), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
        }


        [Test]
        public void Handle_NormalRow_ColumnAIsBetTypeGroupName()
        {
            // Arrange           
            var match = new MatchWinLossModel { BetTypeGroupName = "bettype", Wagers = 1, Stake = 2, ValidStake = 3, MemberIncome = 4, HouseIncome = 5, HeadlineStock = 6 };
            var matches = new List<MatchWinLossModel>
            {
                match
            };
            var viewModel = new MatchWinLossViewModel(matches);
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<MatchWinLossModel>>(i => (string)i.Mappings[A](match) == "bettype"), Arg.Any<Mapper<MatchWinLossModel>>(), Arg.Any<Mapper<MatchWinLossModel>>());
        }

        [Test]
        public void Handle_SubTotalRow_ColumnAIsSubTotalText()
        {
            // Arrange           
            var match = new MatchWinLossModel();
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>()
                , Arg.Is<Mapper<MatchWinLossModel>>(i => (string)i.Mappings[A](match) == ReportResources.SubTotal)
                , Arg.Any<Mapper<MatchWinLossModel>>());
        }

        [Test]
        public void Handle_GrandTotalRow_ColumnAIsGrandTotalText()
        {
            // Arrange           
            var match = new MatchWinLossModel();
            var viewModel = fixture.Create<MatchWinLossViewModel>();
            matchWinLossService.Get(Arg.Any<MatchWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<MatchWinLossViewModel>()).Returns(workbook);

            // Act
            handler.Handle(new MatchWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<MatchWinLossModel>>()
                , Arg.Any<Mapper<MatchWinLossModel>>()
                , Arg.Is<Mapper<MatchWinLossModel>>(i => (string)i.Mappings[A](match) == ReportResources.GrandTotal));
        }

        [Test]
        public void GetReportTitle_TitleOfReport_CorrectFormat()
        {
            // Arrange
            var query = new MatchWinLossExcelQuery(currentSession)
            {
                Date = new DateTime(2019, 1, 1)
            };
            var expectedTitle = $"{ReportResources.MatchWinLossReport} {ReportResources.Of} {query.Date.ToShortDate()} - {AppResources.AllSports} - {AppResources.AllBetTypes} - {AppResources.AllMarkets}";

            // Act
            var act = handler.GetReportTitle(query);

            // Assert
            Assert.AreEqual(expectedTitle, act);
        }


        [Test]
        public void GetExcelFileName_ExcelFileName_CorrectFormat()
        {
            // Arrange
            var query = new MatchWinLossExcelQuery(currentSession)
            {
                Date = new DateTime(2019, 1, 1)
            };
            var expectedFileName = $"Match Win Loss - {query.Date.ToShortDate()}";

            // Act
            var act = handler.GetExcelFileName(query);

            // Assert
            Assert.AreEqual(expectedFileName, act);
        }
    }
}
