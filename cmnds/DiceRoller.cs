using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
                Match m1 = Regex.Match(mtch, @"(\d(\d)?)?d(\d)(\d)?", RegexOptions.IgnoreCase);
                string expression = m1.Groups[0].Value;
                if (m1.Groups[1].Success) {
                    count = int.Parse(expression[0].ToString());
                    if (m1.Groups[2].Success) {
                        count =count*10 + int.Parse(expression[1].ToString());
                    }
                }
                if (m1.Groups[4].Success)
                    numb = int.Parse(m1.Groups[3].Value + m1.Groups[4].Value);
                else numb = int.Parse(m1.Groups[3].Value);
                for (int i =0;i<count;i++) {
                    result += Commands.r.Next(1,numb+1);
                }
                MatchCollection bonusMatches = Regex.Matches(mtch, @"([+--]\d(\d)?)");
                if (bonusMatches.Count > 0) {
                    for (int i=0;i<bonusMatches.Count;i++) {
                        int temp = 0;
                        temp = int.Parse(bonusMatches[i].Groups[1].Value[1].ToString());
                        if (bonusMatches[i].Groups[2].Success)
                        {
                            temp = temp * 10 + int.Parse(bonusMatches[i].Groups[1].Value[2].ToString());
                        }
                        if (bonusMatches[i].Groups[1].Value[0] == '-') temp = temp * -1;
                        bonus = bonus+temp;
                    }
                }
                result += bonus;
                Regex totalRegex = new Regex("t");
                if (totalRegex.IsMatch(mtch)) total +=result;
                message += "> " + mtch + " = " + result+"\n";
                result = 0;
                bonus = 0;
            }
            await cmct.RespondAsync(message);
            if(total!=0) await cmct.RespondAsync("> total = " + total).ConfigureAwait(false);
            total = 0;
        }
    }
}