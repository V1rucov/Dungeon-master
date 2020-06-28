using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_master
{
    class Character
    {
        public int Id { get; set; }
        public string Имя { get; set; }
        public int сила { get; set; }
        public int ловкость { get; set; }
        public int телосложение { get; set; }
        public int интеллект { get; set; }
        public int харизма { get; set; }
        public int мудрость { get; set; }
        public string внешка { get; set; }
        public string предыстория { get; set; }
        public int уровень { get; set; }
    }
}
