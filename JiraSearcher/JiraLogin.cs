using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraSearcher
{
    class JiraLogin
    {
        public static string UserName => ConfigurationManager.AppSettings["UserName"];
        public static string Password => ConfigurationManager.AppSettings["Password"];
        public static string Project => ConfigurationManager.AppSettings["Project"];
        public static string Server => ConfigurationManager.AppSettings["Server"];

        public static Jira JIRA => Jira.CreateRestClient(JiraLogin.Server, JiraLogin.UserName, JiraLogin.Password);
    }
}
