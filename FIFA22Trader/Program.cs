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

                await WaitForSingIn(browser);

                await EnterTransfersMarket(browser);

                await FindWantedPlayer(browser);

                await SetMaximumPrice(browser);

                while (true)
                {
                    await MakeSearch(browser);

                    await ShowSearchResultsPurchasePrices(browser);

                    await ExitFromSearchResults(browser);
                }
            }
            catch (Exception e)
            {
                // TODO: Protect all null references.
                Console.Error.WriteLine($"An unexpected error occurred: {e.Message}");
            }
            finally
            {
                browser?.Dispose();
            }
        }

        private static async Task WaitForSingIn(IWebDriver browser)
        {
            Console.WriteLine("Waiting for sing in...");

            IWebElement singInButton = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-standard call-to-action");

            singInButton.Click();

            await SeleniumFinder.FindHtmlElementByClass(browser, "title", retryMessage: "Login not completed yet. Please, sing in.");

            Console.WriteLine("Sing in completed successfully. Main page reached.");
        }

        private static async Task EnterTransfersMarket(IWebDriver browser)
        {
            IWebElement transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElementByClass(browser, "ut-tab-bar-item icon-transfer");

            transfersMarketMainMenuButton.Click();

            IWebElement transfersMarketSearchButton = await SeleniumFinder.FindHtmlElementByClass(browser, "tile col-1-1 ut-tile-transfer-market");

            transfersMarketSearchButton.Click();
        }

        private static async Task FindWantedPlayer(IWebDriver browser)
        {
            string wantedPlayer = ConfigurationManager.AppSettings.Get("WantedPlayer");

            IWebElement playerNameInput = await SeleniumFinder.FindHtmlElementByClass(browser, "ut-text-input-control");

            playerNameInput.SendKeys(wantedPlayer);

            IWebElement wantedPlayerSelector = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-text");

            wantedPlayerSelector.Click();
        }

        private static async Task SetMaximumPrice(IWebDriver browser)
        {
            string maxPurchasePrice = ConfigurationManager.AppSettings.Get("MaxPurchasePrice");

            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElementsByClass(browser, "price-filter");

            IWebElement maxPurchasePriceFilterDiv = priceFilterDivs.ElementAt(3);

            IWebElement maxPriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "numericInput");

            maxPriceNumericInput.SendKeys(maxPurchasePrice.ToString());
        }

        private static async Task MakeSearch(IWebDriver browser)
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-standard call-to-action");

            searchButton.Click();
        }

        private static async Task ShowSearchResultsPurchasePrices(IWebDriver browser)
        {
            IEnumerable<IWebElement> foundedPlayers = null;

            try
            {
                foundedPlayers = await SeleniumFinder.FindHtmlElementsByClass(browser, "listFUTItem has-auction-data", retries: 3);
            }
            catch
            {
            }

            if (foundedPlayers == null || !foundedPlayers.Any())
            {
                Console.Error.WriteLine("No players found.");

                return;
            }

            Console.WriteLine("\nSearch results:");

            foreach (IWebElement foundedPlayer in foundedPlayers)
            {
                IEnumerable<IWebElement> currencyCoinsValueLabels = await SeleniumFinder.FindHtmlElementsByClass(foundedPlayer, "currency-coins value");

                IWebElement foundedPlayerPurchasePriceLabel = currencyCoinsValueLabels.ElementAt(2);

                string foundedPlayerPurchasePrice = foundedPlayerPurchasePriceLabel.Text;

                Console.WriteLine($"\t- {foundedPlayerPurchasePrice}");
            }
        }

        private static async Task ExitFromSearchResults(IWebDriver browser)
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(browser, "ut-navigation-button-control");

            searchButton.Click();
        }
    }
}