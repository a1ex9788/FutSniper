using FutSniper.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FutSniper.Managers
{
    public static class ConfigurationManager
    {
        public static Credentials GetCredentials()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            // Credentials are read from a .json file ignored by Git. Windows credentials system is not used for simplicity.
            // Windows specific dlls and administrator permissions may be needed.
            configurationBuilder.AddJsonFile("Credentials.json", optional: true);

            IConfiguration credentialsConfiguration = configurationBuilder.Build();

            return credentialsConfiguration.Get<Credentials>();
        }

        public static IEnumerable<WantedPlayer> GetWantedPlayers()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddJsonFile("Configuration.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();

            List<WantedPlayer> wantedPlayers = new List<WantedPlayer>();

            foreach (IConfigurationSection wantedPlayer in configuration.GetRequiredSection("WantedPlayers").GetChildren())
            {
                wantedPlayers.Add(new WantedPlayer()
                {
                    Name = wantedPlayer.Key,
                    MaxPurchasePrice = int.Parse(wantedPlayer.Value),
                });
            };

            return wantedPlayers;
        }
    }
}