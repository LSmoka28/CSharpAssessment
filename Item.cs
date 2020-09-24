using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAssessmentProject
{
    //base item class for printing child class values
    class Item
    {
        private string name;
        private string type;
        private string info;
        private int attackPwr;
        private int defense;
        private string rarity;
        private int price;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Info { get => info; set => info = value; }
        public int AttackPwr { get => attackPwr; set => attackPwr = value; }
        public int Defense { get => defense; set => defense = value; }
        public string Rarity { get => rarity; set => rarity = value; }
        public int Price { get => price; set => price = value; }


        public virtual void ToString(string name, string type, string info, int attackOrDefense, string rarity, int price)
        {
            
        }
    }
}
