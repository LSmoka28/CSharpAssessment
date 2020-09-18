using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleProjTemp
{
    class Weapon : Item
    {
        public struct WeaponStruct
        {
            private string name;
            private string type;
            private string info;
            private int attackPwr;
            private string rarity;
            private int price;

            public string Name { get => name; set => name = value; }
            public string Type { get => type; set => type = value; }
            public string Info { get => info; set => info = value; }
            public int AttackPwr { get => attackPwr; set => attackPwr = value; }
            public string Rarity { get => rarity; set => rarity = value; }
            public int Price { get => price; set => price = value; }
        }

    }
}
