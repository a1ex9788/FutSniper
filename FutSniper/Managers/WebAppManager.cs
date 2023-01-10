using FutSniper.Entities;
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

        public bool IsActive()
        {
            try
            {
                string _ = this.browser.Title;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task SingIn(Credentials credentials)
        {
            // Wait a little in order to let the browser load correctly.
            await Task.Delay(3000);

            Console.WriteLine("Waiting for sing in...");

            IWebElement singInButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard call-to-action", retries: int.MaxValue);

            singInButton.Click();

            IWebElement rememberMeCheckBox = await SeleniumFinder.FindHtmlElementByClass(this.browser, "link-in-message", retries: int.MaxValue);

            rememberMeCheckBox.Click();

            if (credentials == null)
            {
                Console.WriteLine("Credentials do not have been provided. Please, log in.");

                return;
            }

            Console.WriteLine("Credentials have been provided so they are going to be written.");

            IWebElement usernameDiv = await SeleniumFinder.FindHtmlElementByClass(this.browser, "otkinput otkinput-grouped otkform-group-field input-margin-bottom-error-message", retries: int.MaxValue);

            IWebElement usernameInput = usernameDiv.FindElement(By.Name("email"));

            usernameInput.SendKeys(credentials.Username);

            IWebElement passwordDiv = await SeleniumFinder.FindHtmlElementByClass(this.browser, "otkinput otkinput-grouped input-margin-bottom-error-message");

            IWebElement passwordInput = passwordDiv.FindElement(By.Name("password"));

            passwordInput.SendKeys(credentials.Password);

            IWebElement logInButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "otkbtn otkbtn-primary ");

            logInButton.Click();

            IWebElement sendMessageButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "otkbtn otkbtn-primary  right", retries: int.MaxValue);

            sendMessageButton.Click();

            IWebElement rememberThisDeviceCheckBox = await SeleniumFinder.FindHtmlElementByClass(this.browser, "link-in-message", retries: int.MaxValue);

            rememberThisDeviceCheckBox.Click();
        }

        public async Task WaitForMainPage()
        {
            Console.WriteLine("Waiting for main page...");

            await SeleniumFinder.FindHtmlElementByClass(this.browser, "title", retryMessage: "Login not completed yet. Please, sing in.", retries: int.MaxValue);
        }

        public async Task EnterTransfersMarket()
        {
            Console.WriteLine("Entering transfers market...");

            IWebElement transfersMarketMainMenuButton;

            try
            {
                transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-tab-bar-item icon-transfer selected");
            }
            catch (NotFoundException)
            {
                transfersMarketMainMenuButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-tab-bar-item icon-transfer");
            }

            transfersMarketMainMenuButton.Click();

            IWebElement transfersMarketSearchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "tile col-1-1 ut-tile-transfer-market");

            transfersMarketSearchButton.Click();
        }

        public async Task FindWantedPlayer(string wantedPlayer)
        {
            Console.WriteLine("Finding wanted player...");

            IWebElement playerNameInput = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-text-input-control");

            playerNameInput.Clear();
            playerNameInput.SendKeys(wantedPlayer);

            IWebElement wantedPlayerSelector = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-text", retries: 1);

            wantedPlayerSelector.Click();
        }

        public async Task SetMaximumPurchasePrice(int maxPurchasePrice)
        {
            Console.WriteLine("Setting maximum purchase price...");

            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElementsByClass(this.browser, "price-filter");

            IWebElement maxPurchasePriceFilterDiv = priceFilterDivs.ElementAt(3);

            IWebElement maxPurchasePriceNumericInput;

            try
            {
                maxPurchasePriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "ut-number-input-control filled");
            }
            catch (NotFoundException)
            {
                maxPurchasePriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(maxPurchasePriceFilterDiv, "ut-number-input-control");
            }

            maxPurchasePriceNumericInput.Clear();
            maxPurchasePriceNumericInput.SendKeys(maxPurchasePrice.ToString());
        }

        public async Task ChangeMinimumPurchasePrice()
        {
            Console.WriteLine("Changing minimum purchase price...");

            IEnumerable<IWebElement> priceFilterDivs = await SeleniumFinder.FindHtmlElementsByClass(this.browser, "price-filter");

            IWebElement minPurchasePriceFilterDiv = priceFilterDivs.ElementAt(2);

            IWebElement minPurchasePriceNumericInput;

            try
            {
                minPurchasePriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(minPurchasePriceFilterDiv, "ut-number-input-control filled");
            }
            catch (NotFoundException)
            {
                minPurchasePriceNumericInput = await SeleniumFinder.FindHtmlElementByClass(minPurchasePriceFilterDiv, "ut-number-input-control");
            }

            string newMinPurchasePrice;

            switch (minPurchasePriceNumericInput.Text)
            {
                case "200":
                    newMinPurchasePrice = "250";
                    break;

                case "250":
                    newMinPurchasePrice = "300";
                    break;

                default:
                    newMinPurchasePrice = "200";
                    break;
            }

            minPurchasePriceNumericInput.SendKeys(newMinPurchasePrice);
        }

        public async Task MakeSearch()
        {
            Console.WriteLine("Making search...");

            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard call-to-action");

            searchButton.Click();
        }

        public async Task<string> CheckPlayerPurchasePriceIfFounded()
        {
            Console.WriteLine("Checking player purchase price...");

            IWebElement foundedPlayer;

            try
            {
                foundedPlayer = await SeleniumFinder.FindHtmlElementByClass(this.browser, "listFUTItem has-auction-data selected", retries: 3);
            }
            catch (NotFoundException)
            {
                return null;
            }

            IEnumerable<IWebElement> currencyCoinsValueLabels = await SeleniumFinder.FindHtmlElementsByClass(foundedPlayer, "currency-coins value");

            IWebElement foundedPlayerPurchasePriceLabel = currencyCoinsValueLabels.ElementAt(2);

            return foundedPlayerPurchasePriceLabel.Text;
        }

        public async Task<bool> TryToBuyPlayer()
        {
            Console.WriteLine("Trying to buy player...");

            try
            {
                IWebElement buyButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "btn-standard buyButton currency-coins");

                buyButton.Click();

                IWebElement acceptPurchaseDialog = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ea-dialog-view ea-dialog-view-type--message");

                IWebElement acceptPurchaseDiv = await SeleniumFinder.FindHtmlElementByClass(acceptPurchaseDialog, "ut-button-group");

                IEnumerable<IWebElement> acceptPurchaseButtons = await SeleniumFinder.FindChildElements(acceptPurchaseDiv);

                IWebElement acceptPurchaseButton = acceptPurchaseButtons.ElementAt(0);

                acceptPurchaseButton.Click();

                // TODO: Verify if the player was correctly bought. Check if it was expired, a red message is shown.
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        public async Task ExitFromSearchResults()
        {
            Console.WriteLine("Exiting from search results...");

            IWebElement searchButton = await SeleniumFinder.FindHtmlElementByClass(this.browser, "ut-navigation-button-control");

            searchButton.Click();
        }

        public void Dispose()
        {
            this.browser?.Dispose();
        }
    }
}