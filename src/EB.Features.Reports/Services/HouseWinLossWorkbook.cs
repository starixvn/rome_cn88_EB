namespace EB.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Model;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public interface IHouseWinLossWorkbook
    {
        Workbook<HouseWinLossModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<HouseWinLossModel> Sheet(string sheetName);

        Workbook<HouseWinLossModel> Title(string titleText);

        Workbook<HouseWinLossModel> Footer(HouseWinLossModel row, Mapper<HouseWinLossModel> footerMapper);

        Workbook<HouseWinLossModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        IHouseWinLossWorkbook SetData(
            IEnumerable<HouseWinLossModel> reportData,
            HouseWinLossModel grandTotal);

        IHouseWinLossWorkbook SetRowMappers(
            Mapper<HouseWinLossModel> rowMapper,
            Mapper<HouseWinLossModel> grandTotalRowMapper);

    }

    public class HouseWinLossWorkbookImpl : NpoiWorkbook<HouseWinLossModel>, IHouseWinLossWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<HouseWinLossModel> ModelDetails { get; set; }

        internal HouseWinLossModel GrandTotal { get; set; }

        internal Mapper<HouseWinLossModel> RowMapper { get; set; }

        internal Mapper<HouseWinLossModel> GrandTotalRowMapper { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row GrandTotalRowTemplate { get; set; }

        public IHouseWinLossWorkbook SetData(
            IEnumerable<HouseWinLossModel> reportData,
            HouseWinLossModel grandTotal)
        {
            ModelDetails = reportData;
            GrandTotal = grandTotal;

            return this;
        }

        public IHouseWinLossWorkbook SetRowMappers(
            Mapper<HouseWinLossModel> rowMapper,
            Mapper<HouseWinLossModel> grandTotalRowMapper)
        {
            RowMapper = rowMapper;
            GrandTotalRowMapper = grandTotalRowMapper;
            return this;
        }

        internal void FillBetTypeWinLoss(
            Sheet sheet,
            int rowIndex,
            bool isEvenRow,
            HouseWinLossModel betTypeWinLoss)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, betTypeWinLoss, RowMapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            MatchTitleRowTemplate = sheet.GetRow(currentRowIndex++);
            currentRowIndex = TemplateInfo.BodyStartIndex;            
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            GrandTotalRowTemplate = sheet.GetRow(currentRowIndex++);

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

            var hasData = (ModelDetails != null && ModelDetails.Any());

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
            var isEvenRow = false;
            foreach (var data in ModelDetails)
            {
                FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, data);
                isEvenRow = !isEvenRow;
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (SpreadSheet.FooterRows.Any())
            {
                var row = sheet.GetRow(currentRowIndex);
                row.CopyCellsFrom(GrandTotalRowTemplate);
                FillCells(row, GrandTotal, GrandTotalRowMapper.Mappings);
            }
        }
    }
}
