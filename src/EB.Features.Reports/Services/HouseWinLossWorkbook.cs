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
    public interface HouseWinLossWorkbook
    {
        Workbook<HouseWinLossModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<HouseWinLossModel> Sheet(string sheetName);

        Workbook<HouseWinLossModel> Title(string titleText);

        Workbook<HouseWinLossModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        HouseWinLossWorkbook SetData(
            IEnumerable<HouseWinLossModel> reportData,
            HouseWinLossModel grandTotal);

        HouseWinLossWorkbook SetRowMappers(
            Mapper<HouseWinLossModel> betTypeRowMapper,
            Mapper<HouseWinLossModel> grandTotalRowMapper);

    }

    public class HouseWinLossWorkbookImpl : NpoiWorkbook<HouseWinLossModel>, HouseWinLossWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<HouseWinLossModel> NoParlayMatchWinLossDetail { get; set; }

        internal HouseWinLossModel NoParlayWinLossTotal { get; set; }

        internal HouseWinLossModel GrandTotal { get; set; }

        internal Mapper<HouseWinLossModel> BetTypeRowMapper { get; set; }
        
        internal Mapper<HouseWinLossModel> GrandTotalRowMapper { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row GrandTotalRowTemplate { get; set; }

        public HouseWinLossWorkbook SetData(
            IEnumerable<HouseWinLossModel> reportData,
            HouseWinLossModel grandTotal)
        {
            NoParlayMatchWinLossDetail = reportData;
            GrandTotal = grandTotal;

            return this;
        }

        public HouseWinLossWorkbook SetRowMappers(
            Mapper<HouseWinLossModel> betTypeRowMapper,
            Mapper<HouseWinLossModel> grandTotalRowMapper)
        {
            BetTypeRowMapper = betTypeRowMapper;
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
            FillCells(row, betTypeWinLoss, BetTypeRowMapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;
            GrandTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            MatchTitleRowTemplate = sheet.GetRow(currentRowIndex++);

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

            var hasData = (NoParlayMatchWinLossDetail != null && NoParlayMatchWinLossDetail.Any());

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

            foreach (var data in NoParlayMatchWinLossDetail)
            {
                var isEvenRow = false;

                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, data);
                    isEvenRow = !isEvenRow;                

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
