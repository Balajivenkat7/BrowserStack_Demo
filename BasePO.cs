//Created Date: 13 Dec 2024
//Created By: Balaji Venkatesan

/*  This class contains all the base selenium methods
    which can be used across all page object classes
    All Page object class will inherit BasePO class.  */

using UX_Automation.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace UX_Automation.PageObjects
{
    public class BasePO
    {
        protected IWebDriver Driver { get; set; }
        private TestDataParser testData;
        private int timeoutInSeconds { get; set; }

        public BasePO(IWebDriver driver)
        {
            Driver = driver;
            testData = new TestDataParser();
            timeoutInSeconds = testData.GetDefaultTimeOutInSeconds();
        }

        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }

        public void NavigateBack()
        {
            Driver.Navigate().Back();
        }

        public void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public IWebElement FindElement(IWebDriver _driver, By by)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new DefaultWait<IWebDriver>(_driver)
                { PollingInterval = TimeSpan.FromSeconds(2), Timeout = TimeSpan.FromSeconds(timeoutInSeconds) };
                wait.IgnoreExceptionTypes(typeof(WebDriverException), typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                return wait.Until(drv => drv.FindElement(by));
            }
            return _driver.FindElement(by);
        }

        public void MoveToElement(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(element).Build().Perform();
        }

        public void MoveToElementAndClick(IWebElement element)
        {
            Actions action = new Actions(Driver);
            var clickElement = action.MoveToElement(element).Click().Build();
            clickElement.Perform();
        }

        public void MoveToElementAndClear(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(element).Build().Perform();
            element.Clear();
        }

        public void DoubleClickOnElement(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.DoubleClick(element).Build().Perform();
        }

        public void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public void AcceptAlert()
        {
            IAlert alert = Driver.SwitchTo().Alert();
            alert.Accept();
        }

        public void DismissAlert()
        {
            IAlert alert = Driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        public void WaitForElementVisiblity(By by, int timeout)
        {
            var wait = new DefaultWait<IWebDriver>(Driver)
            {
                PollingInterval = TimeSpan.FromSeconds(1),
                Timeout = TimeSpan.FromSeconds(timeout)
            };
            wait.IgnoreExceptionTypes(typeof(WebDriverException), typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public void WaitForElementTobeClickable(IWebElement element, int timeout)
        {
            var wait = new DefaultWait<IWebDriver>(Driver)
            {
                PollingInterval = TimeSpan.FromSeconds(1),
                Timeout = TimeSpan.FromSeconds(timeout)
            };
            wait.IgnoreExceptionTypes(typeof(WebDriverException), typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public void WaitForElementPresence(By by, int timeout)
        {
            var wait = new DefaultWait<IWebDriver>(Driver)
            {
                PollingInterval = TimeSpan.FromSeconds(1),
                Timeout = TimeSpan.FromSeconds(timeout)
            };
            wait.IgnoreExceptionTypes(typeof(WebDriverException), typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
        }

        public void WaitUntilElementExist(By by, int timeout)
        {
            var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, timeout));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
        }

        public void WaitUntilInvisiblityOfElement(By by, int timeout)
        {
            var wait = new DefaultWait<IWebDriver>(Driver)
            {
                PollingInterval = TimeSpan.FromSeconds(1),
                Timeout = TimeSpan.FromSeconds(timeout)
            };
            wait.IgnoreExceptionTypes(typeof(WebDriverException), typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        public bool IsElementDisplayedByXpath(string xPath)
        {
            var result = false;
            var elements = Driver.FindElements(By.XPath(xPath)).ToList();
            if (elements.Count > 0) { result = true; }
            return result;
        }

        public bool IsElementDisplayed(By by)
        {
            var result = false;
            var element = Driver.FindElements(by).ToList();
            if (element.Count > 0) { result = true; }
            return result;
        }

        public bool IfElementClickable(By by)
        {
            try
            {
                var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 5));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void WaitForPageLoadCompletely()
        {
            var wait = new WebDriverWait(Driver, new TimeSpan(0, 5, 0));
            wait.Until(drv => ((IJavaScriptExecutor)drv).ExecuteScript("return document.readyState").Equals("complete"));
        }
        public void ClickAnElementByJs(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].click();", element);
        }
        
        public void ScrollToPageBottom()
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
    }
    }
}