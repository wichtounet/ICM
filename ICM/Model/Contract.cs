using System;
using System.Collections.Generic;

namespace ICM.Model
{
    public class Contract
    {
        public string Title
        {
            get;
            set;
        }
        public int Id
        {
            get;
            set;
        }
        public DateTime Start
        {
            get;
            set;
        }
        public DateTime End
        {
            get;
            set;
        }
        public string XmlContent
        {
            get;
            set;
        }
        public string File
        {
            get;
            set;
        }
        public string User
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        public Boolean Archived
        {
            get;
            set;
        }
        public int fileId
        {
            get;
            set;
        }
        public List<Person> persons
        {
            get;
            set;
        }
        public List<Department> departments
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}