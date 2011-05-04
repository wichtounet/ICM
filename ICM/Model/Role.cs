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

        public override string ToString()
        {
            return Name;
        }
    }
}