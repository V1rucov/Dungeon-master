using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_master
{
    class CharacterContext : DbContext
    {
        public CharacterContext() :base("CharacterContext")
        { }
        public DbSet<Character> Characters { get; set; }
        public DbSet<weapon> weapons { get; set; }
    }
}
