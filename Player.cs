using CSharpAssessmentProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAssessmentProject
{
    public class Player
    {
        
        public string name;
        public Address address;       
        public Player(string name, Address address)
        {
            this.address = address;
            this.name = name;
        }

        
        // prints player info to the console
        public void Display()
        {
            Console.WriteLine($"{name} - {address.city}, {address.stateOrCountry}");
        }


    }

    
}
