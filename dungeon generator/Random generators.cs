using System;
using System.IO;
using System.Linq;
using Dungeon_master.cmnds;

namespace Dungeon_master{
    class randomTableGet{
        public string Generate(string path, int sides){
            StreamReader sr = new StreamReader(path);
            int numb = Commands.r.Next(1,sides);
            for(int i =0;i<100;i++){
                string temp = sr.ReadLine();
                int min = Int32.Parse(temp[0].ToString()+temp[1].ToString());
                int max = Int32.Parse(temp[3].ToString()+temp[4].ToString());
                if(max==00) max = 100;
                if(numb>=min && numb<=max){
                    string toReturn = temp.Substring(5);
                    return toReturn;
                }
            }
            return null;
        }
    }
}