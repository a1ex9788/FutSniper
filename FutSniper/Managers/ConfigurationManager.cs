using FutSniper.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FutSniper.Managers
{
    public static class ConfigurationManager
    {
        private static IConfiguration configuration;

        public static Credentials GetCredentials()
        {
            InitializeConfigurationIfItIsNot();

            return configuration.GetRequiredSection("Credentials").Get<Credentials>();
        }

        public static IEnumerable<WantedPlayer> GetWantedPlayers()
        {
            InitializeConfigurationIfItIsNot();

            List<WantedPlayer> wantedPlayers = new List<WantedPlayer>();

            foreach (var wantedPlayer in configuration.GetRequiredSection("WantedPlayers").GetChildren())
            {
                wantedPlayers.Add(new WantedPlayer()
                {
                    Name = wantedPlayer.Key,
                    MaxPurchasePrice = int.Parse(wantedPlayer.Value),
                });
            };

            return wantedPlayers;
        }

        private static void InitializeConfigurationIfItIsNot()
        {
            if (configuration == null)
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

                configurationBuilder.AddJsonFile("Configuration.json", optional: false, reloadOnChange: true);

                configuration = configurationBuilder.Build();
            }
        }
    }
}