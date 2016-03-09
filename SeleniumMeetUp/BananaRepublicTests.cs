using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMeetUp
{
    [TestClass]
    public class BananaRepublicTests
    {
        const string AppLoadingProgressBarId = "com.skava.hybridapp.bananarepublic:id/progressbar";
        const string SearchIconId = "gm_searchIcon_id";
        const string SearchBoxId = "gm_title_searchbox_input";
        const string SearchString = "shirts & tees";
        const string FirstItemInSearchSuggestionId = "gm_search_suggestion_0";
        const string SearchResultsTitleXPath = "//*[@id=\"id_titleInnerDiv\"]";

        AndroidDriver<IWebElement> driver;
        WebDriverWait wait;

        [TestInitialize]
        public void BeforeAll()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();

            //capabilities.App = "<your-app-file>";
            //capabilities.AutoWebView = true;
            //capabilities.AutomationName = "";
            //capabilities.BrowserName = String.Empty; // Leave empty otherwise you test on browsers
            //capabilities.DeviceName = "Needed if testing on IOS on a specific device. This will be the UDID";
            //capabilities.FwkVersion = "1.0"; // Not really needed
            //capabilities.Platform = TestCapabilities.DevicePlatform.Android; // Or IOS
            //capabilities.PlatformVersion = String.Empty; // Not really needed

            capabilities.SetCapability(MobileCapabilityType.App, Path.GetFullPath(@"APK\BR_v1.7.2.apk"));
            capabilities.SetCapability(MobileCapabilityType.NewCommandTimeout, 120);
            capabilities.SetCapability(MobileCapabilityType.DeviceName, "169.254.76.233:5555");
            capabilities.SetCapability(MobileCapabilityType.AppPackage, "com.skava.hybridapp.bananarepublic");
            capabilities.SetCapability(MobileCapabilityType.AppActivity, ".BananaRepublicActivity");

            driver = new AndroidDriver<IWebElement>(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);

            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(AppLoadingProgressBarId)));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id(AppLoadingProgressBarId)));
        }

        [TestCleanup]
        public void AfterAll()
        {
            driver.Quit();
        }

        [TestMethod]
        public void SearchTest()
        {
            foreach (string ctx in driver.Contexts)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}]: {1}", "Context found", ctx));
            }

            this.driver.Context = "WEBVIEW";

            System.Diagnostics.Debug.WriteLine(string.Format("[{0}]: {1}", "1", DateTime.Now.ToString("hh:mm:ss")));
            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(15));

            System.Diagnostics.Debug.WriteLine(string.Format("[{0}]: {1}", "2", DateTime.Now.ToString("hh:mm:ss")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SearchIconId)));
            this.driver.FindElementById(SearchIconId).Click();

            System.Diagnostics.Debug.WriteLine(string.Format("[{0}]: {1}", "3", DateTime.Now.ToString("hh:mm:ss")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SearchBoxId)));
            this.driver.FindElementById(SearchBoxId).SendKeys(SearchString);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(FirstItemInSearchSuggestionId)));
            this.driver.FindElementById(FirstItemInSearchSuggestionId).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SearchResultsTitleXPath)));

            SwipeUp();
            this.driver.Context = "WEBVIEW";

            Assert.IsTrue(this.driver.FindElementByXPath(SearchResultsTitleXPath).Text == "Results for shirts & tees");
        }

        public void SwipeUp()
        {
            this.driver.Context = "NATIVE_APP";
            driver.Swipe(504, 1319, 504, 594, 800); //this action includes almost all touch actions
        }

        public void SwipeDown()
        {
            this.driver.Context = "NATIVE_APP";
            IList<IWebElement> els = this.driver.FindElementsByClassName("android.widget.TextView");
            var loc1 = els[3].Location;
            IWebElement target = els[1];
            var loc2 = target.Location;
            driver.Swipe(504, 594, 504, 1319, 800); //this action includes almost all touch actions
        }
    }
}
