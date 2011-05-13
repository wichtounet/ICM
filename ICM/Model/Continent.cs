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