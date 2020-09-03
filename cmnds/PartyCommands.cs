using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using DSharpPlus.Interactivity;

namespace Dungeon_master.cmnds
{
    class PartyCommands
    {
        public static Dictionary<string,string[]> prty = new Dictionary<string, string[]>();

        [Command("cp")]
        [About("Creates a party of players")]
        public async Task cp(CommandContext cmct, params string[] names) {
            using (CharacterContext cc = new CharacterContext()) {
                for (int i =0;i<names.Length;i++) {
                    try
                    {
                        string tempName = names[i];
                        var temp = cc.Characters.Where(c => c.ShortName == tempName).FirstOrDefault();
                        prty.Add(cmct.Channel.Name,names);
                        Console.WriteLine(cmct.Channel.Name+" ");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                await cmct.RespondAsync("Party was created.").ConfigureAwait(false);
            }
        }
        [Command("gp")]
        [About("Returns a party of players")]
        public async Task gp(CommandContext cmct) {
            string toReturn = "";
            foreach (var temp in prty[cmct.Channel.Name]) {
                toReturn += temp + ", ";
            }
            await cmct.RespondAsync(toReturn);
        }
        [Command("plu")]
        [About("Increases party level (by 1 by default).")]
        public async Task lu(CommandContext cmct, int up = 1)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                try
                {
                    foreach (var temp in prty[cmct.Channel.Name]) {
                        var chara = cc.Characters.Where(c => c.name == temp).FirstOrDefault();
                        chara.level = chara.level + up;
                        for (int i = 0; i < up; i++)
                        {
                            chara.max_HP = chara.max_HP + Commands.r.Next(1, chara.dice) + (chara.bod - 10) / 2;
                        }
                        chara.HP = chara.max_HP;
                    }
                    cc.SaveChanges();
                }
                catch (Exception ex)
                {
                    await cmct.RespondAsync("ERROR! " + ex.Message);
                }
            }
            await cmct.RespondAsync("Party level up.");
        }
        public static async void battle(CommandContext cmct, List<Character> members) {
            members.Sort((a, b) => b.ini.CompareTo(a.ini));
            string compl = "";
            foreach (var CC in members)
            {
                compl = compl + CC.name + ", iniciative - " + CC.ini + ". \n";
            }
            var embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Goldenrod,
                Title = "Brawl: \n",
                Description = "Fighters - " + compl
            };
            await cmct.RespondAsync(embed: embed).ConfigureAwait(false);

            var Interactivity = cmct.Client.GetInteractivityModule();
            for (int i = 0; ; i++)
            {
                try
                {
                    if (i == members.Count) i = 0;
                    await cmct.RespondAsync("turn of " + members[i].name).ConfigureAwait(false);
                    var message = await Interactivity.WaitForMessageAsync(m => m.Channel == cmct.Channel && (m.Content == "-e" || m.Content == "-k" || m.Content == "-n")).ConfigureAwait(false);
                    if (message.Message.Content == "-e") break;
                    else if (message.Message.Content == "-k") members.RemoveAt(i);
                }
                catch {
                    i = 0;
                }
            }
        }
    }
}