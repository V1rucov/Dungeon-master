using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using DSharpPlus.Interactivity;
using Dungeon_master.cmnds;
using Microsoft.EntityFrameworkCore.Internal;

namespace Dungeon_master
{
    partial class Commands
    {
        static readonly int[] masterstwo = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };
        public static Random r = new Random();

        [Command("check")]
        [About("checks a param.")]
        public async Task check(CommandContext cmct, string personName, string statName, string skillName=null) {
            using (CharacterContext cc = new CharacterContext())
            {
                Character chara = cc.Characters.Where(c => c.ShortName == personName).FirstOrDefault();
                PropertyInfo temp = chara.GetType().GetProperty(statName);
                int result = 0;
                if (skillName==null) {
                    result = r.Next(1, 20) + ((int)temp.GetValue(chara) - 10) / 2;
                    await cmct.RespondAsync("result = D20 + " + ((int)temp.GetValue(chara) - 10) / 2 + " = " + result).ConfigureAwait(false);
                }
                else if(skillName!=null)
                {
                    PropertyInfo pi = chara.GetType().GetProperty("skills");
                    string skill = (string)pi.GetValue(chara);
                    skill = skill.Replace(",", string.Empty);
                    string[] skills = skill.Split();
                    int stp = skills.IndexOf(skillName)+1;
                    int bonus = skills[stp].Length;
                    result = r.Next(1, 20) + ((int)temp.GetValue(chara) - 10) / 2 + masterstwo[chara.level] * bonus;
                    await cmct.RespondAsync("result = D20 + " + ((int)temp.GetValue(chara) - 10) / 2 + " + " + masterstwo[chara.level] * bonus + " = " + result);
                }
            }
        }

        [Command("cc")]
        [About("Create new character.")]
        public async Task cc(CommandContext cmct, string name, string clas,int pow, int dex, int body, int wisdom, int intel, int charisma, int dice, int def, params string[] skills)
        {
            string skillCC = String.Join(" ", skills);
            Character temp = new Character() { ShortName = name, pow = pow, dex = dex, bod = body, wis = wisdom, Int = intel, cha = charisma,
                skills = skillCC, level = 1, ini=0, clas=clas, max_HP = dice, dice=dice, HP =dice,def=def, history="no.", name="no.", spellPoints="-"};
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
                        "History: "+chara.history+"\n"
                    };
                    var JoinMessage = await cmct.RespondAsync(embed: embed).ConfigureAwait(false);
                    //spell points
                    string temp = chara.spellPoints;
                    string[] splitted = temp.Split();
                    string message = " (level : count) ";
                    for (int i = 0; i < splitted.Length - 1; i++)
                    {
                        int j = i + 1;
                        message += "**" + j + "**" + " - " + splitted[i] + ", ";
                    }
                    var embedd = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Azure,
                        Title = "Spell points of " + Name,
                        Description = message
                    };
                    await cmct.RespondAsync(embed: embedd).ConfigureAwait(false);
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
        [Command("ss")]
        [About("Set a spell point.")]
        public async Task ss(CommandContext cmct, string name, params string[] points) {
            using (CharacterContext cc = new CharacterContext()) {
                var chara = cc.Characters.Where(c => c.ShortName == name).FirstOrDefault();
                string toInput = "";
                for (int i =0; i<points.Length; i++) {
                    int j = i + 1;
                    toInput += points[i]+" ";
                }
                chara.spellPoints = toInput;
                cc.SaveChanges();
                await cmct.RespondAsync(toInput);
            }
        }
        [Command("us")]
        [About("Use a spell.")]
        public async Task us(CommandContext cmct, string name, int pointLevel, int pointCount = 1) {
            using (CharacterContext cc = new CharacterContext())
            {
                pointLevel = pointLevel - 1;
                var chara = cc.Characters.Where(c => c.ShortName == name).FirstOrDefault();
                string[] splitted = chara.spellPoints.Split();
                int temp = int.Parse(splitted[pointLevel]);
                if (temp - pointCount >= 0)
                {
                    temp -= pointCount;
                    splitted[pointLevel] = temp.ToString();
                    chara.spellPoints = string.Join(" ",splitted);
                    cc.SaveChanges();
                    await cmct.RespondAsync("ok.");
                }
                else await cmct.RespondAsync("Not enough spell points.");

            }
        }

        [Command("bb")]
        [About("Begins a battle.")]
        public async Task bb(CommandContext cmct,bool team, params string [] list)
        {
            await cmct.RespondAsync("Started battle.");
            List<Character> members = new List<Character>();
            using (CharacterContext cc = new CharacterContext()) {
                for (int j =0; j<list.Length;j++) {
                    if (team) {
                        foreach (var temp in PartyCommands.Party)
                        {
                            var person = cc.Characters.Where(c => c.name == temp).FirstOrDefault();
                            person.ini = Commands.r.Next(1, 20) + (person.dex - 10) / 2;
                            members.Add(person);
                        }
                    }
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