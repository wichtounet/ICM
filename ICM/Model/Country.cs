namespace ICM.Model
{
    public class Country
    {
        public string Name {
            get; set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}