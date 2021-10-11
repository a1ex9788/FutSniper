using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIFA22Trader
{
    public static class SeleniumFinder
    {
        public static async Task<IWebElement> FindHtmlElementByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null, int retries = 30)
        {
            return await FindHtmlElement(searchContext, $".//*[@class='{wantedClass}']", retryMessage, retries);
        }

        public static async Task<IWebElement> FindHtmlElement(ISearchContext searchContext, string xPathSentence, string retryMessage = null, int retries = 30)
        {
            IEnumerable<IWebElement> htmlElements = await FindHtmlElements(searchContext, xPathSentence, retryMessage, retries);

            return htmlElements?.FirstOrDefault();
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElementsByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null, int retries = 30)
        {
            return await FindHtmlElements(searchContext, $".//*[@class='{wantedClass}']", retryMessage, retries);
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElements(ISearchContext searchContext, string xPathSentence, string retryMessage = null, int retries = 30)
        {
            IEnumerable<IWebElement> htmlElements = null;

            int currentReties = 0;

            do
            {
                try
                {
                    htmlElements = searchContext.FindElements(By.XPath(xPathSentence));

                    if (htmlElements != null && htmlElements.Any())
                    {
                        return htmlElements;
                    }
                }
                catch
                {
                    if (retryMessage != null)
                    {
                        Console.Error.WriteLine(retryMessage);
                    }
                }

                if (currentReties++ == retries)
                {
                    throw new Exception($"The retries limit has been reached.");
                }

                await Task.Delay(500);
            }
            while (htmlElements == null || !htmlElements.Any());

            return htmlElements;
        }
    }
}