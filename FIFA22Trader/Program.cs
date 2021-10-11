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

                while (true)
                {
                    try
                    {
                        await FindWantedPlayer(browser);

                        await SetMaximumPrice(browser);

                        await MakeSearch(browser);

                        try
                        {
                            await BuyPlayerIfFounded(browser);
                        }
                        catch
                        {
                        }

                        await ExitFromSearchResults(browser);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
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

            await SeleniumFinder.FindHtmlElementByClass(browser, "title", retryMessage: "Login not completed yet. Please, sing in.", retries: int.MaxValue);

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

            playerNameInput.Clear();
            playerNameInput.SendKeys(wantedPlayer);

            IWebElement wantedPlayerSelector = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-text");

            wantedPlayerSelector.Click();
        }

        private static async Task SetMaximumPrice(IWebDriver browser)
        {
            int maxPurchasePrice = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxPurchasePrice"));

            // Changing the price used in order to avoid results caching.
            if (ProbabilityGetter.GetHalfProbability())
            {
                maxPurchasePrice -= 100;
            }

            if (maxPurchasePrice < 200)
            {
                maxPurchasePrice = 200;
            }

            ConfigurationManager.RefreshSection("appSettings");

            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElementsByClass(browser, "price-filter");

            IWebElement maxPurchasePriceFilterDiv = priceFilterDivs.ElementAt(3);

            IWebElement maxPriceNumericInput;

            try
            {
                maxPriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "numericInput", retries: 0);
            }
            catch
            {
                maxPriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "numericInput filled");
            }

            maxPriceNumericInput.Clear();
            maxPriceNumericInput.SendKeys(maxPurchasePrice.ToString());
        }

        private static async Task MakeSearch(IWebDriver browser)
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-standard call-to-action");

            searchButton.Click();
        }

        private static async Task BuyPlayerIfFounded(IWebDriver browser)
        {
            IWebElement foundedPlayer;

            try
            {
                foundedPlayer = await SeleniumFinder.FindHtmlElementByClass(browser, "listFUTItem has-auction-data selected", retries: 3);
            }
            catch
            {
                Console.Error.WriteLine($"[{DateTime.Now}]: No players found.");

                return;
            }

            IEnumerable<IWebElement> currencyCoinsValueLabels = await SeleniumFinder.FindHtmlElementsByClass(foundedPlayer, "currency-coins value");

            IWebElement foundedPlayerPurchasePriceLabel = currencyCoinsValueLabels.ElementAt(2);

            string foundedPlayerPurchasePrice = foundedPlayerPurchasePriceLabel.Text;

            Console.WriteLine($"[{DateTime.Now}]: Player founded - {foundedPlayerPurchasePrice} coins");

            IWebElement buyButton = await SeleniumFinder.FindHtmlElementByClass(browser, "btn-standard buyButton currency-coins");

            buyButton.Click();

            IWebElement acceptPurchaseDialog = await SeleniumFinder.FindHtmlElementByClass(browser, "ea-dialog-view ea-dialog-view-type--message");

            IWebElement acceptPurchaseDiv = await SeleniumFinder.FindHtmlElementByClass(acceptPurchaseDialog, "ut-button-group");

            IEnumerable<IWebElement> acceptPurchaseButtons = SeleniumFinder.FindChildElements(acceptPurchaseDiv);

            IWebElement acceptPurchaseButton = acceptPurchaseButtons.ElementAt(0);

            acceptPurchaseButton.Click();

            Console.Beep();
            Console.WriteLine($"[{DateTime.Now}]: Player bought for {foundedPlayerPurchasePrice} coins");
        }

        private static async Task ExitFromSearchResults(IWebDriver browser)
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(browser, "ut-navigation-button-control");

            searchButton.Click();
        }
    }
}