using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System.Threading;
using System.Diagnostics;
using Dungeon_master.cmnds;
using System.Xml;

namespace Dungeon_master
{
    class Program
    {
        public static string Token { get; set; }
        static CommandsNextModule CNMmodule;
        static DiscordClient client { get; set; }
        static void Main(string[] args)
        {
            preLoad();
            debug();
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        [Conditional("debug")]
        static void debug() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[" + DateTime.Now + "] "+ " [Dungeon master v1.0] Debug mode -- ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("UNCOMMERCIAL USE ONLY");
        }
        static void preLoad() {
            XmlDocument config = new XmlDocument();
            config.Load(@"bot.xml");
            XmlElement root = config.DocumentElement;
            foreach (XmlNode cc in root)
            {
                XmlNode attr = cc.Attributes.GetNamedItem("contentType");
                if (attr.Value == "token")
                {
                    XmlNode content = cc.ChildNodes[0];
                    Token = content.InnerText;
                }
            }
        }
        static async Task MainAsync(string[] args)
        {
            client = new DiscordClient(new DiscordConfiguration() { Token = Token, TokenType = TokenType.Bot, UseInternalLogHandler = true});
            CNMmodule = client.UseCommandsNext(new CommandsNextConfiguration() { StringPrefix = "/"});
            client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = Timeout.InfiniteTimeSpan
            });

            CNMmodule.RegisterCommands<Commands>();
            CNMmodule.RegisterCommands<OwnerCommands>();
            CNMmodule.RegisterCommands<PartyCommands>();
            CNMmodule.RegisterCommands<DiceRoller>();

            client.MessageCreated += async e =>
            {
                if (e.Message.MentionEveryone)
                {
                    await e.Message.RespondAsync("Нельзя использовать эвриван");
                }
                else if (e.Message.Content == "тут?")
                {
                    await e.Message.RespondAsync("хули ты хотел?");
                }
            };
            await client.ConnectAsync();
        }
    }
}
