using System;

namespace Dungeon_master{
    abstract class RoomBuilder{
        Room room = new Room();
        randomTable rt = new randomTable();
        public abstract Room build();
    }
    class planarRoomBuilder : RoomBuilder{
        Room room = new Room();
        randomTable rt = new randomTable();
        public override Room build(){
            room.destination = rt.Generate("dungeon generator/donj_planar.txt",101);
            room.Form = rt.Generate("dungeon generator/room_form.txt",21);
            room.exits = Commands.r.Next(0,6);
            room.state = rt.Generate("dungeon generator/room_state.txt",21);
            room.smell = rt.Generate("dungeon generator/room_smell.txt",100);
            return room;
        }
    }
}