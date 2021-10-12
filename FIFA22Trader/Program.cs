using System;
using System.Threading.Tasks;

namespace FIFA22Trader
{
    public class Program
    {
        public async static Task Main()
        {
            FIFA22WebAppManager fIFA22WebAppManager = null;

            try
            {
                Console.WriteLine("FIFA 22 Trader started.");

                fIFA22WebAppManager = new FIFA22WebAppManager();

                await MainImplementation(fIFA22WebAppManager);
            }
            catch (Exception e)
            {
                // TODO: Protect all null references.
                Console.Error.WriteLine($"An unexpected error occurred: {e.Message}");
            }
            finally
            {
                fIFA22WebAppManager?.Dispose();
            }
        }

        private static async Task MainImplementation(FIFA22WebAppManager fIFA22WebAppManager)
        {
            Console.WriteLine("Waiting for sing in...");

            await fIFA22WebAppManager.WaitForSingIn();

            Console.WriteLine("Sing in completed successfully. Main page reached.");

            await fIFA22WebAppManager.EnterTransfersMarket();

            while (true)
            {
                try
                {
                    await FindPlayerAndTryToBuyIt(fIFA22WebAppManager);

                    await fIFA22WebAppManager.ExitFromSearchResults();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    await fIFA22WebAppManager.EnterTransfersMarket();
                }
            }
        }

        private static async Task FindPlayerAndTryToBuyIt(FIFA22WebAppManager fIFA22WebAppManager)
        {
            (string Name, int MaxPurchasePrice) wantedPlayer = WantedPlayersObtainer.GetWantedPlayer();

            await fIFA22WebAppManager.FindWantedPlayer(wantedPlayer.Name);

            await fIFA22WebAppManager.SetMaximumPrice(wantedPlayer.MaxPurchasePrice);

            await fIFA22WebAppManager.MakeSearch();

            string foundedPlayerPurchasePrice = await fIFA22WebAppManager.CheckPlayerPurchasePriceIfFounded();

            if (foundedPlayerPurchasePrice == null)
            {
                Console.Error.WriteLine($"[{DateTime.Now}]: No players found.");

                return;
            }

            Console.WriteLine($"[{DateTime.Now}]: Player founded - {foundedPlayerPurchasePrice} coins");

            bool playerBought = await fIFA22WebAppManager.TryToBuyPlayer();

            if (playerBought)
            {
                Console.Beep();
                Console.WriteLine($"[{DateTime.Now}]: Player bought for {foundedPlayerPurchasePrice} coins");
            }
        }
    }
}