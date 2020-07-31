using Dungeon_master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace IUI.SaveLoad
{
    class LoadCommands
    {
        public static States states = new States();

        [Command("SaveChar")]
        public async Task SaveChar(CommandContext cmct, string shortName)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                Character temp = cc.Characters.Where(c => c.ShortName == shortName).FirstOrDefault();
                states.add(temp);
                await cmct.RespondAsync("saved "+temp.clas).ConfigureAwait(false);
            }
        }

        [Command("LoadChar")]
        public async Task LoadChar(CommandContext cmct,string shortName) {
            using (CharacterContext cc = new CharacterContext()) {
                Character FromDB = cc.Characters.Where(c => c.ShortName == shortName).FirstOrDefault();
                Character t = states.states.Find(c => c.ShortName == shortName);
                FromDB = t;
                cc.SaveChanges();
                await cmct.RespondAsync("loaded "+FromDB.clas).ConfigureAwait(false);
            }
        }
    }
}
