using EB.Features.Reports.Model;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Services
{
    public interface ReportsWorkbook
    {
        Workbook<ReportModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<ReportModel> Sheet(string sheetName);

        Workbook<ReportModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        ReportsWorkbook SetData(IEnumerable<ReportModel> reportData, ReportModel total, bool isShowProfitMargin);

        ReportsWorkbook SetRowMappers(Mapper<ReportModel> bodyRowMapper, Mapper<ReportModel> totalRowMapper, Mapper<ReportModel> footerRowMapper);
    }

    public class ReportsWorkbookImpl : NpoiWorkbook<ReportModel>, ReportsWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal bool IsShowProfitMargin;

        internal IEnumerable<ReportModel> ReportData { get; set; }

        internal ReportModel Total { get; set; }

        internal Mapper<ReportModel> BodyRowMapper { get; set; }

        internal Mapper<ReportModel> TotalRowMapper { get; set; }

        internal Mapper<ReportModel> FooterRowMapper { get; set; }

        internal Row BodyRowTemplate { get; set; }

        internal Row FooterRowTemplate { get; set; }

        public ReportsWorkbook SetData(IEnumerable<ReportModel> reportData, ReportModel total, bool isShowProfitMargin)
        {
            ReportData = reportData;
            Total = total;
            IsShowProfitMargin = isShowProfitMargin;

            return this;
        }

        public ReportsWorkbook SetRowMappers(
            Mapper<ReportModel> bodyRowMapper,
            Mapper<ReportModel> totalRowMapper,
            Mapper<ReportModel> footerRowMapper)
        {
            BodyRowMapper = bodyRowMapper;
            TotalRowMapper = totalRowMapper;
            FooterRowMapper = footerRowMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;
            BodyRowTemplate = sheet.GetRow(currentRowIndex++);
            FooterRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.OddRowTemplate = BodyRowTemplate;
            reportSheet.EvenRowTemplate = BodyRowTemplate;
            reportSheet.FooterRowsTemplate = new List<Row> { FooterRowTemplate };

            return reportSheet;
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (ReportData.Any())
            {
                FillBodyRows(sheet);
                if (IsShowProfitMargin)
                {
                    FillFooterRows(sheet);
                }
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

            var totalRow = sheet.GetRow(currentRowIndex);
            totalRow.CopyCellsFrom(BodyRowTemplate);
            FillCells(totalRow, Total, TotalRowMapper.Mappings);

            foreach (var data in ReportData)
            {
                var row = sheet.GetRow(++currentRowIndex);
                row.CopyCellsFrom(BodyRowTemplate);
                FillCells(row, data, BodyRowMapper.Mappings);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            var row = sheet.GetRow(++currentRowIndex);
            row.CopyCellsFrom(FooterRowTemplate);
            FillCells(row, Total, FooterRowMapper.Mappings);
        }
    }
}
