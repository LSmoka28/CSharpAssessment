using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProjTemp
{
    public class Player
    {    
        private string name;
        Address address;       
        public Player(string name, Address address)
        {
            this.address = address;
            this.name = name;
        }

        
        
        public void Display()
        {
            Console.WriteLine($"{name} - {address.city}, {address.stateOrCountry}");
        }


    }
    public class Address
    {
        public string city;
        public string stateOrCountry;

        public Address(string city, string stateOrCountry)
        {
            this.city = city;
            this.stateOrCountry = stateOrCountry;
        }

    }

}
