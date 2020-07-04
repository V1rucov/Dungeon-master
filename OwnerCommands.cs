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
using System.Security.Cryptography.Pkcs;

namespace Dungeon_master
{
    class OwnerCommands
    {
        static DiscordRole Moderator;

        [Command("CreateSurvey")]
        [About("Создаёт опрос.")]
        public async Task CreateSurvey(CommandContext cmx, string titel, params string[] message) {
            if ((DiscordRole)cmx.Member.Roles==Moderator) {
                var embed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.DarkRed,
                    Title = titel,
                    Description = String.Join(" ", message)
                };
                var JoinMessage = await cmx.RespondAsync(embed: embed).ConfigureAwait(false);

                var plus = DiscordEmoji.FromName(cmx.Client, ":+1:");
                var minus = DiscordEmoji.FromName(cmx.Client, ":-1:");

                await JoinMessage.CreateReactionAsync(plus).ConfigureAwait(false);
                await JoinMessage.CreateReactionAsync(minus).ConfigureAwait(false);
                await cmx.Message.DeleteAsync();
            }
        }
        [Command("DeleteMessages")]
        [About("Удаляет нужное кол-во последних сообщений.")]
        public async Task dm(CommandContext cmx, int amount)
        {
            if ((DiscordRole)cmx.Member.Roles == Moderator)
            {
                var messages = await cmx.Channel.GetMessagesAsync(amount + 1);

                await cmx.Channel.DeleteMessagesAsync(messages);
                var CC = await cmx.RespondAsync("Удалено " + amount + " сообщений");
                await Task.Delay(5000);
                await CC.DeleteAsync();
            }
        }
        [Command("SetModeratorRole")]
        [About("Устанавлиевает роль для использования комманд владельца.")]
        public async Task SetModeratorRole(CommandContext cmx) {
            if (cmx.Member.IsOwner) {
                var role = cmx.Guild.GetRole(538742467302785025);
                Moderator = role;
                await cmx.RespondAsync("установлена роль модератора.");
            }
        }
    }
}
