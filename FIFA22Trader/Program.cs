using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;

namespace WhatsAppConnectionMonitorer
{
    public class Program
    {
        public async static Task Main()
        {
            IWebDriver browser = null;

            try
            {
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
            IWebElement singInButton = null;

            do
            {
                try
                {
                    singInButton = browser.FindElement(By.XPath(".//*[@class='btn-standard call-to-action']"));

                    break;
                }
                catch
                {
                    Console.Error.WriteLine("Login not completed yet. Please, sing in.");
                }

                await Task.Delay(1000);
            }
            while (singInButton == null);

            singInButton.Click();

            IWebElement startMenuLabel = null;

            do
            {
                try
                {
                    startMenuLabel = browser.FindElement(By.XPath(".//*[@class='title']"));

                    break;
                }
                catch
                {
                    Console.Error.WriteLine("Main page not reached yet. Please, sing in.");
                }

                await Task.Delay(1000);
            }
            while (startMenuLabel == null);
        }
    }
}