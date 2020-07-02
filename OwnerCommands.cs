using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace Dungeon_master
{
    class OwnerCommands
    {
        [Command("CreateSurvey")]
        [About("Создаёт опрос.")]
        public async Task CreateSurvey(CommandContext cmx, string titel, params string[] message) {
            var embed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.DarkRed,
                Title = titel,
                Description = String.Join(" ", message)
            };
            var JoinMessage = await cmx.RespondAsync(embed: embed).ConfigureAwait(false);

            var plus = DiscordEmoji.FromName(cmx.Client,":+1:");
            var minus = DiscordEmoji.FromName(cmx.Client, ":-1:");

            await JoinMessage.CreateReactionAsync(plus).ConfigureAwait(false);
            await JoinMessage.CreateReactionAsync(minus).ConfigureAwait(false);
            await cmx.Message.DeleteAsync();
        }
        [Command("dm")]
        [About("Удаляет нужное кол-во последних сообщений.")]
        public async Task dm(CommandContext cmct, int amount)
        {
            var messages = await cmct.Channel.GetMessagesAsync(amount + 1);

            await cmct.Channel.DeleteMessagesAsync(messages);
            var CC = await cmct.RespondAsync("Удалено " + amount + " сообщений");
            await Task.Delay(5000);
            await CC.DeleteAsync();
        }
    }
}
