namespace ICM.Model
{
    /// <summary>
    ///  A Person, represents a single person in a contract. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Person
    {
        ///<summary>
        /// The id of the person
        ///</summary>
        public int Id
        {
            get;
            set;
        }

        ///<summary>
        /// The first name of the person
        ///</summary>
        public string FirstName {
            get; 
            set;
        }

        ///<summary>
        /// The name of the person
        ///</summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        ///  The phone number of the person. 
        /// </summary>
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        /// The email address of the Person. 
        ///</summary>
        public string Email
        {
            get;
            set;
        }

        ///<summary>
        /// A boolean tag indicating 
        ///</summary>
        public bool Archived
        {
            get;
            set;
        }

        ///<summary>
        /// The department
        ///</summary>
        public Department Department
        {
            get;
            set;
        }

        ///<summary>
        /// The "FirstName Name" name couple of the person. 
        ///</summary>
        public string NameFirstName
        {
            get { return FirstName + " " + Name; }
        }

        ///<summary>
        /// The role of the person.
        ///</summary>
        public string Role
        {
            get;
            set;
        }

        ///<summary>
        /// The "Role:NameFirstName" couple of the person. 
        ///</summary>
        public string RoleFirstName
        {
            get { return Role +": " + NameFirstName; }
        }

        ///<summary>
        /// The "Id;Role" couple of the person. 
        ///</summary>
        public string RoleId
        {
            get { return Id + ";" + Role; }
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
            return FirstName + " " + Name;
        }
    }
}