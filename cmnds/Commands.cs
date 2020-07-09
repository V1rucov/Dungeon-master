﻿using System;
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
using DSharpPlus.Interactivity;

namespace Dungeon_master
{
     partial class Commands
     {
        static int[] masterstwo = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };
        static Random r = new Random();

        [Command("cc")]
        [About("Создаёт нового персонажа с задаными свойствами.")]
        public async Task cc(CommandContext cmct, string name, int pow, int dex, int body, int wisdom, int intel, int charisma, params string[] wneshka)
        {
            string wneshkaCC = String.Join(" ", wneshka);
            Character temp = new Character() { Имя = name, сила = pow, ловкость = dex, телосложение = body, мудрость = wisdom, интеллект = intel, харизма = charisma, внешка = wneshkaCC, уровень = 1, инициатива=0};
            temp.предыстория = "не задано ";
            //temp.Spell = new spell();
            //temp.Spell.spellsPerLevel = new int[9];
            using (CharacterContext cc = new CharacterContext())
            {
                try
                {
                    cc.Characters.Add(temp);
                    cc.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            await cmct.RespondAsync("персонаж " + name + " создан");
            await cmct.Message.DeleteAsync();
        }
        [Command("gc")]
        [About("Возвращает свойства персонажа.")]
        public async Task gc(CommandContext cmct, string Name)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                foreach (Character CC in cc.Characters)
                {
                    if (CC.Имя == Name)
                    {
                        string wne = String.Join(" ", CC.внешка);
                        await cmct.RespondAsync($"Имя - " + CC.Имя +
                            ". статы: сила " + CC.сила + ", ловкость " + CC.ловкость + ", телосложение " + CC.телосложение + ", мудрость " + CC.мудрость + ", харизма " + CC.харизма + ", интеллект " + CC.интеллект +
                            ". Внешний вид: " + wne);
                    }
                }
            }
        }

        [Command("rc")]
        [About("Удаляет персонажа.")]
        public async Task rc(CommandContext cmct, string Name)
        {
            using (CharacterContext cc = new CharacterContext())
            {
                Character person = cc.Characters
                    .Where(b => b.Имя == Name).FirstOrDefault();

                cc.Characters.Remove(person);
                cc.SaveChanges();
                await cmct.RespondAsync("Персонаж " + Name + " удалён");
            }
        }
        [Command("lu")]
        [About("Повышает уровень персонажа (на 1 по умолчанию).")]
        public async Task lu(CommandContext cmct, string Name, int up=1) {
            using (CharacterContext cc = new CharacterContext()) {
                var chara = cc.Characters.Where(c => c.Имя == Name).FirstOrDefault();
                chara.уровень = chara.уровень + up;
                cc.SaveChanges();
            }
            await cmct.RespondAsync("Уровень персонажа "+Name+" повышен.");
        }
        [Command("ss")]
        [About("Устанавливает ячейки заклинаний.")]
        public async Task ss(CommandContext cmct, string Name, params int[] spls) {
            using (CharacterContext cc = new CharacterContext()) {
                var chara = cc.Characters.Where(c => c.Имя == Name).FirstOrDefault();
                for (int i=0;i<spls.Length;i++) {
                    chara.Spell.spellsPerLevel[i] = spls[i];
                }
                cc.SaveChanges();
            }
            await cmct.RespondAsync("Изменения сохранены.");
        }
        [Command("gs")]
        [About("Возвращает ячейки заклинаний.")]
        public async Task gs(CommandContext cmct, string Name) {
            using (CharacterContext cc = new CharacterContext()) {
                var chara = cc.Characters.Where(c => c.Имя == Name).FirstOrDefault();
                string ToSend = "*Ячейки заклинаний каждого уровня*: \n";
                for (int i = 0; i < 9; i++) {
                    ToSend+= i+" - "+chara.Spell.spellsPerLevel[i].ToString() + " штук. \n";
                }
                await cmct.RespondAsync(ToSend).ConfigureAwait(false);
            }
        }
        [Command("bb")]
        [About("начинает бой.")]
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
                compl = compl + CC.Имя + ", инициатива - "+CC.инициатива+". \n";
            }
            var embed = new DiscordEmbedBuilder {
                Title = "Участники боя: \n",
                Description = compl
            };
            await cmct.RespondAsync(embed: embed).ConfigureAwait(false);
            var Interactivity = cmct.Client.GetInteractivityModule();

            int t = 0;
            while (true) {
                if (t >= members.Length) t = 0;
                if (members[t] == null) t++;
                await cmct.RespondAsync("Сейчас ходит " + members[t].Имя).ConfigureAwait(false);
                var message = await Interactivity.WaitForMessageAsync(m => m.Channel==cmct.Channel).ConfigureAwait(false);
                if (message.Message.Content == "-e")
                {
                    Console.WriteLine("[" + DateTime.Now + "]  [Dungeon master v1.0] -- ended a battle");
                    await cmct.RespondAsync("бой окончен");
                    break;
                }
                else if (message.Message.Content == "-n") {
                    t++;
                }
                else if (message.Message.Content == "-k") {
                    await cmct.RespondAsync(members[t].Имя+" вылетел из боя.");
                    members[t] = null;
                }
            }
        }

        [Command("dr")]
        [About("Бросает заданную кость.")]
        public async Task dr(CommandContext cmct, int sides)
        {
            await cmct.RespondAsync("результат броска к" + sides + " = " + r.Next(1, sides).ToString());
        }
        [Command("drp")]
        [About("Бросает заданную кость и прибавляет значение.")]
        public async Task drp(CommandContext cmct, int sides, int bonus)
        {
            int CC = r.Next(1, sides) + bonus;
            await cmct.RespondAsync("результат броска к" + sides + "+" + bonus + " = " + CC);
        }
        [Command("drs")]
        [About("Бросает заданную кость и добавляет бонус от требуемого параметра.")]
        public async Task drs(CommandContext cmct, int sides, string name, string stat, bool mast = false)
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
                result = r.Next(1, sides) + bon;
                await cmct.RespondAsync("Результат броска к" + sides + " +" + stat + " = " + result);
            }
            else
            {
                int bm = masterstwo[person.уровень];
                result = r.Next(1, sides) + bon + bm;
                await cmct.RespondAsync("Результат броска к" + sides + " + " + stat + " + " + bm + " = " + result);
            }
        }

        [Command("gloc")]
        [About("Возвращет список доступных команд.")]
        public async Task gloc(CommandContext cmct)
        {
            Type t = typeof(Commands);
            MethodInfo[] methodInfo = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            await cmct.RespondAsync("_Всего команд_ - " + methodInfo.Length);
            string about = "";
            foreach (var CC in methodInfo)
            {

                Type atr = typeof(AboutAttribute);
                AboutAttribute Remark = (AboutAttribute)Attribute.GetCustomAttribute(CC, atr);

                about = about + "**" + CC.Name + "** --" + Remark.Remark + " Параметры:";
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