using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Xml;

namespace Dungeon_master
{
    class OwnerCommands
    {
        public static DiscordRole moder = new DiscordRole();

        [Command("start")]
        [About("starts bot.")]
        public async Task start(CommandContext cmx) {
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(@"bot.xml");
                XmlElement root = config.DocumentElement;
                foreach (XmlNode cc in root)
                {
                    XmlNode attr = cc.Attributes.GetNamedItem("contentType");
                    if (attr.Value == "role")
                    {
                        XmlNode content = cc.ChildNodes[0];
                        moder = cmx.Guild.GetRole(ulong.Parse(content.InnerText));
                    }
                }
                await cmx.RespondAsync("bot started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        [Command("CreateSurvey")]
        [About("Creates a survey.")]
        public async Task CreateSurvey(CommandContext cmx, string titel, params string[] message) {
            if (cmx.Member.Roles.Contains(moder)) {
                var embed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.DarkRed,
                    Title = titel,
                    Description = String.Join(" ", message)
                };
                var JoinMessage = await cmx.RespondAsync(embed: embed).ConfigureAwait(false);

                var plus = DiscordEmoji.FromName(cmx.Client, ":+1:");
                var minus = DiscordEmoji.FromName(cmx.Client, ":-1:");

                await JoinMessage.CreateReactionAsync(plus);
                await JoinMessage.CreateReactionAsync(minus);
                await cmx.Message.DeleteAsync();
            }
        }
        [Command("DeleteMessages")]
        [About("Deletes the desired number of recent messages.")]
        public async Task DeleteMessages(CommandContext cmx, int amount)
        {
            if (cmx.Member.Roles.Contains(moder))
            {
                var messages = await cmx.Channel.GetMessagesAsync(amount + 1);

                await cmx.Channel.DeleteMessagesAsync(messages);
                var CC = await cmx.RespondAsync("deleted " + amount + " messages");
                await Task.Delay(5000);
                await CC.DeleteAsync();
            }
        }
    }
}
