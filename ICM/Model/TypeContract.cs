namespace ICM.Model
{
    /// <summary>
    /// Represents the type of a contract stored in the database.
    /// </summary>
    /// <remarks>Kean Mariotti</remarks>
    public class TypeContract
    {
        /// <summary>
        /// The contract type's name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a string representing the contract type.
        /// </summary>
        /// <returns>The contract type's name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}