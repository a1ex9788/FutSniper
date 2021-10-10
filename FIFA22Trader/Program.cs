using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FIFA22Trader
{
    public class Program
    {
        public async static Task Main()
        {
            IWebDriver browser = null;

            try
            {
                // TODO: Kill Chome Driver process when application stops.
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                browser = new ChromeDriver(service)
                {
                    Url = "https://www.ea.com/es-es/fifa/ultimate-team/web-app/",
                };

                //browser.Manage().Window.Maximize();

                Console.WriteLine("FIFA 22 Trader started.");

                Console.WriteLine("Waiting for sing in...");

                await WaitForSingIn(browser);

                Console.WriteLine("Sing in completed successfully. Main page reached.");

                await EnterTransfersMarket(browser);

                await FindWantedPlayer(browser);

                await SetMaximumPrice(browser);

                await MakeSearch(browser);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"An unexpected error occurred: {e.Message}");
            }
            finally
            {
                browser?.Dispose();
            }
        }

        private static async Task WaitForSingIn(IWebDriver browser)
        {
            IWebElement singInButton = await SeleniumFinder.FindHtmlElement(browser, "class='btn-standard call-to-action'", "Login not completed yet. Please, sing in.");

            singInButton.Click();

            await SeleniumFinder.FindHtmlElement(browser, "class='title'", "Main page not reached yet. Please, sing in.");
        }

        private static async Task EnterTransfersMarket(IWebDriver browser)
        {
            IWebElement transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElement(browser, "class='ut-tab-bar-item icon-transfer'");

            transfersMarketMainMenuButton.Click();

            IWebElement transfersMarketSearchButton = await SeleniumFinder.FindHtmlElement(browser, "class='tile col-1-1 ut-tile-transfer-market'");

            transfersMarketSearchButton.Click();
        }

        private static async Task FindWantedPlayer(IWebDriver browser)
        {
            string wantedPlayer = ConfigurationManager.AppSettings.Get("WantedPlayer");

            IWebElement playerNameInput = await SeleniumFinder.FindHtmlElement(browser, "class='ut-text-input-control'");

            playerNameInput.SendKeys(wantedPlayer);

            IWebElement wantedPlayerSelector = await SeleniumFinder.FindHtmlElement(browser, "class='btn-text'");

            wantedPlayerSelector.Click();
        }

        private static async Task SetMaximumPrice(IWebDriver browser)
        {
            string maxPurchasePrice = ConfigurationManager.AppSettings.Get("MaxPurchasePrice");

            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElements(browser, "class='price-filter'");

            IWebElement maxPurchasePriceFilterDiv = priceFilterDivs.ElementAt(3);

            IWebElement maxPriceNumericInput = await SeleniumFinder.FindHtmlElement(maxPurchasePriceFilterDiv, "class='numericInput'");

            maxPriceNumericInput.SendKeys(maxPurchasePrice.ToString());
        }

        private static async Task MakeSearch(IWebDriver browser)
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElement(browser, "class='btn-standard call-to-action'");

            searchButton.Click();
        }
    }
}