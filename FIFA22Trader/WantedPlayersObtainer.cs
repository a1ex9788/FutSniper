using System;
using System.Configuration;

namespace FIFA22Trader
{
    public static class WantedPlayersObtainer
    {
        public static (string name, int maxPurchasePrice) GetWantedPlayer()
        {
            ConfigurationManager.RefreshSection("appSettings");

            string wantedPlayer = ConfigurationManager.AppSettings.Get("WantedPlayer");
            int maxPurchasePrice = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxPurchasePrice"));

            maxPurchasePrice = MaxPurchasePriceObtainer.CalculateMaxPurchasePrice(maxPurchasePrice);

            return (wantedPlayer, maxPurchasePrice);
        }
    }
}