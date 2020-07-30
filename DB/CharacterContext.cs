using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Dungeon_master
{
    class CharacterContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=characters.db");
        }
        public DbSet<Character> Characters { get; set; }
    }
}
