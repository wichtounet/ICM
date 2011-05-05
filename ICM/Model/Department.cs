namespace ICM.Model
{
    public class Department
    {
        public int Id
        {
            get;
            set;
        }

        public string Name {
            get; 
            set;
        }

        public int InstitutionId
        {
            get;
            set;
        }

        public string InstitutionName
        {
            get; 
            set; 
        }

        public override string ToString()
        {
            return Name;
        }
    }
}