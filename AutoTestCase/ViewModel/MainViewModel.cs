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
    public class MainViewModel : ViewModelBase,IShowWindow
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

        public TestCase SelectedTestCase
        {
            get { return selectedTestCase; }
            set
            {
                Set(ref selectedTestCase , value);
                ShowWindowEvent?.Invoke(this, value);
            }
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
            TestCases.Clear();
            Result = e.Result;
            var jArray = JsonConvert.DeserializeObject(Result) as JArray;
            if (jArray != null && jArray.Count > 0)
            {
                var list = jArray.ToObject<IList<TestCase>>();
                foreach (var item in list)
                {
                    TestCases.Add(item);
                }
            }
        }

        private void Search()
        {

            if (!Client.IsBusy)
            { 
                Client.DownloadStringAsync(new Uri(ServiceAddress + TestCaseId));
            }
        }
    }
}
