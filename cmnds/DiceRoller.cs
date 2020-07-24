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
using Dungeon_master.cmnds;
using System.Text.RegularExpressions;

namespace Dungeon_master.cmnds
{
    class DiceRoller
    {
        [Command("r")]
        [About("Rolls a dice.")]
        public async Task roll(CommandContext cmct, params string[] context) {
            int result = 0;
            int count = 1;
            int numb = 0;
            int bonus = 0;
            int total = 0;
            string message = "";

            foreach (string mtch in context) {

                Match m1 = Regex.Match(mtch, @"(\d)?d(\d)(\d)?(\+\d(\d)?)?(\+\d(\d)?)?(\+\d(\d)?)?(\-\d(\d)?)?(\-\d(\d)?)?(\-\d(\d)?)?(t)?", RegexOptions.IgnoreCase);
                string expression = m1.Groups[0].Value;
                if (m1.Groups[1].Success) {
                    count = int.Parse(expression[0].ToString());
                }
                if (m1.Groups[3].Success)
                    numb = int.Parse(m1.Groups[2].Value + m1.Groups[3].Value);
                else numb = int.Parse(m1.Groups[2].Value);

                for (int i =0;i<count;i++) {
                    result += Commands.r.Next(1,numb+1);
                }
                for (int i =4;i<9;i=i+2) {
                    if (m1.Groups[i].Success)
                    {
                        bonus += int.Parse(expression[m1.Groups[i].Index + 1].ToString());
                        if (m1.Groups[i+1].Success)
                        {
                            bonus += int.Parse(expression[m1.Groups[i].Index + 1].ToString() + expression[m1.Groups[i+1].Index].ToString()) - 1;
                        }
                    }
                }
                for (int i = 10; i < 15; i = i + 2)
                {
                    if (m1.Groups[i].Success)
                    {
                        bonus -= int.Parse(expression[m1.Groups[i].Index + 1].ToString());
                        if (m1.Groups[i + 1].Success)
                        {
                            bonus -= int.Parse(expression[m1.Groups[i].Index + 1].ToString() + expression[m1.Groups[i + 1].Index].ToString()) - 1;
                        }
                    }
                }
                result += bonus;
                if (m1.Groups[16].Success) total +=result;
                message += "> " + expression + " = " + result+"\n";
                result = 0;
                bonus = 0;
            }
            await cmct.RespondAsync(message);
            if(total!=0) await cmct.RespondAsync("> total = " + total).ConfigureAwait(false);
            total = 0;
        }
    }
}