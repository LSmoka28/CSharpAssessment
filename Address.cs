using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAssessmentProject
{
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
