using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using AngleSharp.Dom;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace WebScraper.Services
{
    public class SeleniumWorker
    {
        private IWebDriver webDriver;
        public SeleniumWorker()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            webDriver = new ChromeDriver(chromeOptions);
        }
        public List<string> GetPhemexNames()
        {
            var names = new List<string>();
            var uri = "https://phemex.com/trade/BTCUSDT";
            webDriver.Navigate().GoToUrl(uri);
            webDriver.FindElement(By.CssSelector("#app > div > div > main > div > header > div > h1 > div > div.symbol.df.aic.f16.fw3.wsn.T1 > i"), 15).Click();
            webDriver.FindElement(By.CssSelector("body > div.wrap.H-5pak8m > div > div > div.body.sv.H-5pak8m > div > div.header.ps.H-embllz > div.wrap.pr.df.ph16.ooo.ovh.sv.H-1sa5lp8 > div.list.df.H-1sa5lp8 > span.btn.T3.fw1.cp.H-1sa5lp8.active"), 15).Click();
            Thread.Sleep(3500);
            int increment = 1;
            try
            {
                while (true)
                {
                    names.Add(webDriver.FindElement(By.CssSelector($"body > div.wrap.H-5pak8m > div > div > div.body.sv.H-5pak8m > div > div.body > div:nth-child({increment}) > div.df > div > div.key.df.aic.H-xxy0b1 > div > span"), 5).Text);
                    increment++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return names;
        }

        public void QuitDriver()
        {
            webDriver.Close();
            webDriver.Quit();
        }
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }

    }
}
