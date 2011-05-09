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

        public string departementInstitution
        {
            get { return InstitutionName + ": " + Name; }
        }

        public string InstitutionCity
        {
            get;
            set;
        }

        public string InstitutionCountry
        {
            get;
            set;
        }

        public string InstitutionLanguage
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