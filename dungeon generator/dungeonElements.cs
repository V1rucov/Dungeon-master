using System;

namespace Dungeon_master{
    class DungeonElement{
        public string description{get;set;}
    }
    class Room{
        public string Form{get;set;}
        public int exits{get;set;}
        public string destination {get;set;}
        public string state{get;set;}
        public string content {get;set;}
        public string noise {get;set;}
        public string air{get;set;}
        public string smell{get;set;}
        public string interior{get;set;}

    }
    class trap{

    }
}