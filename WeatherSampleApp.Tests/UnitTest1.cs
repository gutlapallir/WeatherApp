using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WeatherSampleApp.Interfaces;
using WeatherSampleApp.Model;
using WeatherSampleApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace WeatherSampleApp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            MockForms.Init();
        }
              
        public class FakeServicemanager : IServiceManager
        {
            public async Task<WeatherData> GetWeatherData(string query)
            {
                var mainResObj = new WeatherData();
                string jsonResponse = "{\"coord\":{\"lon\":-99.2506,\"lat\":31.2504},\"weather\":[{\"id\":804,\"main\":\"Clouds\",\"description\":\"overcastclouds\",\"icon\":\"04d\"}],\"base\":\"stations\",\"main\":{\"temp\":64.69,\"feels_like\":65.55,\"temp_min\":62.74,\"temp_max\":65.64,\"pressure\":1020,\"humidity\":100},\"visibility\":10000,\"wind\":{\"speed\":6.91,\"deg\":10},\"clouds\":{\"all\":100},\"dt\":1684237517,\"sys\":{\"type\":1,\"id\":3395,\"country\":\"US\",\"sunrise\":1684237245,\"sunset\":1684286771},\"timezone\":-18000,\"id\":4736286,\"name\":\"Texas\",\"cod\":200}";
                WeatherData weatherData = new WeatherData();
                weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                return weatherData;
            }
        }

        [Test]
        public void Test_GetWeatherData()
        {
            Application.Current = new App();
            Application.Current.Properties["PostalCode"] = "Hyderabad";
            var mainPageViewModel = new MainPageViewModel(new FakeServicemanager());
            mainPageViewModel.GetWeatherData();
        }
    }
}
