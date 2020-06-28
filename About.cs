using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_master
{
    [AttributeUsage(AttributeTargets.Method)]
    class AboutAttribute : Attribute
    {
        private string comment;
        public AboutAttribute(string x) => comment = x;
        public string Remark
        {
            get { return comment; }
        }
    }
}
