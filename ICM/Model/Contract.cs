using System;

namespace ICM.Model
{
    public class Contract
    {
        public Contract()
        {
            //Nothing to do
        }
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
        public string Start
        {
            get;
            set;
        }
        public string End
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

        public override string ToString()
        {
            return Title;
        }
    }
}