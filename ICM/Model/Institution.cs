using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICM.Model
{
    /// <summary>
    /// Class institution, represents an institution in a contract.
    /// </summary>
    /// <remarks>Kean Mariotti</remarks>
    public class Institution
    {
        /// <summary>
        /// Gives access to the institution's id.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gives access to the institution's name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's description.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's city.
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's interest.
        /// </summary>
        public string Interest
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's Language.
        /// </summary>
        public Language Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's country.
        /// </summary>
        public Country Country
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's departments.
        /// </summary>
        public List<Department> Departments
        {
            get;
            set;
        }

        /// <summary>
        /// Gives access to the institution's state (archived or not archived).
        /// </summary>
        public bool IsArchived
        {
            get;
            set;
        }

        /// <summary>
        /// String representing the institution.
        /// </summary>
        /// <returns>The institution's name</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Institution constructor.
        /// </summary>
        /// <param name="Id">Institution's id.</param>
        /// <param name="Name">Institutin's name.</param>
        /// <param name="Description">Institution's description.</param>
        /// <param name="City">Institution's city.</param>
        /// <param name="Interest">Institutin's insterest.</param>
        /// <param name="Language">Institution's language.</param>
        /// <param name="Country">Institution's country.</param>
        /// <param name="Departments">Institution's department.</param>
        /// <param name="IsArchived">Institution's state (archived or not archived).</param>
        public Institution( int Id,
                            string Name,
                            string Description,
                            string City,
                            string Interest,
                            Language Language,
                            Country Country,
                            List<Department> Departments,
                            bool IsArchived)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.City = City;
            this.Interest = Interest;
            this.Language = Language;
            this.Country = Country;
            this.Departments = Departments;
            this.IsArchived = IsArchived;
        }
    }
}