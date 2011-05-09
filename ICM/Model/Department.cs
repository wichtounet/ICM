namespace ICM.Model
{
    /// <summary>
    /// Class department, represents a department belonging to an institution.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Gives access to the department's id.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the department's name.
        /// </summary>
        public string Name {
            get; 
            set;
        }

        /// <summary>
        /// Gives access to the institution's id to which the department belongs.
        /// </summary>
        public int InstitutionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the insitution's name to which the department belongs.
        /// </summary>
        public string InstitutionName
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gives access to the insitution's name followed by the department's name.
        /// </summary>
        public string departementInstitution
        {
            get { return InstitutionName + ": " + Name; }
        }

        /// <summary>
        /// Gives access to the institution's city to which the department belongs.
        /// </summary>
        public string InstitutionCity
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institutin's country to which the department belongs.
        /// </summary>
        public string InstitutionCountry
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's language to which the department belongs.
        /// </summary>
        public string InstitutionLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// String representing the department.
        /// </summary>
        /// <returns>The department's name.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}