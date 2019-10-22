namespace EB.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Model;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public interface IPerformanceBonusWorkbook
    {
        Workbook<PerformanceBonusModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<PerformanceBonusModel> Sheet(string sheetName);

        Workbook<PerformanceBonusModel> Title(string titleText);

        Workbook<PerformanceBonusModel> Footer(PerformanceBonusModel row, Mapper<PerformanceBonusModel> footerMapper);

        Workbook<PerformanceBonusModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        IPerformanceBonusWorkbook SetData(IEnumerable<PerformanceBonusModel> viewModel);

        IPerformanceBonusWorkbook SetRowMappers(
            Mapper<PerformanceBonusModel> normalRowMapper,
            Mapper<PerformanceBonusModel> totalRowMapper);

    }

    public class PerformanceBonusWorkbookImpl : NpoiWorkbook<PerformanceBonusModel>, IPerformanceBonusWorkbook
    {
        private const int ReportSheetIndex = 1;

        internal IEnumerable<PerformanceBonusModel> Reports { get; set; }

        internal Mapper<PerformanceBonusModel> NormalRowMapper { get; set; }

        internal Mapper<PerformanceBonusModel> TotalRowMapper { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public IPerformanceBonusWorkbook SetData(IEnumerable<PerformanceBonusModel> viewModel)
        {
            this.Reports = viewModel;

            return this;
        }

        public IPerformanceBonusWorkbook SetRowMappers(
            Mapper<PerformanceBonusModel> normalRowMapper,
            Mapper<PerformanceBonusModel> totalRowMapper)
        {
            NormalRowMapper = normalRowMapper;
            TotalRowMapper = totalRowMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);
            var index = TemplateInfo.BodyStartIndex;
            OddRowTemplate = sheet.GetRow(index++);
            EvenRowTemplate = sheet.GetRow(index++);
            TotalRowTemplate = sheet.GetRow(index);

            return GetSheetAt(ReportSheetIndex);
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
                TotalRowTemplate.LastCellIndex - 1,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void FillBody(Sheet sheet)
        {
            RemoveTemplateRows(sheet);

            var hasData = Reports != null && Reports.Any();

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
            var index = TemplateInfo.BodyStartIndex;
            var isEvenRow = false;
            foreach (var model in Reports)
            {
                var row = sheet.GetRow(index++);
                var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

                row.CopyCellsFrom(templateRow);
                FillCells(row, model, NormalRowMapper.Mappings);

                isEvenRow = !isEvenRow;
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (SpreadSheet.FooterRows.Any())
            {
                var row = sheet.GetRow(TemplateInfo.BodyStartIndex + Reports.Count());
                row.CopyCellsFrom(TotalRowTemplate);
                FillCells(row, Reports.Sum(), TotalRowMapper.Mappings);
            }
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

    }
}
