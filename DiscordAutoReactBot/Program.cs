using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace DiscordAutoEmojiBot
{
    class Program
    {
        static DiscordClient Client;

        static void Main(string[] args)
        {
            var prog = new Program();
            RunBotAsync().GetAwaiter().GetResult();
        }

        static async Task RunBotAsync()
        {
            Config cfg = Config.Read("config.json");
            if (cfg.Token == "")
            {
                Console.WriteLine("Invalid bot token!");
                Console.ReadKey();
                return;
            }
            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = cfg.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            Client.MessageCreated += async e =>
            {
                if (e.Message.ChannelId == cfg.ChannelID && !e.Message.Author.IsBot)
                {
                    foreach (string emoji in cfg.Reactions)
                    {
                        await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, emoji));
                        await Task.Delay(1000);
                    }
                }
            };

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
