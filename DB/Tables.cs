﻿using System;
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
        public string Имя { get; set; }
        public int сила { get; set; }
        public int ловкость { get; set; }
        public int телосложение { get; set; }
        public int интеллект { get; set; }
        public int харизма { get; set; }
        public int мудрость { get; set; }
        [MaxLength(256)]
        public string навыки { get; set; }
        [MaxLength(512)]
        public string предыстория { get; set; }
        public int уровень { get; set; }
        public int инициатива { get; set; }
        public string класс { get; set; }
    }
}
