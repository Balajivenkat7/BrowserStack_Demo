//Created Date: 13 Dec 2024
//Created By: Balaji Venkatesan

/*  This class contians Test Setup and TearDown Methods
    Which usaully runs before the all test runs.
    All Test calss will inherit this BaseTest class  */

using UX_Automation.Core;
using UX_Automation.Reporter;
using UX_Automation.TestData;
using NUnit.Framework;
using OpenQA.Selenium;

namespace UX_Automation.Tests
{
    public class BaseTest
    {
        protected IWebDriver Driver { get; set; }
        public TestReporter testReporter { get; set; }
        protected WebDriverSetup webDriverInstance = new WebDriverSetup();

        [SetUp]
        public void TestSetup()
        {
            Driver = webDriverInstance.CreateWebDriver();
            testReporter = new TestReporter();
            testReporter.AddTestCaseMetaDataToBrowserStack(Driver);
            Console.WriteLine("Test Initialized");
        }

        [TearDown]
        public void TestCleanUp()
        {
            if (Driver != null)
            {
                try { testReporter?.RecordTestCaseOutCome(Driver); }
                catch (Exception ex) { Console.WriteLine("TestReporter failed: " + ex.Message); }

                try { Driver.Close(); Driver.Quit(); }
                catch (Exception ex) { Console.WriteLine("Driver quit failed: " + ex.Message); }

            }
            Console.WriteLine("Test Concluded");
        }
    }
}