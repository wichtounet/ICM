namespace ICM.Model
{
    public class Department
    {
        public string Name {
            get; 
            set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}