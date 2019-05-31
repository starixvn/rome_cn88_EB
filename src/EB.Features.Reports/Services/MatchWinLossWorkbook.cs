using EB.Features.Reports.Model;
using EB.Features.Reports.ViewModel;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Services
{
    public interface MatchWinLossWorkbook
    {
        Workbook<MatchWinLossViewModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<MatchWinLossViewModel> Sheet(string sheetName);

        Workbook<MatchWinLossViewModel> Title(string titleText);

        Workbook<MatchWinLossViewModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        MatchWinLossWorkbook SetData(
            IEnumerable<MatchWinLossViewModel> reportData,
            MatchWinLossViewModel systemParlay,
            MatchWinLossViewModel grandTotal);

        MatchWinLossWorkbook SetRowMappers(
            Mapper<MatchWinLossViewModel> betTypeRowMapper,
            Mapper<MatchWinLossViewModel> subTotalRowMapper,
            Mapper<MatchWinLossViewModel> grandTotalRowMapper);

    }

    public class MatchWinLossWorkbookImpl : NpoiWorkbook<MatchWinLossViewModel>, MatchWinLossWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<MatchWinLossViewModel> NoParlayMatchWinLossDetail { get; set; }

        internal MatchWinLossViewModel NoParlayWinLossTotal { get; set; }

        internal MatchWinLossViewModel SystemParlay { get; set; }

        internal MatchWinLossViewModel GrandTotal { get; set; }

        internal Mapper<MatchWinLossViewModel> BetTypeRowMapper { get; set; }

        internal Mapper<MatchWinLossViewModel> SubTotalRowMapper { get; set; }

        internal Mapper<MatchWinLossViewModel> TotalRowMapper { get; set; }

        internal Mapper<MatchWinLossViewModel> GrandTotalRowMapper { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row SubTotalRowTemplate { get; set; }


        internal Row SystemParlayRowTemplate { get; set; }

        internal Row GrandTotalRowTemplate { get; set; }

        public MatchWinLossWorkbook SetData(
            IEnumerable<MatchWinLossViewModel> reportData,
            MatchWinLossViewModel systemParlay,
            MatchWinLossViewModel grandTotal)
        {
            NoParlayMatchWinLossDetail = reportData;
            SystemParlay = systemParlay;
            GrandTotal = grandTotal;

            return this;
        }

        public MatchWinLossWorkbook SetRowMappers(
            Mapper<MatchWinLossViewModel> betTypeRowMapper,
            Mapper<MatchWinLossViewModel> subTotalRowMapper,
            Mapper<MatchWinLossViewModel> grandTotalRowMapper)
        {
            BetTypeRowMapper = betTypeRowMapper;
            SubTotalRowMapper = subTotalRowMapper;
            GrandTotalRowMapper = grandTotalRowMapper;
            return this;
        }

        internal void FillMatchTitle(Sheet sheet, int rowIndex, MatchWinLossViewModel data)
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
            MatchWinLossViewModel betTypeWinLoss)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, betTypeWinLoss, BetTypeRowMapper.Mappings);
        }

        internal void FillMatchTotal(Sheet sheet, int rowIndex, MatchWinLossViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SubTotalRowTemplate);
            FillCells(row, data, SubTotalRowMapper.Mappings);
        }

        internal void FillSystemParlayWinLossTitle(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = "PARLAY";
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
                var subTotal = Sum(firstRowOfGroup);

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

        private MatchWinLossViewModel Sum(IEnumerable<MatchWinLossViewModel> matches)
        {
            var total = new MatchWinLossViewModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}
