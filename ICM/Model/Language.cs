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

        public override string ToString()
        {
            return Name;
        }
    }
}