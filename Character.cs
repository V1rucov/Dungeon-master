using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_master
{
    class Character
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Имя { get; set; }
        public int сила { get; set; }
        public int ловкость { get; set; }
        public int телосложение { get; set; }
        public int интеллект { get; set; }
        public int харизма { get; set; }
        public int мудрость { get; set; }
        [MaxLength(256)]
        public string внешка { get; set; }
        [MaxLength(512)]
        public string предыстория { get; set; }
        public int уровень { get; set; }
    }

    class Game {
        public int Id { get; set; }
        public string CampagnName { get; set; }
        public string Setting { get; set; }
        public string DM { get; set; }
        [MaxLength(256)]
        public string about { get; set; }
        public List<string> chars = new List<string>();
    }
}
