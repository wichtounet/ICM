﻿using System;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;

namespace ICM
{
    /// <summary>
    ///  This page enable the users to view informations about a Person
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public partial class ShowPerson : Page
    {
        /// <summary>
        /// Load the informations about the person and fill the page. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["person"] != null)
            {
                var id = Request.QueryString["person"].ToInt();

                Extensions.SqlOperation operation = () =>
                {
                    var person = new PersonsDAO().GetPersonByID(id);

                    IDLabel.Text = person.Id.ToString();
                    NameLabel.Text = person.Name;
                    FirstNameLabel.Text = person.FirstName;
                    MailLabel.Text = person.Email;
                    PhoneLabel.Text = person.Phone;
                    DepartmentLabel.Text = person.Department == null ? "" : person.Department.Name;
                    InstitutionLabel.Text = person.Department == null ? "" : person.Department.InstitutionName;

                    StateLabel.Text = person.Archived ? "Oui" : "Non";
                };

                this.Verified(operation, ErrorLabel);
            } 
            else
            {
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
            } 
        }

        /// <summary>
        /// Edit the current person. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void EditPerson(object sender, EventArgs e)
        {
            Response.Redirect("AddPerson.aspx?person=" + IDLabel.Text);
        }

        /// <summary>
        /// Archive the current person.
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
        protected void ArchivePerson(object sender, EventArgs e)
        {
            var id = IDLabel.Text.ToInt();

            Extensions.SqlOperation operation = () =>
            {
                new PersonsDAO().ArchivePerson(id);

                Response.Redirect("ShowPerson.aspx?person=" + id);
            };

            this.Verified(operation, ErrorLabel);
        }
    }
}