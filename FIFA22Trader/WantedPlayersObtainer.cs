using System;
using System.Configuration;

namespace FIFA22Trader
{
    public static class WantedPlayersObtainer
    {
        private const int MinPurchasePrice = 200;

        private static readonly Random random = new Random();

        public static (string name, int maxPurchasePrice) GetWantedPlayer()
        {
            ConfigurationManager.RefreshSection("appSettings");

            string wantedPlayer = ConfigurationManager.AppSettings.Get("WantedPlayer");
            int maxPurchasePrice = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxPurchasePrice"));

            maxPurchasePrice = CalculateMaxPurchasePrice(maxPurchasePrice);

            return (wantedPlayer, maxPurchasePrice);
        }

        private static int CalculateMaxPurchasePrice(int maxPurchasePrice)
        {
            int finalMaxPurchasePrice = maxPurchasePrice;

            // Changing the price used in order to avoid results caching.
            if (GetHalfProbability())
            {
                finalMaxPurchasePrice -= 100;
            }

            if (finalMaxPurchasePrice < MinPurchasePrice)
            {
                finalMaxPurchasePrice = MinPurchasePrice;
            }

            return finalMaxPurchasePrice;
        }

        private static bool GetHalfProbability()
        {
            return random.Next(0, 2) == 0;
        }
    }
}