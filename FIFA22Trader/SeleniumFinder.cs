using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIFA22Trader
{
    public static class SeleniumFinder
    {
        public static async Task<IWebElement> FindHtmlElement(ISearchContext browser, string xPathSentence, string errorMessage = null)
        {
            IWebElement htmlElement = null;

            do
            {
                try
                {
                    htmlElement = browser.FindElement(By.XPath($".//*[@{xPathSentence}]"));

                    break;
                }
                catch
                {
                    if (errorMessage != null)
                    {
                        Console.Error.WriteLine(errorMessage);
                    }
                }

                await Task.Delay(1000);
            }
            while (htmlElement == null);

            return htmlElement;
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElements(IWebDriver browser, string xPathSentence, string errorMessage = null)
        {
            IEnumerable<IWebElement> htmlElements = null;

            do
            {
                try
                {
                    htmlElements = browser.FindElements(By.XPath($".//*[@{xPathSentence}]"));

                    break;
                }
                catch
                {
                    if (errorMessage != null)
                    {
                        Console.Error.WriteLine(errorMessage);
                    }
                }

                await Task.Delay(1000);
            }
            while (htmlElements == null);

            return htmlElements;
        }
    }
}