namespace EB.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using EB.Features.Reports.Model;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Resources;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using EB.Features.Reports.Extension;

    public interface IMatchWinLossWorkbook
    {
        Workbook<MatchWinLossModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<MatchWinLossModel> Sheet(string sheetName);

        Workbook<MatchWinLossModel> Title(string titleText);

        Workbook<MatchWinLossModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        IMatchWinLossWorkbook SetData(MatchWinLossViewModel viewModel);

        IMatchWinLossWorkbook SetRowMappers(
            Mapper<MatchWinLossModel> betTypeRowMapper,
            Mapper<MatchWinLossModel> subTotalRowMapper,
            Mapper<MatchWinLossModel> grandTotalRowMapper);

    }

    public class MatchWinLossWorkbookImpl : NpoiWorkbook<MatchWinLossModel>, IMatchWinLossWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal MatchWinLossViewModel ViewModel { get; set; }

        internal Mapper<MatchWinLossModel> BetTypeRowMapper { get; set; }

        internal Mapper<MatchWinLossModel> SubTotalRowMapper { get; set; }

        internal Mapper<MatchWinLossModel> TotalRowMapper { get; set; }

        internal Mapper<MatchWinLossModel> GrandTotalRowMapper { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row SubTotalRowTemplate { get; set; }


        internal Row SystemParlayRowTemplate { get; set; }

        internal Row GrandTotalRowTemplate { get; set; }

        public IMatchWinLossWorkbook SetData(MatchWinLossViewModel viewModel)
        {
            this.ViewModel = viewModel;

            return this;
        }

        public IMatchWinLossWorkbook SetRowMappers(
            Mapper<MatchWinLossModel> betTypeRowMapper,
            Mapper<MatchWinLossModel> subTotalRowMapper,
            Mapper<MatchWinLossModel> grandTotalRowMapper)
        {
            BetTypeRowMapper = betTypeRowMapper;
            SubTotalRowMapper = subTotalRowMapper;
            GrandTotalRowMapper = grandTotalRowMapper;
            return this;
        }

        internal void FillMatchTitle(Sheet sheet, int rowIndex, MatchWinLossModel data)
        {
            var lastRowIndex = rowIndex;
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = (data.LeagueName + (string.IsNullOrEmpty(data.HomeName + data.AwayName) ? string.Empty : $" >> {data.HomeName} V {data.AwayName}"))
                .ToUpper(CultureInfo.InvariantCulture);
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, lastRowIndex, row.LastCellIndex - 1, title);
        }

        internal void FillBetTypeWinLoss(
            Sheet sheet,
            int rowIndex,
            bool isEvenRow,
            MatchWinLossModel betTypeWinLoss)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, betTypeWinLoss, BetTypeRowMapper.Mappings);
        }

        internal void FillMatchTotal(Sheet sheet, int rowIndex, MatchWinLossModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SubTotalRowTemplate);
            FillCells(row, data, SubTotalRowMapper.Mappings);
        }

        internal void FillSystemParlayWinLossTitle(Sheet sheet, int rowIndex)
        {
            var lastRowIndex = rowIndex;
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = AppResources.Parlay.ToUpperInvariant();
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, lastRowIndex, row.LastCellIndex - 1, title);
        }

        internal void FillSystemParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SystemParlayRowTemplate);
            FillCells(row, ViewModel.Parlay, BetTypeRowMapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;
            GrandTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            MatchTitleRowTemplate = sheet.GetRow(currentRowIndex++);
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            SubTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            SystemParlayRowTemplate = sheet.GetRow(currentRowIndex++);

            return GetSheetAt(ReportSheetIndex);
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var rowIndex = TemplateInfo.BodyStartIndex;
            var lastRowIndex = rowIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                lastRowIndex,
                MatchTitleRowTemplate.LastCellIndex - 1,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void FillBody(Sheet sheet)
        {
            RemoveTemplateRows(sheet);

            var hasData = (ViewModel != null && ViewModel.NormalMatches.Any())
                || ViewModel.Parlay != null;

            if (hasData)
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            currentRowIndex = TemplateInfo.BodyStartIndex;

            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(GrandTotalRowTemplate);
            FillCells(row, ViewModel.Total, GrandTotalRowMapper.Mappings);

            foreach (var data in ViewModel.NormalMatches.GroupBy(i => $"{i.LeagueId}_{i.MatchId}"))
            {
                var isEvenRow = false;
                var firstRowOfGroup = data.ToList();
                var matchId = firstRowOfGroup.First().MatchId;
                var leagueId = firstRowOfGroup.First().LeagueId;
                var subTotal = firstRowOfGroup.Sum();

                FillMatchTitle(sheet, currentRowIndex++, firstRowOfGroup.First());

                foreach (var betType in ViewModel.NormalMatchesHasMajorBetType.Where(i => i.MatchId == matchId))
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betType);
                    isEvenRow = !isEvenRow;
                }

                foreach (var betType in ViewModel.NormalMatchesHasMinorBetType.Where(i => i.MatchId == matchId))
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betType);
                    isEvenRow = !isEvenRow;
                }

                foreach (var betType in ViewModel.NormalMatchesHasOutright.Where(i => i.LeagueId == leagueId))
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betType);
                    isEvenRow = !isEvenRow;
                }

                FillMatchTotal(sheet, currentRowIndex++, subTotal);
            }

            if (ViewModel.Parlay != null)
            {
                FillSystemParlayWinLossTitle(sheet, currentRowIndex++);
                FillSystemParlayWinLoss(sheet, currentRowIndex++);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (SpreadSheet.FooterRows.Any())
            {
                var row = sheet.GetRow(currentRowIndex);
                row.CopyCellsFrom(GrandTotalRowTemplate);
                FillCells(row, ViewModel.Total, GrandTotalRowMapper.Mappings);
            }
        }
    }
}
