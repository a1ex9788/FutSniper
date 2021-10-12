using FIFA22Trader.Entities;
using System;
using System.Configuration;

namespace FIFA22Trader.Managers
{
    public static class WantedPlayersObtainer
    {
        public static WantedPlayer GetWantedPlayer()
        {
            ConfigurationManager.RefreshSection("appSettings");

            string wantedPlayer = ConfigurationManager.AppSettings.Get("WantedPlayer");
            int maxPurchasePrice = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxPurchasePrice"));

            maxPurchasePrice = MaxPurchasePriceObtainer.CalculateMaxPurchasePrice(maxPurchasePrice);

            return new WantedPlayer()
            {
                Name = wantedPlayer,
                MaxPurchasePrice = maxPurchasePrice,
            };
        }
    }
}