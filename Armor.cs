using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleProjTemp
{
    class Armor : Item
    {
        public struct ArmorStruct
        {
            private string name;
            private string type;
            private string info;
            private int defense;
            private string rarity;
            private int price;

            public string Name { get => name; set => name = value; }
            public string Type { get => type; set => type = value; }
            public string Info { get => info; set => info = value; }
            public int Defense { get => defense; set => defense = value; }
            public string Rarity { get => rarity; set => rarity = value; }
            public int Price { get => price; set => price = value; }
        }

    }
}
