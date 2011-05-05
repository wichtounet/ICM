namespace ICM.Model
{
    /// <summary>
    ///  A User, represents a user along with authorization level and password. 
    /// </summary>
    /// <remarks>Kean Mariotti</remarks>
    public class User
    {
        ///<summary>
        /// The login of the user
        ///</summary>
        public string Login
        {
            get;
            set;
        }

        ///<summary>
        /// The autorization level of the user
        ///</summary>
        public bool Admin
        {
            get;
            set;
        }

        /// <summary>
        ///  The password of the user.
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }
}