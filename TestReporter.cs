//Created Date: 18 Dec 2024
//Created By: Balaji Venkatesan

/*  This class helps to set the Test session name 
    and test outcome in BrowserStack Dashboard using
    TestContext class and JavaScriptExecutor.   */

using NUnit.Framework;
using OpenQA.Selenium;

namespace UX_Automation.Reporter
{
    public class TestReporter
    {
        private TestContext TestConText { get; }

        public TestReporter()
        {
            TestConText = TestContext.CurrentContext;
        }

        public void AddTestCaseMetaDataToBrowserStack(IWebDriver driver)
        {
            var testName = TestConText.Test.Name;
            ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionName\", \"arguments\": {\"name\":\" " + testName + " \"}}");
        }

        public void RecordTestCaseOutCome(IWebDriver driver)
        {
            var result = TestConText.Result.Outcome.Status;
            switch (result.ToString())
            {
                case "Passed":
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Test ran successfully!\"}}");
                    break;
                case "Failed":
                    var failerMessage = RecordFailureMessage();
                    Console.WriteLine(failerMessage);
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \""+ failerMessage +"\"}}");
                    break;
                case "Skipped":
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"skipped\", \"reason\": \" Test skipped due to some reason\"}}");
                    break;
                default:
                    break;
            }
        }

        public string ExtractAssertionFailureMessage(string message)
        {
            var finalMessage = message.Split("\n");
            return $"Assertion Failed: {finalMessage[0]}";
        }

        public string ExtractSeleniumFaliureMessage(string message)
        {
            var finalMessage = message.Split(":");
            return finalMessage[0];
        }

        public string RecordFailureMessage()
        {
            var failerMessage = TestConText.Result.Message;
            if (failerMessage.Contains("Expected")) return ExtractAssertionFailureMessage(failerMessage);
            else if (failerMessage.Contains("OpenQA")) return ExtractSeleniumFaliureMessage(failerMessage);
            else return failerMessage;
        }
    }
}