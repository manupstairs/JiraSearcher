using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraSearcher
{
    class JiraSearch
    {
        public async void TestJiraSearch()
        {

            var project = await JiraLogin.JIRA.Projects.GetProjectAsync(JiraLogin.Project);
            Console.WriteLine(project.ToString());
        }

        public async Task<IEnumerable<Issue>> SearchIssuesAsync(string type, string project)
        {
            var issues = new List<Issue>();
            string jql = $"project = {project} AND type = {type} AND resolution = Unresolved order by priority DESC,updated DESC";
            foreach (var issue in await JiraLogin.JIRA.Issues.GetIssuesFromJqlAsync(jql, 500))
            {
                issues.Add(issue);
            }

            return issues;
        }
    }
}
