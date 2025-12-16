//Created Date: 13 Dec 2024
//Created By: Balaji Venkatesan

/*  This Class does the driver initialization
    It get all BrowserStack capbabilities and
    initialize the driver and make BrowerStack connection  */

using UX_Automation.TestData;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using BrowserStack;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace UX_Automation.Core
{
    public class WebDriverSetup
    {
        public TestDataParser testDataParser;
        private static string browserstack_Url { get; set; }
        private static string bStackUserName { get; set; }
        private static string bStackAccessKey { get; set; }
        private static string bstackBuildName { get; set; }
        private static string bStackLocal { get; set; }
        private static string Platform { get; set; }
        private static string Browser { get; set; }
        private Local bsLocal { get; set; }
        private DriverOptions driverOptions { get; set; }

        public WebDriverSetup()
        {
            testDataParser = new TestDataParser();
            bStackUserName = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            bStackAccessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            bstackBuildName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            bStackLocal = Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL");
            browserstack_Url = GetBrowserStackUrl();
            Platform = TestContext.Parameters["Platform"];
            Browser = TestContext.Parameters["Browser"];
        }

        public IWebDriver CreateWebDriver()
        {
            if (Platform == "Desktop")  return GetDesktopDriverInstance();
            else if (Platform == "Mobile") return GetMobileDriverInstance();
            else throw new ArgumentException("Please specify correct platform to run the test :)");            
        }

        public IWebDriver GetDesktopDriverInstance()
        {
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions = GetWebCapabilities();
            var capabilities = GetBrowserType();
            capabilities.AddAdditionalOption("bstack:options", browserstackOptions);

            return new RemoteWebDriver(
                new Uri(browserstack_Url), capabilities.ToCapabilities(), TimeSpan.FromMinutes(5)
            );
        }

        public IWebDriver GetMobileDriverInstance()
        {
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions = GetResponsiveCapabilities();
            var capabilities = ChromeCapabilities();
            capabilities.AddAdditionalOption("bstack:options", browserstackOptions);

            return new RemoteWebDriver(
                new Uri(browserstack_Url), capabilities.ToCapabilities(), TimeSpan.FromMinutes(5)
            );
        }

        public DriverOptions GetBrowserType()
        {
            switch (Browser)
            {
                case "Chrome":
                    driverOptions = ChromeCapabilities();
                    break;
                case "FireFox":
                    driverOptions = FirefoxCapabilities();
                    break;
                case "Edge":
                    driverOptions = EdgeCapabilities();
                    break;
                default:
                    Assert.Fail("Browser Name is not specificed. Please declare Browser type to run the script");
                    break;
            }
            return driverOptions;
        }

        public ChromeOptions ChromeCapabilities()
        {
            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = testDataParser.GetBrowserVersion();
            capabilities.AddArgument("--start-maximized");
            capabilities.AddArguments("disable-popup-blocking");
            capabilities.AddArguments("--disable-notifications");
            capabilities.AddArguments("no-sandbox");
            capabilities.SetLoggingPreference(LogType.Performance, LogLevel.Info);
            capabilities.AcceptInsecureCertificates = false;
            return capabilities;
        }

        public FirefoxOptions FirefoxCapabilities()
        {
            FirefoxOptions capabilities = new FirefoxOptions();
            capabilities.BrowserVersion = testDataParser.GetBrowserVersion();
            capabilities.AddArgument("--start-maximized");
            capabilities.AddArguments("disable-popup-blocking");
            capabilities.AddArguments("--disable-notifications");
            capabilities.AddArguments("no-sandbox");
            return capabilities;
        }

        public EdgeOptions EdgeCapabilities()
        {
            EdgeOptions capabilities = new EdgeOptions();
            capabilities.BrowserVersion = testDataParser.GetBrowserVersion();
            capabilities.AddArgument("--start-maximized");
            capabilities.AddArguments("disable-popup-blocking");
            capabilities.AddArguments("--disable-notifications");
            capabilities.AddArguments("no-sandbox");
            return capabilities;
        }

        public Dictionary<string, Object> GetWebCapabilities()
        {
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            //    Dictionary<string, object> networkLogsOptions = new Dictionary<string, object>();
            //    networkLogsOptions.Add("captureContent", "true");
            browserstackOptions.Add("os", testDataParser.GetOsType());
            browserstackOptions.Add("osVersion", testDataParser.GetOsVersion());
            browserstackOptions.Add("local", bStackLocal);
            browserstackOptions.Add("localIdentifier", testDataParser.GetBStackLocalIdentifier());
            browserstackOptions.Add("idleTimeout", testDataParser.GetBStackIdleTimeOut());
            //    browserstackOptions.Add("networkLogs", "true");
            //    browserstackOptions.Add("networkLogsOptions", networkLogsOptions);
            browserstackOptions.Add("projectName", testDataParser.GetProjectName());
            browserstackOptions.Add("buildName", bstackBuildName);
            return browserstackOptions;
        }

        public Dictionary<string, Object> GetResponsiveCapabilities()
        {
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            //    Dictionary<string, object> networkLogsOptions = new Dictionary<string, object>();
            //    networkLogsOptions.Add("captureContent", "true");
            browserstackOptions.Add("osVersion", testDataParser.GetMobileDeviceOSVersion());
            browserstackOptions.Add("deviceName", testDataParser.GetMobileDeviceName());
            browserstackOptions.Add("local", bStackLocal);
            browserstackOptions.Add("localIdentifier", testDataParser.GetBStackLocalIdentifier());
            browserstackOptions.Add("idleTimeout", testDataParser.GetBStackIdleTimeOut());
            //    browserstackOptions.Add("networkLogs", "true");
            //    browserstackOptions.Add("networkLogsOptions", networkLogsOptions);
            browserstackOptions.Add("projectName", testDataParser.GetProjectName());
            browserstackOptions.Add("buildName", bstackBuildName);
            browserstackOptions.Add("consoleLogs", "info");
            return browserstackOptions;
        }

        public string GetBrowserStackUrl()
        {
            var url = $"https://{bStackUserName}:{bStackAccessKey}@hub-cloud.browserstack.com/wd/hub/";
            return url;
        }

        public void SetupLocalProxyInstance()
        {
            bsLocal = new Local();
            List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>();
            // Adding required arguments for the Local instance.
            bsLocalArgs.Add(new KeyValuePair<string, string>("key", bStackAccessKey));
            bsLocalArgs.Add(new KeyValuePair<string, string>("forcelocal", "true"));
            bsLocalArgs.Add(new KeyValuePair<string, string>("localIdentifier", testDataParser.GetBStackLocalIdentifier()));
            // Starts the Local instance with the required arguments.
            bsLocal.start(bsLocalArgs);
        }

        public void TearDownLocalProxyInstance()
        {
            bsLocal.stop();
        }
    }
}
