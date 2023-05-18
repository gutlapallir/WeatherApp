using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherSampleApp.Interfaces;
using WeatherSampleApp.Services;
using WeatherSampleApp.ViewModel;
using Xamarin.Forms;

namespace WeatherSampleApp
{
    public partial class MainPage : ContentPage
    {
        //IServiceManager _serviceManager;
        //public IServiceManager serviceManager
        //{
        //    get => _serviceManager;
        //    set => _serviceManager = value;
        //}
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(new ServiceManager());
        }
    }
}
