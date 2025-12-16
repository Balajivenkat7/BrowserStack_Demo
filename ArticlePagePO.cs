// Created Date: 14 Jan 2025
// Created By: Balaji Venkatesan

/* This class holds only specific element 
   locators and methods of Article and longform page
   which can be used only for Desktop */

using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.DevTools.V129.WebAuthn;
using UX_Automation.PageObjects.Common;

namespace UX_Automation.PageObjects.Desktop
{
    public class ArticlePagePO : CommonArticlePagePO
    {
        public IWebElement RelatedVideoPlayBtn => FindElement(_driver, By.XPath("//div[@data-module= 'related-videos']/descendant::div[@class='amp-ads']/following-sibling::button[@data-rh= 'Play']"));
        public IWebElement RelatedVideoUnMuteIcon => FindElement(_driver, By.XPath("//div[@data-module= 'related-videos']/descendant::div[@class='amp-unmute-layer']/button[@data-rh= 'Unmute']"));
        public IWebElement RelatedVideoMuteBtn => FindElement(_driver, By.XPath("//div[@data-module= 'related-videos']/descendant::button[@aria-label= 'Mute']"));
        public IWebElement RelatedVideoPanelUnMuteBtn => FindElement(_driver, By.XPath("//div[@data-module= 'related-videos']/descendant::div[@class= 'amp-time-display']/following-sibling::button[@data-rh= 'Unmute']"));
        public IWebElement ArticleCloseBtn => FindElement(_driver, By.CssSelector("a[class= 'article__close']"));
        public IWebElement FaceBookIcon => FindElement(_driver, By.CssSelector("[class= 'article-sharing__icon']>[alt= 'Facebook']"));

        public ArticlePagePO(IWebDriver driver) : base(driver)
        {
            _driver = driver;
        }

        public ArticlePagePO ValidateArticleVideoPlayFunction()
        {
            AssertArticleVideoPlayAfterPreRollFinishes();
            RefreshPage();
            WaitForPageLoadCompletely();
            SkipAdsInArticle();
            AssertVideoPlaysInArticle();
            return this;
        }

        public ArticlePagePO ValidateRelatedVideoPlayMuted()
        {
            ScrollToElement(RelatedVideoPlayBtn);
            RelatedVideoPlayBtn.Click();
            WaitForElementVisiblity(By.XPath("//div[@data-module= 'related-videos']/descendant::div[contains(@class, 'amp-vod amp-medium-video amp-ad-break')]"), 60);
            WaitUntilInvisiblityOfElement(By.XPath("//div[@data-module= 'related-videos']/descendant::div[contains(@class, 'amp-vod amp-medium-video amp-ad-break')]"), 60);
            WaitForElementVisiblity(By.XPath("//div[@data-module= 'related-videos']/descendant::div[contains(@class, 'amp-vod amp-medium-video amp-playing')]"), 60);
            Assert.IsTrue(RelatedVideoUnMuteIcon.Displayed, "Related Video is not muted by default");
            return this;
        }

        public ArticlePagePO ValidateRelatedVideoPlayUnMuted()
        {
            RelatedVideoUnMuteIcon.Click();
            Assert.IsTrue(RelatedVideoMuteBtn.Displayed, "Mute button is not displayed even after Unmuting the video");
            var videoPlayMode = _driver.FindElement(By.XPath("//div[@data-module= 'related-videos']/descendant::div[contains(@class, 'amp-vod amp-medium-video amp-playing')]/div[1]")).GetDomAttribute("class");
            Assert.IsTrue(!videoPlayMode.Contains("amp-playing amp-muted"), "Video is muted");
          //  WaitForElementVisiblity(By.XPath("//div[@data-module= 'related-videos']/descendant::div[contains(@class, 'amp-vod amp-medium-video amp-ad-break')]"), 60);
            var relatedVideoContainer = _driver.FindElement(By.XPath("//div[@data-module= 'related-videos']/descendant::div[@class= 'video__player-container']"));
            MoveToElement(relatedVideoContainer);
            if (IsElementDisplayed(By.XPath("//div[@data-module= 'related-videos']/descendant::div[@class= 'amp-time-display']/following-sibling::button[@data-rh= 'Unmute']"))) RelatedVideoPanelUnMuteBtn.Click();
            return this;
        }

        public ArticlePagePO ValidatePageAutoRefresh()
        {
            var reloadStatus = WaitForLongTimeAndAssertReLoad(7);
            AssertPageAutoRefresh(reloadStatus);
            return this;
        }

        public ArticlePagePO ValidateClosingArticleNavigateBackToPreviousPage()
        {
            var articleName = _driver.FindElement(By.CssSelector("article[class= 'article']")).GetDomAttribute("data-title");
            AssertArticleDisplayedIsCorrect(articleName);
            ClickOnCloseArticleButton();
            WaitUntilInvisiblityOfElement(By.XPath($"//article[@class= 'article']/descendant::h1[text()= '{articleName}']"), 30);
            AssertClosingArticleRedirectCorrectUrl();
            return this;
        }

        public void ClickOnCloseArticleButton()
        {
            ArticleCloseBtn.Click();
        }

        public void ClickonSecondArticleCloseButton()
        {
            var secondArticleCloseBtn = _driver.FindElement(By.XPath("(//a[@class= 'article__close'])[2]"));
            ScrollToElement(secondArticleCloseBtn);
            secondArticleCloseBtn.Click();
        }

