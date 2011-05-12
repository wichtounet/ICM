using System;
using System.Collections.Generic;

namespace ICM.Model
{
    /// <summary>
    ///  A Contract with all informations. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public class Contract
    {
        ///<summary>
        /// The Title of the contract
        ///</summary>
        public string Title
        {
            get;
            set;
        }
        ///<summary>
        /// The ID of the contract
        ///</summary>
        public int Id
        {
            get;
            set;
        }
        ///<summary>
        /// The date start of the contract
        ///</summary>
        public DateTime Start
        {
            get;
            set;
        }
        ///<summary>
        /// The end start of the contract
        ///</summary>
        public DateTime End
        {
            get;
            set;
        }
        ///<summary>
        /// The XML content of the contract
        ///</summary>
        public string XmlContent
        {
            get;
            set;
        }
        ///<summary>
        /// The UserName who create the contract
        ///</summary>
        public string User
        {
            get;
            set;
        }
        ///<summary>
        /// The type of the contract
        ///</summary>
        public string Type
        {
            get;
            set;
        }
        ///<summary>
        /// Boolean tag 
        ///</summary>
        public Boolean Archived
        {
            get;
            set;
        }
        ///<summary>
        /// The file ID of the contract
        ///</summary>
        public int fileId
        {
            get;
            set;
        }
        ///<summary>
        /// The contacts of the contract
        ///</summary>
        public List<Person> persons
        {
            get;
            set;
        }
        ///<summary>
        /// The destinations of the contract
        ///</summary>
        public List<Department> departments
        {
            get;
            set;
        }
        ///<summary>
        /// Override ToString
        ///</summary>
        public override string ToString()
        {
            return Title;
        }
    }
}