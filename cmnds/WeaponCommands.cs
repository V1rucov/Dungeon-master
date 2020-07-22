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
        [Command("ak")]
        [About("Attacks a character.")]
        public async Task ak(CommandContext cmct, string attackerName, string defenderName) {
            using (CharacterContext cc = new CharacterContext()) {
                var attacker = cc.Characters.Where(n => n.ShortName == attackerName).FirstOrDefault();
                var defender = cc.Characters.Where(n => n.ShortName == defenderName).FirstOrDefault();
                int attackRoll = Commands.r.Next(1, 20);
                if (attackRoll >= defender.def)
                {
                    int damage = Commands.r.Next(1, attacker.Weapon.damage) + (attacker.pow - 10)/2;
                    defender.HP -= damage;
                    cc.SaveChanges();
                    await cmct.RespondAsync(attackerName+" attacked "+defenderName+" for "+damage.ToString()+" damage!");
                }
                else {
                    await cmct.RespondAsync(attackerName+" missed!");
                }
            }
        }
    }
}
