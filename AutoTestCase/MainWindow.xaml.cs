using AutoTestCase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoTestCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            vm.ShowWindowEvent += (sender, e) =>
              {
                  var page = new DetailPage(e);
                  page.ShowDialog();
              };
            vm.ShowMessageEvent += (sender,e)=>
            {
                MessageBox.Show(e);
            };
            this.DataContext = vm;
        }
    }
}
