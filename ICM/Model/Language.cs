namespace ICM.Model
{
    /// <summary>
    ///  A Language, represents in what language we communicate with the institution
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Language
    {
        ///<summary>
        /// The name of the language. 
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