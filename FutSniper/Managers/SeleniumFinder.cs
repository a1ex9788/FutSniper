using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutSniper.Managers
{
    public static class SeleniumFinder
    {
        public static async Task<IWebElement> FindHtmlElement(ISearchContext searchContext, string xPathSentence, string retryMessage = null, int retries = 0)
        {
            IEnumerable<IWebElement> htmlElements = await FindHtmlElements(searchContext, xPathSentence, retryMessage, retries);

            return htmlElements?.FirstOrDefault();
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElements(ISearchContext searchContext, string xPathSentence, string retryMessage = null, int retries = 0)
        {
            int currentRetry = 0;

            do
            {
                IEnumerable<IWebElement> htmlElements = searchContext.FindElements(By.XPath(xPathSentence));

                if (htmlElements != null && htmlElements.Any())
                {
                    return htmlElements;
                }

                if (retryMessage != null)
                {
                    Console.Error.WriteLine(retryMessage);
                }

                await Task.Delay(1000);
            }
            while (currentRetry++ < retries);

            throw new NotFoundException($"Html element was not found with {retries} retries.");
        }

        public static async Task<IWebElement> FindHtmlElementByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null, int retries = 0)
        {
            return await FindHtmlElement(searchContext, $".//*[@class='{wantedClass}']", retryMessage, retries);
        }

        public static async Task<IEnumerable<IWebElement>> FindHtmlElementsByClass(ISearchContext searchContext, string wantedClass, string retryMessage = null, int retries = 0)
        {
            return await FindHtmlElements(searchContext, $".//*[@class='{wantedClass}']", retryMessage, retries);
        }

        public static async Task<IEnumerable<IWebElement>> FindChildElements(ISearchContext searchContext, string retryMessage = null, int retries = 0)
        {
            return await FindHtmlElements(searchContext, ".//*", retryMessage, retries);
        }
    }
}