using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().ExportJiraIssuesToExcel();
            Console.ReadKey();
        }

        private async void TestSearchResult()
        {
            var jiraSearch = new JiraSearch();
            var issues = await jiraSearch.SearchIssuesAsync("Bug", JiraLogin.Project);
        }

        private async void ExportJiraIssuesToExcel()
        {
            Console.WriteLine("Searching Jira issues...");
            var jiraSearch = new JiraSearch();
            var issues = await jiraSearch.SearchIssuesAsync("Bug", JiraLogin.Project);

            Console.WriteLine("Create Jira issues excel report D:\\JiraReport.xlsx...");
            Report report = new Report();
            report.CreateExcelDoc(@"D:\JiraReport.xlsx", issues);

            Console.WriteLine("Excel file has created!");
        }
    }
}
