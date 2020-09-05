using System;

namespace Dungeon_master{
    abstract class dungeonType{
        public string path {get;set;}
    }
    class RoomBuilder{
        Room room = new Room();
        randomTable rt = new randomTable();
        dungeonType dt;
        public RoomBuilder(dungeonType dt){
            this.dt=dt;
        }
        public Room build(){
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
    class planarDungeon : dungeonType{
        public planarDungeon(){
            path = "dungeon generator/donj_planar.txt";
        }
    }
    
}