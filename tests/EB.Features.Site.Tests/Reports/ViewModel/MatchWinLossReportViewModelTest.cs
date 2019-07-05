using System;
using System.Collections.Generic;
using AutoFixture;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Shared.Models;
using EB.Features.Site.Shared.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.ViewModel
{
    public class MatchWinLossReportViewModelTest
    {
        private UserSession currentSession;
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            currentSession = Substitute.For<UserSession>();
            fixture = new Fixture();
        }


        [Test]
        public void DatePickerViewModel_MinDate_IsBalanceUpDate()
        {
            // Arrange
            var balanceUpdate = DateTime.Now;
            var viewModel = new MatchWinLossReportViewModel(
                fixture.Create<MatchWinLossViewModel>(),
                new MatchWinLossQuery(currentSession),
                balanceUpdate,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>());

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(balanceUpdate, act.MinDate);
        }

        [Test]
        public void DatePickerViewModel_MaxDate_IsToday()
        {
            // Arrange
            SystemTime.Today = DateTime.Now;
            var viewModel = new MatchWinLossReportViewModel(
                fixture.Create<MatchWinLossViewModel>(),
                new MatchWinLossQuery(currentSession),
                DateTime.Now,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>());

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(SystemTime.Today, act.MaxDate);
        }

        [Test]
        public void DatePickerViewModel_CurrentDate_IsNotChange()
        {
            // Arrange
            var currentDate = DateTime.Now.AddDays(-1);
            var query = new MatchWinLossQuery(currentSession) { Date = currentDate };
            var viewModel = new MatchWinLossReportViewModel(
                fixture.Create<MatchWinLossViewModel>(),
                query,
                DateTime.Now,
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>(),
                new List<SelectListItemExt>());

            // Act
            var act = viewModel.ReportFilterViewModel.DatePickerViewModel;

            // Assert
            Assert.AreEqual(currentDate, act.CurrentDate);
        }
    }
}