        public ArticlePagePO ValidateScrollingDownArticleAndClosingIt()
        {
            WaitForElementVisiblity(By.CssSelector("article[class= 'article']"), 30);
            ScrollDownToArticle(2);
            ClickonSecondArticleCloseButton();
            AssertClosingArticleRedirectCorrectUrl();
            return this;
        }

        public ArticlePagePO ValidateMainMediaAssetImageCaptionCredit()
        {
            Assert.IsTrue(ArticleMainAsset.GetDomAttribute("data-module") == "photo", "Main media asset doesn't contains photo");
            ScrollToElement(ArticleMainAssetPhotoCredit);
            Assert.IsTrue(ArticleMainAssetPhotoCredit.Displayed && ArticleMainAssetPhotoCredit.Text != "", "Main Asset Photo credit is not displayed or Photo credit is empty");
            Assert.IsTrue(ArticleMainAssetPhotoCaption.Displayed && ArticleMainAssetPhotoCaption.Text != "", "Main Asset Photo caption is not displayed or Photo caption is empty");
            return this;
        }

        public ArticlePagePO ValidateStoryBodyImageCaptionCredit()
        {
            ScrollToElement(ArticleStoryBody);
            Assert.IsTrue(ArticleStoryBody.GetDomAttribute("data-module") == "photo", "Story Body of article dosen't contain image");
            ScrollToElement(ArticleStoryBodyCaption);
            Assert.IsTrue(ArticleStoryBodyCaption.Displayed && ArticleStoryBodyCaption.Text != "", "Story body caption is not displayed or empty");
            Assert.IsTrue(ArticleStoryBodyCredit.Displayed && ArticleStoryBodyCredit.Text != "", "Story Body credit is not displayed or empty");
            return this;
        }

        public ArticlePagePO ValidateMainMediaAssetImageCreditDisplayButNotCaption()
        {
            Assert.IsTrue(ArticleMainAsset.GetDomAttribute("data-module") == "photo", "Main media asset doesn't contains photo");
            ScrollToElement(ArticleMainAssetPhotoCredit);
            Assert.IsTrue(ArticleMainAssetPhotoCredit.Displayed && ArticleMainAssetPhotoCredit.Text != "", "Main Asset Photo credit is not displayed or Photo credit is empty");
            Assert.IsFalse(IsElementDisplayed(By.XPath("//div[@class= 'article__lead-asset']/descendant::div[@class= 'photo__caption']")), "Main media image caption is displayed");
            return this;
        }
        
        public ArticlePagePO ValidateStoryBodyImageCreditDisplayButNotCaption()
        {
            ScrollToElement(ArticleStoryBody);
            Assert.IsTrue(ArticleStoryBody.GetDomAttribute("data-module") == "photo", "Story Body of article dosen't contain image");
            ScrollToElement(ArticleStoryBodyCredit);
            Assert.IsTrue(ArticleStoryBodyCredit.Displayed && ArticleStoryBodyCredit.Text != "", "Story body credit is not displayed or empty");
            Assert.IsFalse(IsElementDisplayed(By.XPath("//div[@class= 'article__body']/descendant::div[@class= 'photo__caption']")), "Story body image caption is displayed");
            return this;
        }

        public ArticlePagePO ValidateUGCVideoPlayWithoutPreRoll()
        {
            MoveToElement(VideoPlayBtn);
            VideoPlayBtn.Click();
            WaitForElementVisiblity(By.XPath("//div[@class= 'video__player amp-html5 amp-player amp-desktop amp-autoplay amp-vod amp-medium-video amp-controls-auto amp-playing']"), 30);
            var videoPlayer = _driver.FindElement(By.CssSelector("[class^= 'video__player amp-html5 amp-player']")).GetDomAttribute("class");
            Assert.IsTrue(!videoPlayer.Contains("amp-ad-break"), $"Pre roll ad plays on UGC video");
            return this;
        }

        public ArticlePagePO ValidateCaptionWorksOnEnbaleAndDisable()
        {
            MoveToElement(ArticleMainAsset);
            MainAssetCCBtn.Click();
            Assert.IsTrue(MainAssetCCBtn.GetDomAttribute("aria-checked") == "true", "Main Asset cc button is not checked even after clicking on it");
            Assert.IsTrue(MainAssetVideoPlayer.GetDomAttribute("class").Contains("amp-cc-active"), "Captions are not displayed in video player after enabling cc button");
            MoveToElement(ArticleMainAsset);
            MainAssetCCBtn.Click();
            Assert.IsTrue(MainAssetCCBtn.GetDomAttribute("aria-checked") == "false", "Main Asset cc button is still checked after disabling it");
            Assert.IsTrue(!MainAssetVideoPlayer.GetDomAttribute("class").Contains("amp-cc-active"), "Captions are still displayed in video player after disabling cc button");
            return this;
        }

        public ArticlePagePO ValidateEndSlateReturnToVideoReplayOnEscapeBtn()
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 1, 0));
            wait.Until(drv => drv.FindElement(By.CssSelector("div[class^= 'video__player amp-html5']")).GetDomAttribute("aria-hidden") == "true");
            Actions action = new Actions(_driver);
            var endslateThumbnail = _driver.FindElements(By.CssSelector("img[class= 'video__endslate-thumbnail']")).ToList();
            action.MoveToElement(endslateThumbnail[0]).SendKeys(Keys.Tab).SendKeys(Keys.Escape).Build().Perform();
            Assert.IsTrue(ReplayBtn.Displayed, "Replay btn is not displayed");
            return this;
        }       

        public CommonArticlePagePO ClickOnFacbookIcon()
        {
            ScrollToElement(FaceBookIcon);
            FaceBookIcon.Click();
            return this;
        }
    }
}
