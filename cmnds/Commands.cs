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
        public async Task cc(CommandContext cmct, string name, string clas,int pow, int dex, int body, int wisdom, int intel, int charisma, params string[] wneshka)
        {
            string wneshkaCC = String.Join(" ", wneshka);
            Character temp = new Character() { Имя = name, сила = pow, ловкость = dex, телосложение = body, мудрость = wisdom, интеллект = intel, харизма = charisma, навыки = wneshkaCC, уровень = 1, инициатива=0, класс=clas};
            temp.предыстория = "no history ";
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
                    var chara = cc.Characters.Where(c => c.Имя == Name).FirstOrDefault();
                    string wne = String.Join(" ", chara.навыки);
                    var embed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.DarkRed,
                        Title = Name,
                        Description = "**class:** "+chara.класс+", **level**: "+chara.уровень+"\n"+
                        "**POW:** "+chara.сила+" **DEX:** "+chara.ловкость+" **BOD:** "+chara.телосложение+"\n"+
                        "**WIS:** "+chara.мудрость+" **INT:** "+chara.интеллект+" **CHA:** "+chara.харизма+"\n"+
                        "_Skills_: "+wne+"\n"+
                        "History: "+chara.предыстория
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
                    .Where(b => b.Имя == Name).FirstOrDefault();

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
                    var chara = cc.Characters.Where(c => c.Имя == Name).FirstOrDefault();
                    chara.уровень = chara.уровень + up;
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
                        Character CTemp = cc.Characters.Where(m => m.Имя == STemp).FirstOrDefault();
                        CTemp.инициатива = r.Next(1, 20) + (CTemp.ловкость-10)/2;
                        members[j] = CTemp;
                    }
                    catch (Exception ex) {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("["+DateTime.Now+"]  [Dungeon master v1.0] -- "+ex.Message);
                        members[j] = new Character() {Имя= list[j], интеллект=10, ловкость=10, мудрость=10, сила=10, телосложение=10, харизма=10, инициатива= r.Next(1,20)};
                    }
                }
            }
            for (int j=0;j<4;j++) {
                for (int i = 0; i < members.Length - 1; i++)
                {
                    if (members[i].инициатива < members[i + 1].инициатива)
                    {
                        var temp = members[i];
                        members[i] = members[i + 1];
                        members[i + 1] = temp;
                    }
                }
            }
            string compl = "";
            foreach (var CC in members) {
                compl = compl + CC.Имя + ", iniciative - "+CC.инициатива+". \n";
            }
            var embed = new DiscordEmbedBuilder {
                Title = "battle participants: \n",
                ImageUrl= "https://i.ytimg.com/vi/w0sUw735gRw/maxresdefault.jpg",
                Description = compl
            };
            await cmct.RespondAsync(embed: embed).ConfigureAwait(false);
            var Interactivity = cmct.Client.GetInteractivityModule();

            int t = 0;
            while (true) {
                if (t >= members.Length) t = 0;
                if (members[t] == null) t++;
                await cmct.RespondAsync("turn of " + members[t].Имя).ConfigureAwait(false);
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
                    await cmct.RespondAsync(members[t].Имя+" was kicked.");
                    members[t] = null;
                }
            }
        }

        [Command("dr")]
        [About("Rolls a given dice and adds a bonus from the required parameter and skill.")]
        public async Task dr(CommandContext cmct, int sides, string name, string stat,int skill = 0, bool mast = false)
        {
            Character person;
            int bon = 0;
            int result;
            using (CharacterContext cc = new CharacterContext())
            {
                person = cc.Characters
                    .Where(b => b.Имя == name).FirstOrDefault();
            }
            switch (stat)
            {
                case "str":
                    bon = (person.сила - 10) / 2;
                    break;
                case "dex":
                    bon = (person.ловкость - 10) / 2;
                    break;
                case "bod":
                    bon = (person.телосложение - 10) / 2;
                    break;
                case "cha":
                    bon = (person.харизма - 10) / 2;
                    break;
                case "int":
                    bon = (person.интеллект - 10) / 2;
                    break;
                case "wis":
                    bon = (person.мудрость - 10) / 2;
                    break;
            }
            if (mast == false)
            {
                result = r.Next(1, sides) + bon+skill;
                await cmct.RespondAsync("result d" + sides + " +" + stat + skill+" = " + result);
            }
            else
            {
                int bm = masterstwo[person.уровень];
                result = r.Next(1, sides) + bon + bm+skill;
                await cmct.RespondAsync("result d" + sides + " + " + stat + " + " + bm + skill+" = " + result);
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