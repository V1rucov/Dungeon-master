﻿using System;
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
using Dungeon_master.cmnds;

namespace Dungeon_master
{
     partial class Commands
     {
        static int[] masterstwo = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };
        public static Random r = new Random();

        [Command("cc")]
        [About("Create new character.")]
        public async Task cc(CommandContext cmct, string name, string clas,int pow, int dex, int body, int wisdom, int intel, int charisma, int dice, int def, params string[] skills)
        {
            string skillCC = String.Join(" ", skills);
            Character temp = new Character() { ShortName = name, pow = pow, dex = dex, bod = body, wis = wisdom, Int = intel, cha = charisma,
                skills = skillCC, level = 1, ini=0, clas=clas, max_HP = dice, dice=dice, HP =dice,def=def, history="no.", name="no."};
            using (CharacterContext cc = new CharacterContext())
            {
                try
                {
                    cc.Characters.Add(temp);
                    cc.SaveChanges();
                }
                catch (Exception ex)
                {
                    await cmct.RespondAsync("ERROR!"+ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
            await cmct.RespondAsync("Character " + name + " was created");
            await cmct.Message.DeleteAsync();
        }
        [Command("gc")]
        [About("Returns a character.")]
        public async Task gc(CommandContext cmct, string Name)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                try {
                    var chara = cc.Characters.Where(c => c.ShortName == Name).FirstOrDefault();
                    string wne = String.Join(" ", chara.skills);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Azure,
                        Title = chara.name+" ("+Name+")",
                        Description = "**class**: "+chara.clas+", **level**: "+chara.level+", **defence**: "+chara.def+", **HP**: "+chara.HP+"\n"+
                        "> **POW:** "+chara.pow+", **DEX:** "+chara.dex+", **BOD:** "+chara.bod+"\n"+
                        "> **WIS:** "+chara.wis+", **INT:** "+chara.Int+", **CHA:** "+chara.cha+"\n"+
                        "_Skills_: "+wne+"\n"+
                        "History: "+chara.history
                    };
                    var JoinMessage = await cmct.RespondAsync(embed: embed).ConfigureAwait(false);
                }
                catch (Exception ex) {
                    await cmct.RespondAsync("ERROR!" + ex.Message);
                }
            }
        }

        [Command("rc")]
        [About("Removes a character.")]
        public async Task rc(CommandContext cmct, string Name)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                try {
                    Character person = cc.Characters
                    .Where(b => b.ShortName == Name).FirstOrDefault();

                    cc.Characters.Remove(person);
                    cc.SaveChanges();
                    await cmct.RespondAsync("character " + Name + " was removed");
                }
                catch (Exception ex)
                {
                    await cmct.RespondAsync("ERROR!" + ex.Message);
                }
            }
        }
        [Command("lu")]
        [About("Increases character level (by 1 by default).")]
        public async Task lu(CommandContext cmct, string Name, int up=1) {
            using (CharacterContext cc = new CharacterContext()) {
                try
                {
                    var chara = cc.Characters.Where(c => c.ShortName == Name).FirstOrDefault();
                    chara.level = chara.level + up;
                    for (int i =0;i<up;i++) {
                        chara.max_HP = chara.max_HP + r.Next(1, chara.dice) + (chara.bod - 10) / 2;
                    }
                    chara.HP = chara.max_HP;
                    cc.SaveChanges();
                }
                catch (Exception ex)
                {
                    await cmct.RespondAsync("ERROR!" + ex.Message);
                }
            }
            await cmct.RespondAsync("Level up "+Name+".");
        }
        [Command("bb")]
        [About("Begins a battle.")]
        public async Task bb(CommandContext cmct, params string [] list)
        {
            await cmct.RespondAsync("Started battle.");
            List<Character> members = new List<Character>();
            using (CharacterContext cc = new CharacterContext()) {
                for (int j =0; j<list.Length;j++) {
                    try {
                        string STemp = list[j];
                        Character CTemp = cc.Characters.Where(m => m.ShortName == STemp).FirstOrDefault();
                        CTemp.ini = r.Next(1, 20) + (CTemp.dex-10)/2;
                        members.Add(CTemp);
                    }
                    catch (Exception ex) {
                        Console.WriteLine("ERROR!" + ex.Message);
                        members.Add(new Character() { name = list[j], Int = 10, dex = 10, wis = 10, pow = 10, bod = 10, cha = 10, ini = r.Next(1, 20) });
                    }
                }
            }
            PartyCommands.battle(cmct,members);
        }
        [Command("dd")]
        [About("Deal damage.")]
        public async Task dd(CommandContext cmct, string Name, int damage) {
            using (CharacterContext cc =new CharacterContext()) {
                Character person = cc.Characters.Where(c => c.ShortName == Name).FirstOrDefault();
                person.HP = person.HP - damage;
                cc.SaveChanges();
                await cmct.RespondAsync("Dealed " +damage+" damage to "+Name);
            }
        }
        [Command("hr")]
        [About("Reset HP.")]
        public async Task hr(CommandContext cmct, string Name) {
            using (CharacterContext cc = new CharacterContext()) {
                var person = cc.Characters.Where(c => c.ShortName == Name).FirstOrDefault();
                person.HP = person.max_HP;
                cc.SaveChanges();
                await cmct.RespondAsync("reseted HP for "+Name);
            }
        }

        [Command("mp")]
        [About("Modify parameter.")]
        public async Task mp(CommandContext cmct, string personName,string statName, params string[] param) {
            using (CharacterContext cc = new CharacterContext()) {
                Character chara = cc.Characters.Where(c => c.ShortName == personName).FirstOrDefault();
                PropertyInfo temp = chara.GetType().GetProperty(statName);
                try {
                    int a = Int32.Parse(param[0]);
                    temp.SetValue(chara, a);
                }
                catch
                {
                    string x = string.Join(" ", param);
                    temp.SetValue(chara, x);
                }
                await cmct.RespondAsync("saved.");
                cc.SaveChanges();
            }
        }

        [Command("cgloc")]
        [About("returns list of commands.")]
        public async Task gloc(CommandContext cmct)
        {
            Type t = typeof(Commands);
            MethodInfo[] methodInfo = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            await cmct.RespondAsync("list - " + methodInfo.Length);
            string about = "";
            foreach (var CC in methodInfo)
            {

                Type atr = typeof(AboutAttribute);
                AboutAttribute Remark = (AboutAttribute)Attribute.GetCustomAttribute(CC, atr);

                about = about + "**" + CC.Name + "** --" + Remark.Remark + " params:";
                ParameterInfo[] pi = CC.GetParameters();
                for (int i = 1; i < pi.Length; i++)
                {
                    var DD = pi[i];
                    about = about + " " + DD.Name + "(" + DD.ParameterType + "),";
                }
                about = about + "\n";
            }
            await cmct.RespondAsync(about);
        }
     }
}