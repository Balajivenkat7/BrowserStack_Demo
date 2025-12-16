//Created Date: 17 Dec 2024
//Created By: Balaji Venkatesan

/*  This classs contains the method that helps 
    to parser the test data present in 
    TestData.yml and returns the string value.  */

using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace UX_Automation.TestData
{
     public class TestDataParser
    {
        private static IConfigurationRoot ConfigBuilder;
        private const string YamlConfigurationFile = "Resources.yml";
        private const string YamlDirectoryName = "TestData";
        private static string YamlDirectoryPath;

        public TestDataParser()
        {
            ConfigBuilder = GetConfigValue();
        }

        public static string GetProjectPath()
        {
            var path = System.Reflection.Assembly.GetCallingAssembly().Location;
            var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            return new Uri(actualPath).LocalPath;
        }

        private static IConfigurationRoot GetConfigValue()
        {
            try{
                var projectPath = GetProjectPath();
                var fileInfo = new FileInfo(projectPath + YamlDirectoryName);
                YamlDirectoryPath = fileInfo?.FullName;
            }
            catch(DirectoryNotFoundException ex){
                Console.WriteLine(ex.Message);
            }

           
            ConfigBuilder = new ConfigurationBuilder()
               .SetBasePath(YamlDirectoryPath)
               .AddYamlFile(YamlConfigurationFile)
               .Build();
           
            return ConfigBuilder;
        }

        public string GetAppUrl()
        {
            var url = ConfigBuilder["app_Url"];
            return url;
        }

        public string GetBrowserVersion()
        {
            var browserVersion = ConfigBuilder["browserVersion"];
            return browserVersion;
        }

        public string GetOsType()
        {
            var osType = ConfigBuilder["ostype"];
            return osType;
        }

        public string GetOsVersion()
        {
            var osVersion = ConfigBuilder["osVersion"];
            return osVersion;
        }

        public int GetDefaultTimeOutInSeconds()
        {
            var timeOut = ConfigBuilder["default_TimeOut"];
            return Convert.ToInt32(timeOut);
        }

        public string GetProjectName()
        {
            var projectName = ConfigBuilder["projectName"];
            return projectName;
        }

        public string GetMobileDeviceName()
        {
            var mobileDeviceName = ConfigBuilder["MobileDeviceName"];
            return mobileDeviceName;
        }

        public string GetMobileDeviceOSVersion()
        {
            var mobileDeviceOSVersion = ConfigBuilder["MobileOsVersion"];
            return mobileDeviceOSVersion;
        }

        public string GetBStackIdleTimeOut()
        {
            var bStackIdleTimeOut = ConfigBuilder["bStackIdleTimeOut"];
            return bStackIdleTimeOut;
        }

        public string GetBStackLocalIdentifier()
        {
            var bStackLocalIdentifer = ConfigBuilder["bStacklocalIdentifier"];
            return bStackLocalIdentifer;
        }

        public string GetTrafficUrl()
        {
            var trafficUrl = ConfigBuilder["TrafficUrl"];
            return trafficUrl;
        }

        public string Get10DayForecastUrl()
        {
            var tenDayForecastUrl = ConfigBuilder["10DayForecastUrl"];
            return tenDayForecastUrl;
        }

        public string GetWeatherUrl()
        {
            var weatherUrl = ConfigBuilder["WeatherUrl"];
            return weatherUrl;
        }

        public string GetWatchUrl()
        {
            var watchUrl = ConfigBuilder["WatchUrl"];
            return watchUrl;
        }

        public string GetNearMeUrl()
        {
            var nearMeUrl = ConfigBuilder["NearMeUrl"];
            return nearMeUrl;
        }

        public string GetNewsUrl()
        {
            var newsUrl = ConfigBuilder["NewsUrl"];
            return newsUrl;
        }

        public string GetInputData(string dataType, string dataName)
        {
            var dirPath = GetProjectPath();
            var actualpath = Path.Combine(dirPath, "Data");
            var returnData = "";
            switch (dataType.ToLower())
            {
                case "image":
                    var imagePath = Path.Combine(actualpath, "Image");
                    returnData = Path.Combine(imagePath, dataName + ".jpeg");
                    break;
                case "audio":
                    var audioPath = Path.Combine(actualpath, "Audio");
                    returnData = Path.Combine(audioPath, dataName + ".mp3");
                    break;
                case "video":
                    var videoPath = Path.Combine(actualpath, "Video");
                    returnData = Path.Combine(videoPath, dataName + ".mp4");
                    break;
                default:
                    Assert.Fail("No proper input data type or data name is given. Please provide audio, video or image data type with data name");
                    break;
            }
            return returnData;
        }
    }
}