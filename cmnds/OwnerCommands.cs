using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Dungeon_master.cmnds;

namespace Dungeon_master
{
    class OwnerCommands
    {
        //static DiscordRole ModeratorRole { get; set; }
        //static string ModeratorRoleName = "";

        //static void ChangeModeratorRole(ref string dr, string value)
        //{
        //    dr = value;
        //}

        /*[Command("SetModeratorRole")]
        [About(" ")]
        [RequireOwner]
        public async Task SetModeratorRole(CommandContext cmx, ulong role) {
            try {
                DiscordRole Role = cmx.Guild.GetRole(role);
                ModeratorRole = Role;
                ChangeModeratorRole(ref ModeratorRoleName,ModeratorRole.Name);
                await cmx.RespondAsync("Moderator role now is "+ModeratorRoleName);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }*/

        [Command("CreateSurvey")]
        [About("Creates a survey.")]
        [RequireRolesAttribute("Moderator", "вертухай")]
        public async Task CreateSurvey(CommandContext cmx, string titel, params string[] message) {
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
        [Command("DeleteMessages")]
        [About("Deletes the desired number of recent messages.")]
        [RequireRolesAttribute("Moderator","вертухай")]
        public async Task DeleteMessages(CommandContext cmx, int amount)
        {
            var messages = await cmx.Channel.GetMessagesAsync(amount + 1);

            await cmx.Channel.DeleteMessagesAsync(messages);
            var CC = await cmx.RespondAsync("deleted " + amount + " messages");
            await Task.Delay(5000);
            await CC.DeleteAsync();
        }
    }
}
