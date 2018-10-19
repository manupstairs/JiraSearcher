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
    public class MainViewModel : ViewModelBase, IShowWindow, IShowMessage
    {

        private WebClient Client { get; set; } = new WebClient();
        private readonly string ServiceAddress = ConfigurationManager.AppSettings["ServiceAddress1"];

        private string testCaseId = "excalibur-2330";
        public string TestCaseId
        {
            get { return testCaseId; }
            set
            {
                Set(ref testCaseId, value);
            }
        }

        private string document;

        public string Document
        {
            get { return document; }
            set { Set(ref document, value); }
        }

        private bool isUserStoryId = true;

        public bool IsUserStoryId
        {
            get { return isUserStoryId; }
            set { Set(ref isUserStoryId, value); }
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
            Client.UploadValuesCompleted += Client_UploadValuesCompleted;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            IsSearching = false;
            TestCases.Clear();
            try
            {
                Result = Encoding.UTF8.GetString(e.Result);
            }
            catch (Exception)
            {
                this.ShowMessageEvent?.Invoke(this, "AI engine is not working now! Please try again later.");
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
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                if (IsUserStoryId)
                {
                    reqparm.Add("id", TestCaseId);
                }
                else
                {
                    reqparm.Add("document", Document);
                }
                Client.UploadValuesAsync(new Uri(ServiceAddress), "POST", reqparm);
            }
        }
    }
}
