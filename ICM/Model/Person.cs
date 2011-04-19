using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICM.Model
{
    public class Person
    {
        public int Id
        {
            get;
            set;
        }

        public string FirstName {
            get; 
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public Department Department
        {
            get;
            set;
        }

        public override string ToString()
        {
            return FirstName + Name;
        }
    }
}