using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_master.cmnds
{
    class WeaponCommands
    {
        [Command("gw")]
        [About("Gives/removes weapon to/from hero.")]
        public async Task gw(CommandContext cmct, string person, string weaponName,int damage) {
            using (CharacterContext cc = new CharacterContext()) {
                try {
                    var chara = cc.Characters.Where(c => c.ShortName == person).FirstOrDefault();
                    var weapn = new weapon() { Name=weaponName,damage=damage};
                    chara.Weapon = weapn;
                    await cmct.RespondAsync("ok.");
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                cc.SaveChanges();
            }
        }
    }
}
