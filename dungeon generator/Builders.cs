using System;

namespace Dungeon_master{
    abstract class dungeonType{
        public string path {get;set;}
    }
    class RoomBuilder{
        dungeonType dt;
        public RoomBuilder(dungeonType dt){
            this.dt=dt;
        }
        public Room build(){
            Room room = new Room();
            randomTableGet rt = new randomTableGet();

            room.destination = rt.Generate(dt.path,101);
            room.Form = rt.Generate("dungeon generator/room_form.txt",21);
            room.exits = Commands.r.Next(1,6);
            room.state = rt.Generate("dungeon generator/room_state.txt",21);
            room.smell = rt.Generate("dungeon generator/room_smell.txt",101);
            room.noise = rt.Generate("dungeon generator/room_noise.txt",101);
            room.air = rt.Generate("dungeon generator/room_air.txt",101);
            return room;
        }
    }
    class TrapBuilder{
        public trap build(){
            trap Trap = new trap();
            randomTableGet rt = new randomTableGet();
            Trap.trigger = rt.Generate("dungeon generator/trap_trigger.txt",7);
            Trap.effect = rt.Generate("dungeon generator/trap_effect.txt",101);
            Trap.damage = rt.Generate("dungeon generator/trap_damage.txt",7);
            return Trap;
        }
    }
    class ladderBuilder{
        public DungeonElement build(){
            DungeonElement ladder = new DungeonElement();
            randomTableGet rt = new randomTableGet();
            ladder.description = rt.Generate("corridor_desc.txt",20) +", "+ rt.Generate("corridor_form.txt",20);
            return ladder;
        }
    }
    class planarDungeon : dungeonType{
        public planarDungeon(){
            path = "dungeon generator/donj_planar.txt";
        }
    }
    
}