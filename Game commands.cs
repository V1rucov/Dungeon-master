using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Runtime.InteropServices;
using System.Linq;

namespace Dungeon_master
{
    partial class Commands
    {
        [Command("cg")]
        [About("Создаёт новую игру.")]
        public async Task cg(CommandContext cmct, string name, string setting, string dm, params string[] About) {
            Game CC = new Game() { CampagnName=name, Setting=setting, DM=dm, about=string.Join(" ",About)};
            using (CharacterContext caC = new CharacterContext()) {
                caC.Games.Add(CC);
                caC.SaveChanges();
            }
            await cmct.RespondAsync("Игра "+name+" создана");
            await cmct.Message.DeleteAsync();
        }
        [Command("gg")]
        [About("Возвращает информацию о игре.")]
        public async Task gg(CommandContext cmct, string GameName) {
            Game temp; //= new Game();
            using (CharacterContext cc = new CharacterContext()) {
                temp = cc.Games.Where(g => g.CampagnName == GameName).FirstOrDefault();
            }
            await cmct.RespondAsync("Название игры --"+temp.CampagnName+", сеттинг --"+temp.Setting+", ведущий --"+temp.DM+", коротко об игре --"+temp.about);
        }
        [Command("agc")]
        [About("Добавляет персонажей в игру.")]
        public async Task agc(CommandContext cmct, string game, string name) {
            using (CharacterContext cc = new CharacterContext()) {
                Game temp = cc.Games.Where(g => g.CampagnName == game).FirstOrDefault();
                temp.chars.Add(name);
            }
            await cmct.RespondAsync("персонаж "+name+" добавлен в игру "+game);
        }
        [Command("ggc")]
        [About("Возвращает список персонажей игры.")]
        public async Task ggc(CommandContext cmct, string game) {
            List<string> characters;
            using (CharacterContext cc = new CharacterContext()) {
                Game temp = cc.Games.Where(g => g.CampagnName == game).FirstOrDefault();
                characters = temp.chars;
                string copml = string.Join(" ", characters);
                await cmct.RespondAsync("список персонажей игры " + game + " -- "+copml);
            }
        }
    }
}