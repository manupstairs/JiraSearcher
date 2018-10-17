using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoTestCase.ViewModel
{
    public class MainViewModel : ViewModelBase,IShowWindow,IShowMessage
    {
        private string testCaseId = "excalibur-3213";
        private WebClient Client { get; set; } = new WebClient();
        private readonly string ServiceAddress = ConfigurationManager.AppSettings["ServiceAddress"];

        public string TestCaseId
        {
            get { return testCaseId; }
            set
            {
                Set(ref testCaseId, value);
            }
        }

        private string result;

        public string Result
        {
            get { return result; }
            set
            {
                Set(ref result, value);
            }
        }

        private TestCase selectedTestCase;

        public event EventHandler<TestCase> ShowWindowEvent;
        public event EventHandler<string> ShowMessageEvent;

        public TestCase SelectedTestCase
        {
            get { return selectedTestCase; }
            set
            {
                Set(ref selectedTestCase, value);
                ShowWindowEvent?.Invoke(this, value);
                selectedTestCase = null;
            }
        }

        private bool isSearching;

        public bool IsSearching
        {
            get { return isSearching; }
            set { Set(ref isSearching, value); }
        }

        public ObservableCollection<TestCase> TestCases { get; set; } = new ObservableCollection<TestCase>();

        public ICommand SearchCommand { get; set; }

        public MainViewModel()
        {
            SearchCommand = new RelayCommand(Search);
            Client.DownloadStringCompleted += Client_DownloadStringCompleted;
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            IsSearching = false;
            TestCases.Clear();
            try
            {
                Result = e.Result;
            }
            catch (Exception)
            {
                this.ShowMessageEvent?.Invoke(this,"AI engine is not working now! Please try again later.");
                return;
            }

            var testCase = JsonConvert.DeserializeObject<TestCase>(Result);
            TestCases.Add(testCase);
        }

        private void Search()
        {
            IsSearching = true;
            if (!Client.IsBusy)
            { 
                Client.DownloadStringAsync(new Uri(ServiceAddress + TestCaseId));
            }
        }
    }
}
