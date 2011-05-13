namespace ICM.Model
{
    /// <summary>
    ///  A Country, represents a country in which the institutions or the person is. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Country
    {
        ///<summary>
        /// The name of the country
        ///</summary>
        public string Name {
            get; set;
        }

        ///<summary>
        /// The continent the country is located on. 
        ///</summary>
        public Continent Continent
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Name;
        }
    }
}