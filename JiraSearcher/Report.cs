using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JiraSearcher
{
    class Report
    {
        public void CreateExcelDoc(string fileName, IEnumerable<Atlassian.Jira.Issue> issues)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Jira Issues" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                WriteData(worksheetPart, issues);
            }
        }

        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        private void WriteData(WorksheetPart worksheetPart, IEnumerable<Atlassian.Jira.Issue> issues)
        {
            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
            Row row = CreateHeaderRow(issues);
            // Insert the header row to the Sheet Data
            sheetData.AppendChild(row);

            foreach (var issue in issues)
            {
                row = CreateDataRow(issue);

                sheetData.AppendChild(row);
            }
            worksheetPart.Worksheet.Save();
        }

        private Row CreateHeaderRow(IEnumerable<Atlassian.Jira.Issue> issues)
        {
            // Constructing header
            Row row = new Row();
            var issue = issues.First();
            foreach (var property in issue.GetType().GetProperties().Where(p=>p.Name != "Item"))
            {
                row.Append(ConstructCell(property.Name, CellValues.String));
            }

            return row;
        }

        private Row CreateDataRow(Atlassian.Jira.Issue issue)
        {
            Row row = new Row();
            foreach (var property in issue.GetType().GetProperties().Where(p => p.Name != "Item"))
            {
                var value = property.GetValue(issue) == null ? string.Empty : property.GetValue(issue).ToString();
                row.Append(ConstructCell(value, CellValues.String));
            }

            return row;
        }
    }
}
