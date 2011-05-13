namespace ICM.Model
{
    /// <summary>
    ///  A Role, represents what the person is doing in a contract
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Role
    {
        ///<summary>
        /// The name of the person. 
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