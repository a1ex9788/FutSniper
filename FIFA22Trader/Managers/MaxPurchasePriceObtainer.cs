using System;

namespace FIFA22Trader.Managers
{
    public static class MaxPurchasePriceObtainer
    {
        private const int MinPurchasePrice = 200;

        private static readonly Random random = new Random();

        public static int CalculateMaxPurchasePrice(int maxPurchasePrice)
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