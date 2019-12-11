using System;
using System.Collections.Generic;
using EB.Features.Reports.Constants;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Resources;
using EB.Features.Reports.Services;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace EB.Features.Site.Tests.Reports.Handler.PerformanceBonus
{
    public class PerformanceBonusExcelHandlerTest
    {
        private IPerformanceBonusService service;
        private ISportGroupService sportGroupService;
        private IPerformanceBonusWorkbook workbook;
        private PerformanceBonusExcelHandler handler;
        private UserSession currentSession;
        private Workbook<PerformanceBonusModel> onebookWorkbook;

        [SetUp]
        public void SetUp()
        {
            sportGroupService = Substitute.For<ISportGroupService>();
            service = Substitute.For<IPerformanceBonusService>();
            workbook = Substitute.For<IPerformanceBonusWorkbook>();
            currentSession = Substitute.For<UserSession>();
            onebookWorkbook = Substitute.For<Workbook<PerformanceBonusModel>>();
            handler = new PerformanceBonusExcelHandler(service, workbook, sportGroupService);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;
        }

        [Test]
        public void Handle_Always_CallPerformanceService()
        {
            // Arrange


            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            service.Received().Get(Arg.Any<PerformanceBonusParam>());
        }

        [Test]
        public void Handle_Always_CallSetDataOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                .Returns(new List<PerformanceBonusModel>());

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>());
        }

        [Test]
        public void Handle_Always_CallSetRowMapperOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>());
        }

        [Test]
        public void Handle_Always_CallUseTemplateOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }

        [Test]
        public void Handle_Always_CallSheetOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is<string>(i => i == "ReportData"));
        }

        [Test]
        public void Handle_Always_CallTitleOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                            .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Title(Arg.Any<string>());
        }

        [Test]
        public void Handle_Always_CallHeaderOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                            .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Test]
        public void Handle_Always_CallFooterOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                            .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().Footer(Arg.Any<PerformanceBonusModel>(), Arg.Any<Mapper<PerformanceBonusModel>>());
        }

        [Test]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            // Arrange
            service.Get(Arg.Any<PerformanceBonusParam>())
                            .Returns(new List<PerformanceBonusModel>());
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<PerformanceBonusModel>>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(Arg.Any<PerformanceBonusModel>(), Arg.Any<Mapper<PerformanceBonusModel>>())
                .Returns(onebookWorkbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Test]
        public void Handle_NormalRow_ColumnABIsUserNameAndHeadlineUserName()
        {
            // Arrange           
            var model = new PerformanceBonusModel { UserName = "Razor", HeadlineUserName = "razorhl" };
            var models = new List<PerformanceBonusModel> { model };
            service.Get(Arg.Any<PerformanceBonusParam>()).Returns(models);
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (string)i.Mappings[A](model) == "Razor"),
                Arg.Any<Mapper<PerformanceBonusModel>>());
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (string)i.Mappings[B](model) == "razorhl"),
                Arg.Any<Mapper<PerformanceBonusModel>>());
        }

        [Test]
        public void Handle_GrandRow_ColumnABIsTotalText()
        {
            // Arrange           
            var model = new PerformanceBonusModel { UserName = "Razor", HeadlineUserName = "razorhl" };
            var models = new List<PerformanceBonusModel> { model };
            service.Get(Arg.Any<PerformanceBonusParam>()).Returns(models);
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(
                Arg.Any<Mapper<PerformanceBonusModel>>(),
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (string)i.Mappings[A](model) == "Total"));
            workbook.Received().SetRowMappers(
                Arg.Any<Mapper<PerformanceBonusModel>>(),
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (string)i.Mappings[B](model) == string.Empty));
        }

        [Test]
        public void Handle_Always_MapperCorrectColumn()
        {
            // Arrange           
            var model = new PerformanceBonusModel
            {
                HeadlineStock = 1,
                Dividend = 2,
                Bonus = 3,
                BonusRoundDown = 4,
                UserName = "Razor",
                HeadlineUserName = "razorhl"
            };
            var models = new List<PerformanceBonusModel> { model };
            service.Get(Arg.Any<PerformanceBonusParam>()).Returns(models);
            workbook.SetData(Arg.Any<IEnumerable<PerformanceBonusModel>>())
                .Returns(workbook);

            // Act
            handler.Handle(new PerformanceBonusExcelQuery(currentSession));

            // Assert
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (decimal)i.Mappings[C](model) == model.HeadlineStock),
                Arg.Any<Mapper<PerformanceBonusModel>>());
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (decimal)i.Mappings[D](model) == model.Dividend),
                Arg.Any<Mapper<PerformanceBonusModel>>());
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (decimal)i.Mappings[E](model) == model.Bonus),
                Arg.Any<Mapper<PerformanceBonusModel>>());
            workbook.Received().SetRowMappers(
                Arg.Is<Mapper<PerformanceBonusModel>>(i => (decimal)i.Mappings[F](model) == model.BonusRoundDown),
                Arg.Any<Mapper<PerformanceBonusModel>>());
        }

        [Test]
        public void GetReportTitle_Always_CorrectFormat()
        {
            // Arrange
            var query = new PerformanceBonusExcelQuery(currentSession)
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                UserSelectedCurrencyId = CurrencyConstant.UUS,
                DividendRate = 1
            };
            var currency = $"{ AppResources.Currency }: UUS";
            var bonusRate = $"{ReportResources.DividendRate}: {1}%";
            var date = $"{DateTime.Now.ToShortDate()} --> {DateTime.Now.ToShortDate()}";
            var sportGroup = "Sport: All Sports";
            var expected = $"{ReportResources.PerformanceBonusTitle} - {date} - {bonusRate} - {currency} - {sportGroup}";
            
            // Act
            var act = handler.GetReportTitle(query);

            // Assert
            Assert.AreEqual(expected, act);
        }

        [Test]
        public void GetExcelFileName_Always_CorrectFormat()
        {
            // Arrange
            var query = new PerformanceBonusExcelQuery(currentSession)
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };
            var expected = $"Performance Bonus Calculator - {DateTime.Now.ToShortDate()}-{DateTime.Now.ToShortDate()}";

            // Act
            var act = PerformanceBonusExcelHandler.GetExcelFileName(query);

            // Assert
            Assert.AreEqual(expected, act);
        }
    }
}
