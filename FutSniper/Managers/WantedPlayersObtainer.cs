using FutSniper.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace FutSniper.Managers
{
    public static class WantedPlayersObtainer
    {
        private static readonly Stack<WantedPlayer> wantedPlayersToSearch = new Stack<WantedPlayer>();

        public static WantedPlayer GetWantedPlayer()
        {
            if (!wantedPlayersToSearch.Any())
            {
                FillWantedPlayersToSearchList();
            }

            return wantedPlayersToSearch.Pop();
        }

        private static void FillWantedPlayersToSearchList()
        {
            ConfigurationManager.RefreshSection("appSettings");

            NameValueCollection wantedPlayers = ConfigurationManager.AppSettings;

            foreach (string wantedPlayerName in wantedPlayers.Keys)
            {
                string maxPurchasePriceString = wantedPlayers[wantedPlayerName];

                int maxPurchasePrice = Convert.ToInt32(maxPurchasePriceString);

                maxPurchasePrice = MaxPurchasePriceObtainer.CalculateMaxPurchasePrice(maxPurchasePrice);

                wantedPlayersToSearch.Push(new WantedPlayer()
                {
                    Name = wantedPlayerName,
                    MaxPurchasePrice = maxPurchasePrice,
                });
            }
        }
    }
}