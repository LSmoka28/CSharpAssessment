﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSharpAssessmentProject
{
    class Armor : Item
    {
        // struct to hold values from armor csv
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

        // prints the values of the struct to the console, overrides parent method
        public override void ToString(string name, string type, string info, int defense, string rarity, int price)
        {
            Console.WriteLine($"____________________\n- {name} -\nType: {type}\nDescription:\n{info}\nDamage: {defense} pts\nRarity: {rarity}\nCost: {price} units\n");
        }
    }
}
