namespace ICM.Model
{
    /// <summary>
    ///  A Continent, represents a way to group countries. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Continent
    {
        ///<summary>
        /// The name of the continent
        ///</summary>
        public string Name {
            get; set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}