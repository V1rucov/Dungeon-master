using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dungeon_master
{
    class Character
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string name { get; set; }
        public int pow { get; set; }
        public int dex { get; set; }
        public int bod { get; set; }
        public int Int { get; set; }
        public int cha { get; set; }
        public int wis { get; set; }
        [MaxLength(256)]
        public string skills { get; set; }
        [MaxLength(512)]
        public string history { get; set; }
        public int level { get; set; }
        public int HP { get; set; }
        public int max_HP { get; set; }
        public int dice { get; set; }
        public int def { get; set; }
        public int ini { get; set; }
        public string clas { get; set; }
    }
}
