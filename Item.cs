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
        public virtual void ToString(string name, string type, string info, int attackOrDefense, string rarity, int price)
        {
            
        }
    }
}
