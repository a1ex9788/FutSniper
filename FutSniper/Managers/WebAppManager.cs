using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutSniper.Managers
{
    public class WebAppManager : IDisposable
    {
        private readonly IWebDriver browser;

        public WebAppManager(bool maximizeWindow = false)
        {
            this.browser = new ChromeDriver()
            {
                Url = "https://www.ea.com/es-es/fifa/ultimate-team/web-app/",
            };

            if (maximizeWindow)
            {
                this.browser.Manage().Window.Maximize();
            }
        }

        public async Task WaitForSingIn()
        {
            IWebElement singInButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard call-to-action", retries: int.MaxValue);

            singInButton.Click();

            await SeleniumFinder.FindHtmlElementByClass(this.browser, "title", retryMessage: "Login not completed yet. Please, sing in.", retries: int.MaxValue);
        }

        public async Task EnterTransfersMarket()
        {
            IWebElement transfersMarketMainMenuButton;

            try
            {
                transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-tab-bar-item icon-transfer selected", retries: 0);
            }
            catch
            {
                transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-tab-bar-item icon-transfer", retries: 0);
            }

            transfersMarketMainMenuButton.Click();

            IWebElement transfersMarketSearchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "tile col-1-1 ut-tile-transfer-market");

            transfersMarketSearchButton.Click();
        }

        public async Task FindWantedPlayer(string wantedPlayer)
        {
            IWebElement playerNameInput = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-text-input-control");

            playerNameInput.Clear();
            playerNameInput.SendKeys(wantedPlayer);

            IWebElement wantedPlayerSelector = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-text");

            wantedPlayerSelector.Click();
        }

        public async Task SetMaximumPrice(int maxPurchasePrice)
        {
            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElementsByClass(this.browser, "price-filter");

            IWebElement maxPurchasePriceFilterDiv = priceFilterDivs.ElementAt(3);

            IWebElement maxPriceNumericInput;

            try
            {
                maxPriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "numericInput", retries: 0);
            }
            catch
            {
                maxPriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "numericInput filled", retries: 0);
            }

            maxPriceNumericInput.Clear();
            maxPriceNumericInput.SendKeys(maxPurchasePrice.ToString());
        }

        public async Task MakeSearch()
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard call-to-action", retries: 0);

            searchButton.Click();
        }

        public async Task<string> CheckPlayerPurchasePriceIfFounded()
        {
            IWebElement foundedPlayer;

            try
            {
                foundedPlayer = await SeleniumFinder.FindHtmlElementByClass(this.browser, "listFUTItem has-auction-data selected", retries: 3);
            }
            catch
            {
                return null;
            }

            IEnumerable<IWebElement> currencyCoinsValueLabels = await SeleniumFinder.FindHtmlElementsByClass(foundedPlayer, "currency-coins value");

            IWebElement foundedPlayerPurchasePriceLabel = currencyCoinsValueLabels.ElementAt(2);

            return foundedPlayerPurchasePriceLabel.Text;
        }

        public async Task<bool> TryToBuyPlayer()
        {
            try
            {
                IWebElement buyButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard buyButton currency-coins");

                buyButton.Click();

                IWebElement acceptPurchaseDialog = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ea-dialog-view ea-dialog-view-type--message");

                IWebElement acceptPurchaseDiv = await SeleniumFinder.FindHtmlElementByClass(acceptPurchaseDialog, "ut-button-group");

                IEnumerable<IWebElement> acceptPurchaseButtons = SeleniumFinder.FindChildElements(acceptPurchaseDiv);

                IWebElement acceptPurchaseButton = acceptPurchaseButtons.ElementAt(0);

                acceptPurchaseButton.Click();

                // TODO: Verify if the player was correctly bought. Check if it was expired, a red message is shown.
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task ExitFromSearchResults()
        {
            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-navigation-button-control");

            searchButton.Click();
        }

        public void Dispose()
        {
            this.browser?.Quit();
            this.browser?.Close();
            this.browser?.Dispose();
        }
    }
}