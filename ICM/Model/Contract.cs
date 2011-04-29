using System;

namespace ICM.Model
{
    public class Contract
    {
        DateTime startDate;
        DateTime endDate;
        string file;
        string xmlContent;

        public Contract()
        {
            //Nothing to do
        }
        public string Title
        {
            get;
            set;
        }
        public int id
        {
            get;
            set;
        }
    }
}