using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
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
        public int инициатива { get; set; }
        public spell Spell { get; set; }
    }
    [ComplexType]
    class spell
    {
        public int[] spellsPerLevel = new int[9];
    }
}
