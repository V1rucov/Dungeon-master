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
        enum ToDo { 
            plus,
            minus,
            multiply,
            divide,
            count
        }
        [Command("r")]
        [About("Rolls a dice.")]
        public async Task roll(CommandContext cmct, params string[] context) {
            int result = 0;
            int count = 1;
            //int index = 0;
            int numb = 0;
            int bonus = 0;

            foreach (string mtch in context) {

                Match m1 = Regex.Match(mtch, @"(\d)?d(\d)(\d)?(\+\d(\d)?)?(\+\d(\d)?)?(\+\d(\d)?)?", RegexOptions.IgnoreCase);
                string expression = m1.Groups[0].Value;
                if (m1.Groups[1].Success) {
                    count = int.Parse(expression[0].ToString());
                    //index = 1;
                }
                if (m1.Groups[3].Success)
                    numb = int.Parse(m1.Groups[2].Value + m1.Groups[3].Value);
                else numb = int.Parse(m1.Groups[2].Value);

                for (int i =0;i<count;i++) {
                    result += Commands.r.Next(1,numb+1);
                }
                //4 and 5
                if (m1.Groups[4].Success) {
                    bonus += int.Parse(expression[m1.Groups[4].Index+1].ToString());
                    if (m1.Groups[5].Success) {
                        bonus += int.Parse(expression[m1.Groups[4].Index + 1].ToString()+ expression[m1.Groups[5].Index].ToString())-1;
                    }
                }
                //6 and 7
                if (m1.Groups[6].Success)
                {
                    bonus += int.Parse(expression[m1.Groups[6].Index + 1].ToString());
                    if (m1.Groups[7].Success)
                    {
                        bonus += int.Parse(expression[m1.Groups[6].Index + 1].ToString() + expression[m1.Groups[7].Index].ToString())-1;
                    }
                }
                //8 and 9
                if (m1.Groups[8].Success)
                {
                    bonus += int.Parse(expression[m1.Groups[8].Index + 1].ToString());
                    if (m1.Groups[9].Success)
                    {
                        bonus += int.Parse(expression[m1.Groups[8].Index + 1].ToString() + expression[m1.Groups[9].Index].ToString())-1;
                    }
                }
                result += bonus;
                await cmct.RespondAsync("> "+expression+" = "+result).ConfigureAwait(false);
                result = 0;
                bonus = 0;
            }
        }
    }
}