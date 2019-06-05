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

    public interface MatchWinLossWorkbook
    {
        Workbook<MatchWinLossModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<MatchWinLossModel> Sheet(string sheetName);

        Workbook<MatchWinLossModel> Title(string titleText);

        Workbook<MatchWinLossModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        MatchWinLossWorkbook SetData(
            IEnumerable<MatchWinLossModel> reportData,
            MatchWinLossModel systemParlay,
            MatchWinLossModel grandTotal);

        MatchWinLossWorkbook SetRowMappers(
            Mapper<MatchWinLossModel> betTypeRowMapper,
            Mapper<MatchWinLossModel> subTotalRowMapper,
            Mapper<MatchWinLossModel> grandTotalRowMapper);

    }

    public class MatchWinLossWorkbookImpl : NpoiWorkbook<MatchWinLossModel>, MatchWinLossWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<MatchWinLossModel> NoParlayMatchWinLossDetail { get; set; }

        internal MatchWinLossModel NoParlayWinLossTotal { get; set; }

        internal MatchWinLossModel SystemParlay { get; set; }

        internal MatchWinLossModel GrandTotal { get; set; }

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

        public MatchWinLossWorkbook SetData(
            IEnumerable<MatchWinLossModel> reportData,
            MatchWinLossModel systemParlay,
            MatchWinLossModel grandTotal)
        {
            NoParlayMatchWinLossDetail = reportData;
            SystemParlay = systemParlay;
            GrandTotal = grandTotal;

            return this;
        }

        public MatchWinLossWorkbook SetRowMappers(
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
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = (data.LeagueName + (string.IsNullOrEmpty(data.EventInfo) ? string.Empty : $" >> {data.EventInfo}"))
                .ToUpper(CultureInfo.InvariantCulture);
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, rowIndex, row.LastCellIndex - 1, title);
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
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = AppResources.Parlay.ToUpperInvariant();
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, rowIndex, row.LastCellIndex - 1, title);
        }

        internal void FillSystemParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SystemParlayRowTemplate);
            FillCells(row, SystemParlay, BetTypeRowMapper.Mappings);
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
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                MatchTitleRowTemplate.LastCellIndex - 1,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            var hasData = (NoParlayMatchWinLossDetail != null && NoParlayMatchWinLossDetail.Any())
                || SystemParlay != null;

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
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(GrandTotalRowTemplate);
            FillCells(row, GrandTotal, GrandTotalRowMapper.Mappings);

            foreach (var data in NoParlayMatchWinLossDetail.GroupBy(i => i.MatchId))
            {
                var isEvenRow = false;
                var firstRowOfGroup = data.ToList();
                var subTotal = firstRowOfGroup.Sum();

                FillMatchTitle(sheet, currentRowIndex++, firstRowOfGroup.First());

                foreach (var betType in data)
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betType);
                    isEvenRow = !isEvenRow;
                }

                FillMatchTotal(sheet, currentRowIndex++, subTotal);
            }

            if (SystemParlay != null)
            {
                FillSystemParlayWinLossTitle(sheet, currentRowIndex++);
                FillSystemParlayWinLoss(sheet, currentRowIndex++);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (!SpreadSheet.FooterRows.Any())
            {
                return;
            }

            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(GrandTotalRowTemplate);
            FillCells(row, GrandTotal, GrandTotalRowMapper.Mappings);
        }
    }
}
