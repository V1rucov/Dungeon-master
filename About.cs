﻿using System;

namespace Dungeon_master
{
    [AttributeUsage(AttributeTargets.Method)]
    class AboutAttribute : Attribute
    {
        private readonly string comment;
        public AboutAttribute(string x) => comment = x;
        public string Remark
        {
            get { return comment; }
        }
    }
}
