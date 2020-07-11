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

namespace Dungeon_master
{
     partial class Commands
     {
        static int[] masterstwo = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };
        static Random r = new Random();

        [Command("cc")]
        [About("Create new character.")]
        public async Task cc(CommandContext cmct, string name, string clas,int pow, int dex, int body, int wisdom, int intel, int charisma, int dice, int def, params string[] skills)
        {
            string skillCC = String.Join(" ", skills);
            Character temp = new Character() { name = name, pow = pow, dex = dex, bod = body, wis = wisdom, Int = intel, cha = charisma,
                skills = skillCC, level = 1, ini=0, clas=clas, max_HP = dice, dice=dice, HP =dice,def=def, history="not yet"};
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
                    var chara = cc.Characters.Where(c => c.name == Name).FirstOrDefault();
                    string wne = String.Join(" ", chara.skills);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Cyan,
                        Title = Name,
                        Description = "**class**: "+chara.clas+", **level**: "+chara.level+", **defence**: "+chara.def+", **HP**: "+chara.HP+"\n"+
                        "> **POW:** "+chara.pow+", **DEX:** "+chara.def+", **BOD:** "+chara.bod+"\n"+
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
                    .Where(b => b.name == Name).FirstOrDefault();

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
                    var chara = cc.Characters.Where(c => c.name == Name).FirstOrDefault();
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
            Console.WriteLine("[" + DateTime.Now + "]  [Dungeon master v1.0] -- started a battle");
            Character[] members = new Character[list.Length];
            using (CharacterContext cc = new CharacterContext()) {
                for (int j =0; j<list.Length;j++) {
                    try {
                        string STemp = list[j];
                        Character CTemp = cc.Characters.Where(m => m.name == STemp).FirstOrDefault();
                        CTemp.ini = r.Next(1, 20) + (CTemp.dex-10)/2;
                        members[j] = CTemp;
                    }
                    catch (Exception ex) {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("["+DateTime.Now+"]  [Dungeon master v1.0] -- "+ex.Message);
                        members[j] = new Character() {name= list[j], Int=10, dex=10, wis=10, pow=10, bod=10, cha=10, ini= r.Next(1,20)};
                    }
                }
            }
            for (int j=0;j<4;j++) {
                for (int i = 0; i < members.Length - 1; i++)
                {
                    if (members[i].ini < members[i + 1].ini)
                    {
                        var temp = members[i];
                        members[i] = members[i + 1];
                        members[i + 1] = temp;
                    }
                }
            }
            string compl = "";
            foreach (var CC in members) {
                compl = compl + CC.name + ", iniciative - "+CC.ini+". \n";
            }
            var embed = new DiscordEmbedBuilder {
                Color = DiscordColor.Goldenrod,
                Title = "Brawl: \n",
                Description = "Fighters - "+compl
            };
            await cmct.RespondAsync(embed: embed).ConfigureAwait(false);
            var Interactivity = cmct.Client.GetInteractivityModule();

            int t = 0;
            while (true) {
                if (t >= members.Length) t = 0;
                if (members[t] == null) t++;
                await cmct.RespondAsync("turn of " + members[t].name).ConfigureAwait(false);
                var message = await Interactivity.WaitForMessageAsync(m => m.Channel==cmct.Channel).ConfigureAwait(false);
                if (message.Message.Content == "-e")
                {
                    Console.WriteLine("[" + DateTime.Now + "]  [Dungeon master v1.0] -- ended a battle");
                    await cmct.RespondAsync("battle ended");
                    break;
                }
                else if (message.Message.Content == "-n") {
                    t++;
                }
                else if (message.Message.Content == "-k") {
                    await cmct.RespondAsync(members[t].name+" was kicked.");
                    members[t] = null;
                }
            }
        }

        [Command("dr")]
        [About("Rolls a given dice and adds a bonus from the required parameter and skill.")]
        public async Task dr(CommandContext cmct, int sides, string name="_", string stat ="", int skill = 0, bool mast = false)
        {
            Character person = new Character() {pow=10, dex=10, bod=10, wis=10, Int=10, cha=10 };
            int bon = 0;
            int result;
            if (name != "_") using (CharacterContext cc = new CharacterContext())
                {
                    person = cc.Characters
                        .Where(b => b.name == name).FirstOrDefault();
                }
            switch (stat)
            {
                case "str":
                    bon = (person.pow - 10) / 2;
                    break;
                case "dex":
                    bon = (person.dex - 10) / 2;
                    break;
                case "bod":
                    bon = (person.bod - 10) / 2;
                    break;
                case "cha":
                    bon = (person.cha - 10) / 2;
                    break;
                case "int":
                    bon = (person.Int - 10) / 2;
                    break;
                case "wis":
                    bon = (person.wis - 10) / 2;
                    break;
                default:
                    bon = 0;
                    break;
            }
            if (mast == false)
            {
                result = r.Next(1, sides) + bon+skill;
                await cmct.RespondAsync("result d" + sides + " +" + stat + skill+" = " + result);
            }
            else
            {
                int bm = masterstwo[person.level];
                result = r.Next(1, sides) + bon + bm+skill;
                await cmct.RespondAsync("result d" + sides + " + " + stat + " + " + bm +" + "+ skill+" = " + result);
            }
        }
        [Command("dd")]
        [About("Deal damage.")]
        public async Task dd(CommandContext cmct, string Name, int damage) {
            using (CharacterContext cc =new CharacterContext()) {
                Character person = cc.Characters.Where(c => c.name == Name).FirstOrDefault();
                person.HP = person.HP - damage;
                cc.SaveChanges();
                await cmct.RespondAsync("Dealed " +damage+" damage to "+Name);
            }
        }
        [Command("mp")]
        [About("Modify parameter.")]
        public async Task mp(CommandContext cmct, string personName,string statName, int param) {
            using (CharacterContext cc = new CharacterContext()) {
                try {
                    Character chara = cc.Characters.Where(c => c.name == personName).FirstOrDefault();
                    PropertyInfo temp = chara.GetType().GetProperty(statName);
                    temp.SetValue(chara, param);
                    cc.SaveChanges();
                    await cmct.RespondAsync("saved.");
                }
                catch (Exception ex)
                {
                    await cmct.RespondAsync("ERROR!" + ex.Message);
                }
            }
        }

        [Command("gloc")]
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