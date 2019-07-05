using System;
using System.Collections.Generic;
using AutoFixture;
using EB.Features.Reports;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request.HouseWinLoss;
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
        [SetUp]
        public void Setup()
        {
            houseWinLossService = Substitute.For<IHouseWinLossService>();
            workbook = Substitute.For<IHouseWinLossWorkbook>();
            cacheRefService = Substitute.For<ICacheRefService>();
            currentSession = Substitute.For<UserSession>();
            handler = new HouseWinLossExcelHandler(houseWinLossService, workbook, cacheRefService);
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

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>());
        }


        [Test]
        public void Handle_Always_CallSetRowMappersOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }


        [Test]
        public void Handle_Always_CallUseTemplateOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
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
        public void Handle_Always_CallFooterOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Is<string>(i => i == "ReportData")).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Footer(Arg.Any<HouseWinLossModel>(), Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            // Arrange
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>(), Arg.Any<Mapper<HouseWinLossModel>>())
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[C](match) == 0.02m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[D](match) == 0.03m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[E](match) == 0.04m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[F](match) == 0.05m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[G](match) == 0.06m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[H](match) == 0.07m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[I](match) == 0.08m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[J](match) == 0.09m), Arg.Any<Mapper<HouseWinLossModel>>());
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (decimal)i.Mappings[K](match) == 100m), Arg.Any<Mapper<HouseWinLossModel>>());
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (int)i.Mappings[B](match) == 1), Arg.Any<Mapper<HouseWinLossModel>>());
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
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Is<Mapper<HouseWinLossModel>>(i => (string)i.Mappings[A](match) == "username"), Arg.Any<Mapper<HouseWinLossModel>>());
        }

        [Test]
        public void Handle_GrandTotalRow_ColumnAIEmpty()
        {
            // Arrange           
            var match = new HouseWinLossModel();
            var viewModel = fixture.Create<HouseWinLossViewModel>();
            houseWinLossService.Get(Arg.Any<HouseWinLossParam>()).Returns(viewModel);
            workbook.SetData(Arg.Any<IEnumerable<HouseWinLossModel>>(), Arg.Any<HouseWinLossModel>())
                .Returns(workbook);

            // Act
            handler.Handle(new HouseWinLossExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<HouseWinLossModel>>()
                , Arg.Is<Mapper<HouseWinLossModel>>(i => (string)i.Mappings[A](match) == string.Empty));
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
            var expectedTitle = $"{ReportResources.HouseWinLossReport} {query.FromDate.ToShortDate()} --> {query.ToDate.ToShortDate()}";
            expectedTitle += $" - {AppResources.Settled} - {AppResources.AllSports} - {AppResources.AllBetTypes} - {AppResources.AllMarkets}";

            // Act
            var act = handler.GetReportTitle(query);

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
            var expectedFileName = $"Wager Report - {query.FromDate.ToShortDate()}-{query.ToDate.ToShortDate()}";

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
            var expectedFileName = $"Wager Report - {query.Date.ToShortDate()}";

            // Act
            var act = handler.GetExcelFileName(query);

            // Assert
            Assert.AreEqual(expectedFileName, act);
        }
    }
}
