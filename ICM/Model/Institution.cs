using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICM.Model
{
    public class Institution
    {
        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Interest
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public Country Country
        {
            get;
            set;
        }

        public List<Department> Departments
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }

        public Institution( int Id,
                            string Name,
                            string Description,
                            string City,
                            string Interest,
                            Language Language,
                            Country Country,
                            List<Department> Departments)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.City = City;
            this.Interest = Interest;
            this.Language = Language;
            this.Country = Country;
            this.Departments = Departments;
        }
    }
}