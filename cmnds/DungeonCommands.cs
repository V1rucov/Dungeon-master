using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using Dungeon_master.cmnds;
using Microsoft.EntityFrameworkCore.Internal;

namespace Dungeon_master{
    class DungeonCommands{
        [Command("de")]
        [About("Generates random dungeon element (room,trap,ladder etc.)")]
        public async Task de(CommandContext cmct, string objectName, string DType = null){
            if(objectName=="room"){
                RoomBuilder dtrb = new RoomBuilder("dungeon generator/donj/donj_"+DType+".txt");
                Room room = dtrb.build();
                var embed = new DiscordEmbedBuilder{
                Title = "Комната",
                Description ="Форма - "+room.Form+"\n состояние - "+room.state+"\n запах - "+room.smell+"\n воздух - "+room.air+"\n назначение - "+
                room.destination+"\n кол-во выходов - "+room.exits+"\n шум - "+room.noise+"\n содержимое - "+room.content
                };
                await cmct.RespondAsync(embed:embed);
            }
            else if (objectName=="trap"){
                TrapBuilder tb = new TrapBuilder();
                trap Trap = tb.build();
                var embed = new DiscordEmbedBuilder{
                Title = "Ловушка",
                Description = "Срабатывает от - "+Trap.trigger+", эффект - "+Trap.effect+", урон - "+Trap.damage
                };
                await cmct.RespondAsync(embed:embed);
            }
            else if (objectName=="ladder"){
                ladderBuilder lb = new ladderBuilder();
                var embed = new DiscordEmbedBuilder{
                Title = "Лестница",
                Description = lb.build()
                };
                await cmct.RespondAsync(embed:embed);
            }
            else if (objectName=="trick"){
                trickBuilder tb = new trickBuilder();
                var embed = new DiscordEmbedBuilder{
                Title = "Трюк",
                Description = tb.build()
                };
                await cmct.RespondAsync(embed:embed);
            }
        }
    }
}