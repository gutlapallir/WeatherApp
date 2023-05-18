// S{FileName}
//
// Author:
//     S{AuthorName}<2109834@cognizant.com>
//
// Copyright (c) 2023 S{CopyrightHolder}
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHAL
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHE
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FR
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// THE SOFTWARE
using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using WeatherSampleApp.Interfaces;
using WeatherSampleApp.Model;
using WeatherSampleApp.Services;
using WeatherSampleApp.Utilitys;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherSampleApp.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private Variables
        /// <summary>
        /// The service manager
        /// </summary>
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// The City Name
        /// </summary>
        private string cityName = string.Empty;

        /// <summary>
        /// The title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// The temperature
        /// </summary>
        private string temperature = string.Empty;

        /// <summary>
        /// The speed
        /// </summary>
        private string speed = string.Empty;

        /// <summary>
        /// The pressure
        /// </summary>
        private string pressure = string.Empty;

        /// <summary>
        /// The humidity
        /// </summary>
        private string humidity = string.Empty;

        /// <summary>
        /// The sunraise
        /// </summary>
        private long sunrise;

        /// <summary>
        /// The sunset
        /// </summary>
        private long sunset;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the city name
        /// </summary>
        /// <value>The temp city name</value>
        public string CityName
        {
            get
            {
                return cityName;
            }
            set
            {
                if(cityName != null)
                {
                    cityName = value;
                    OnPropertyChanged("CityName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        /// <value>The title</value>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != null)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the temperature
        /// </summary>
        /// <value>The temperature</value>
        public string Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (temperature != null)
                {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }

        /// <summary>
        /// Gets or sets the speed
        /// </summary>
        /// <value>The speed</value>
        public string Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (speed != null)
                {
                    speed = value;
                    OnPropertyChanged("Speed");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Pressure
        /// </summary>
        /// <value>The Pressure</value>
        public string Pressure
        {
            get
            {
                return pressure;
            }
            set
            {
                if (pressure != null)
                {
                    pressure = value;
                    OnPropertyChanged("Pressure");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Humidity
        /// </summary>
        /// <value>The Humidity</value>
        public string Humidity
        {
            get
            {
                return humidity;
            }
            set
            {
                if (humidity != null)
                {
                    humidity = value;
                    OnPropertyChanged("Humidity");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Sunrise
        /// </summary>
        /// <value>The Sunrise</value>
        public long Sunrise
        {
            get
            {
                return sunrise;
            }
            set
            {
                sunrise = value;
                OnPropertyChanged("Sunrise");
            }
        }

        /// <summary>
        /// Gets or sets the Sunset
        /// </summary>
        /// <value>The Sunset</value>
        public long Sunset
        {
            get
            {
                return sunset;
            }
            set
            {
                sunset = value;
                OnPropertyChanged("Sunset");
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// Gets the weather command
        /// </summary>
        public Command GetWeatherCommand { get; private set; }
        #endregion

        public MainPageViewModel()
        {

        }
        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="serviceManager"></param>
        public MainPageViewModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            if(CityName == string.Empty)
            {
                CityName = getValueForPostalCode();
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    GetWeatherData();
                }
            }
            GetWeatherCommand = new Command(async () => await GetWeatherDataAsync());
        }

        #region Methods
        /// <summary>
        /// The get weather data
        /// </summary>
        /// <returns></returns>
        private Task GetWeatherDataAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    GetWeatherData();
                }
                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// The get weather data
        /// </summary>
        public async void GetWeatherData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                    Application.Current.Properties["PostalCode"] = CityName;
                    await Application.Current.SavePropertiesAsync();
                    WeatherData weatherData = await _serviceManager.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(weatherData);
                    Application.Current.Properties["WeatherData"] = jsonString;
                    await Application.Current.SavePropertiesAsync();
                    Title = weatherData.Title;
                    Temperature = weatherData.Main.Temperature.ToString();
                    Speed = weatherData.Wind.Speed.ToString();
                    Humidity = weatherData.Main.Humidity.ToString();
                    Sunrise = weatherData.Sys.Sunrise;
                    Sunset = weatherData.Sys.Sunset;
                    Pressure = weatherData.Main.Pressure.ToString();
                }
                else
                {
                    if (Application.Current.Properties["WeatherData"] != null)
                    {
                        var weatherDetail = (Application.Current.Properties["WeatherData"].ToString());
                        var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(weatherDetail);
                        Title = weatherData.Title;
                        Temperature = weatherData.Main.Temperature.ToString();
                        Speed = weatherData.Wind.Speed.ToString();
                        Humidity = weatherData.Main.Humidity.ToString();
                        Sunrise = weatherData.Sys.Sunrise;
                        Sunset = weatherData.Sys.Sunset;
                        Pressure = weatherData.Main.Pressure.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /// <summary>
        /// The value for postal code
        /// </summary>
        /// <returns></returns>
        public string getValueForPostalCode()
        {
            if (Application.Current.Properties.ContainsKey("PostalCode"))
            {
                if (Application.Current.Properties["PostalCode"] != null)
                {
                    var postalCode = (Application.Current.Properties["PostalCode"].ToString());

                    return postalCode;
                }
            }
            return "";
        }

        /// <summary>
        /// The request url
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={CityName}";
            requestUri += "&units=imperial";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
        #endregion
    }
}
