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

        public override string ToString()
        {
            return FirstName + " " + Name;
        }
    }
}