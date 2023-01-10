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
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            // Credentials are read from a .json file ignored by Git. Windows credentials system is not used for simplicity.
            // Windows specific dlls and administrator permissions may be needed.
            configurationBuilder.AddJsonFile("Credentials.json", optional: true);

            IConfiguration credentialsConfiguration = configurationBuilder.Build();

            return credentialsConfiguration.Get<Credentials>();
        }

        public static IEnumerable<WantedPlayer> GetWantedPlayers()
        {
            if (configuration == null)
            {
                BuildConfiguration();
            }

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

        public static int GetDelaySecondsBetweenAttempts()
        {
            if (configuration == null)
            {
                BuildConfiguration();
            }

            return configuration.GetValue<int>("DelaySecondsBetweenAttempts");
        }

        private static void BuildConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddJsonFile("Configuration.json", optional: false, reloadOnChange: true);

            configuration = configurationBuilder.Build();
        }
    }
}