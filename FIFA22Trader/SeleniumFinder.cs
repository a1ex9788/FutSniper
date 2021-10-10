using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIFA22Trader
{
    public static class SeleniumFinder
    {
        public static async Task<IWebElement> FindHtmlElementByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null)
        {
            return await FindHtmlElement(searchContext, $".//*[@class='{wantedClass}']", retryMessage);
        }

        public static async Task<IWebElement> FindHtmlElement(ISearchContext searchContext, string xPathSentence, string retryMessage = null)
        {
            IWebElement htmlElement = null;

            do
            {
                try
                {
                    htmlElement = searchContext.FindElement(By.XPath(xPathSentence));

                    break;
                }
                catch
                {
                    if (retryMessage != null)
                    {
                        Console.Error.WriteLine(retryMessage);
                    }
                }

                await Task.Delay(1000);
            }
            while (htmlElement == null);

            return htmlElement;
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElementsByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null)
        {
            return await FindHtmlElements(searchContext, $".//*[@class='{wantedClass}']", retryMessage);
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElements(ISearchContext searchContext, string xPathSentence, string retryMessage = null)
        {
            IEnumerable<IWebElement> htmlElements = null;

            do
            {
                try
                {
                    htmlElements = searchContext.FindElements(By.XPath(xPathSentence));

                    break;
                }
                catch
                {
                    if (retryMessage != null)
                    {
                        Console.Error.WriteLine(retryMessage);
                    }
                }

                await Task.Delay(1000);
            }
            while (htmlElements == null);

            return htmlElements;
        }
    }
}