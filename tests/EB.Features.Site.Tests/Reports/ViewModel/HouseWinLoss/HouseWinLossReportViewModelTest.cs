using System;
using System.Collections.Generic;
using AutoFixture;
using EB.Features.Reports;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Extensions;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using EB.Features.Site.Shared.Models;
using EB.Features.Site.Shared.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.ViewModel.HouseWinLoss
{
    public class HouseWinLossReportViewModelTest
    {
        private DateTime minDate = new DateTime(2019, 1, 1);
        private UserSession currentSession;
        private HouseWinLossQuery query;
        private Fixture fixture;


        [SetUp]
        public void Setup()
        {
            currentSession = Substitute.For<UserSession>();
            query = new HouseWinLossQuery(currentSession);
            fixture = new Fixture();
            SystemTime.Today = DateTime.Now;
            SystemTime.Yesterday = SystemTime.Today.AddDays(-1);
        }

        //[Test]
        //public void GetDownlineLink_Always_CorrectFormat()
        //{
        //    // Arrange
        //    var expectedFormat = "~/Reports/Index?DisplayMode=1&IsSetMenu=True";

        //    // Act
        //    var act = HouseWinLossReportViewModel.GetDownlineLink();

        //    // Assert
        //    Assert.AreEqual(expectedFormat, act);
        //}


        [Test]
        public void NoteMaxDateDescription_Always_CorrectFormat()
        {
            // Arrange
            SystemTime.Yesterday = DateTime.Now;
            var expectedFormat = string.Format(ReportResources.NoteViewCurrentReport, SystemTime.Yesterday.ToShortDate());

            // Act
            var act = HouseWinLossReportViewModel.NoteMaxDateDescription;

            // Assert
            Assert.AreEqual(expectedFormat, act);
        }


        [Test]
        public void DatePickerViewModel_Always_CorrectMinDate()
        {
            // Arrange
            var expectedMinDate = minDate;
            var viewModel = new HouseWinLossReportViewModel(
                fixture.Create<HouseWinLossViewModel>(),
                query,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(expectedMinDate, act.MinDate);
        }

        [Test]
        public void DatePickerViewModel_Always_CorrectMaxDate()
        {
            // Arrange
            var expectedMaxDate = SystemTime.Today;
            var viewModel = new HouseWinLossReportViewModel(
                fixture.Create<HouseWinLossViewModel>(),
                query,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(expectedMaxDate, act.MaxDate);
        }

        [Test]
        public void DatePickerViewModel_Always_CorrectCurrentDate()
        {
            // Arrange
            var expectedCurrentDate = new DateTime(2019, 1, 2);
            query.Date = expectedCurrentDate;
            var viewModel = new HouseWinLossReportViewModel(
                fixture.Create<HouseWinLossViewModel>(),
                query,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(expectedCurrentDate, act.CurrentDate);
        }

        [Test]
        public void DateRangePickerViewModel_Always_CorrectMinDate()
        {
            // Arrange
            var expectedMinDate = minDate;
            var viewModel = new HouseWinLossReportViewModel(
                fixture.Create<HouseWinLossViewModel>(),
                query,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(expectedMinDate, act.MinDate);
        }

        [Test]
        public void DateRangePickerViewModel_MaxDate_IsYesterday()
        {
            // Arrange
            var yesterday = SystemTime.Yesterday;
            var viewModel = new HouseWinLossReportViewModel(
                fixture.Create<HouseWinLossViewModel>(),
                query,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(yesterday, act.MaxDate);
        }

        [Test]
        public void DateRangePickerViewModel_Default_FromDateIsFirstDateOfMonth()
        {
            // Arrange
            var firstDateOfMonth = SystemTime.Yesterday.FirstDateOfMonth();
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(firstDateOfMonth, act.FromDate);
        }

        [Test]
        public void DateRangePickerViewModel_SecondLoading_FromDateIsNotChange()
        {
            // Arrange
            var fromDate = new DateTime(2019, 1, 2);
            query.FromDate = fromDate;
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(fromDate, act.FromDate);
        }

        [Test]
        public void DateRangePickerViewModel_InvalidFromDate_FromDateIsFirstDateOfMonth()
        {
            // Arrange
            query.FromDate = DateTime.MaxValue;
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(SystemTime.Yesterday.FirstDateOfMonth(), act.FromDate);
        }

        [Test]
        public void DateRangePickerViewModel_Default_ToDateIsYesterday()
        {
            // Arrange
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(SystemTime.Yesterday, act.ToDate);
        }

        [Test]
        public void DateRangePickerViewModel_SecondLoading_ToDateIsNotChange()
        {
            // Arrange
            var toDate = new DateTime(2019, 1, 2);
            query.ToDate = toDate;
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(toDate, act.ToDate);
        }

        [Test]
        public void DateRangePickerViewModel_InvalidToDate_ToDateIsYesterday()
        {
            // Arrange
            query.ToDate = DateTime.MaxValue;
            var viewModel = new HouseWinLossReportViewModel(
               fixture.Create<HouseWinLossViewModel>(),
               query,
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               new List<SelectListItemExt>(),
               "EN-US");

            // Act
            var act = viewModel.ReportFilterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.AreEqual(SystemTime.Yesterday, act.ToDate);
        }
    }
}
