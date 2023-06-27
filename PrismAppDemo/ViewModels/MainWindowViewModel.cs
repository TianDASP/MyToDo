using Prism.Mvvm;
using System.Windows;

namespace PrismAppDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application Demo";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }

        public void Test()
        {
            MessageBox.Show("test");
        }
    }
}
