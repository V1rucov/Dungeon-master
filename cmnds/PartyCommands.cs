using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Linq;
using DSharpPlus.Interactivity;

namespace Dungeon_master.cmnds
{
    class PartyCommands
    {
        public static List<string> Party = new List<string>();

        [Command("cp")]
        [About("Creates a party of players")]
        public async Task cp(CommandContext cmct, params string[] names) {
            using (CharacterContext cc = new CharacterContext()) {
                for (int i =0;i<names.Length;i++) {
                    try
                    {
                        string tempName = names[i];
                        var temp = cc.Characters.Where(c => c.name == tempName).FirstOrDefault();
                        Party.Add(temp.name);
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
            foreach (var temp in Party) {
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
                    foreach (var chara in Party) {
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
                    await cmct.RespondAsync("ERROR!" + ex.Message);
                }
            }
            await cmct.RespondAsync("Party level up .");
        }
    }
}