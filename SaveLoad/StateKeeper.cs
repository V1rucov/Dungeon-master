using System;
using System.Collections.Generic;
using System.Text;
using Dungeon_master;

namespace IUI.SaveLoad
{
    class States
    {
        public List<Character> states { get; set; }
        public States() {
            states = new List<Character>();
        }
        public void add(Character x) {
            states.Add(x);
        }
    }
}
