using System;

namespace Dungeon_master{
    class RoomBuilder{
        string path;
        public RoomBuilder(string path){
            this.path=path;
        }
        public Room build(){
            Room room = new Room();
            randomTableGet rt = new randomTableGet();

            room.destination = rt.Generate(path,101);
            room.Form = rt.Generate("dungeon generator/room/room_form.txt",21);
            room.exits = Commands.r.Next(1,6);
            room.state = rt.Generate("dungeon generator/room/room_state.txt",21);
            room.smell = rt.Generate("dungeon generator/room/room_smell.txt",101);
            room.noise = rt.Generate("dungeon generator/room/room_noise.txt",101);
            room.air = rt.Generate("dungeon generator/room/room_air.txt",101);
            room.content = rt.Generate("dungeon generator/room/room_content.txt",100);
            return room;
        }
    }
    class TrapBuilder{
        public trap build(){
            trap Trap = new trap();
            randomTableGet rt = new randomTableGet();
            Trap.trigger = rt.Generate("dungeon generator/trap/trap_trigger.txt",7);
            Trap.effect = rt.Generate("dungeon generator/trap/trap_effect.txt",101);
            Trap.damage = rt.Generate("dungeon generator/trap/trap_damage.txt",7);
            return Trap;
        }
    }
    class ladderBuilder{
        public string build(){
            randomTableGet rt = new randomTableGet();
            string desc = rt.Generate("dungeon generator/corridor_desc.txt",20) +", "+ rt.Generate("dungeon generator/corridor_form.txt",20);
            return desc;
        }
    }
    class trickBuilder{
        public string build(){
            randomTableGet rt = new randomTableGet();
            string trick = rt.Generate("dungeon generator/tricks/trick_item.txt",20)+"\n влияние - "+rt.Generate("dungeon generator/tricks/trick_effect.txt",100);;
            return trick;
        }
    }
}