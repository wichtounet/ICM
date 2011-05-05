using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using ICM.Model;

namespace ICM.Utils
{
    /// <summary>
    ///  Contains several extensions methods useful to make easier the development. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public static class Extensions
    {
        public static bool ContainsDepartmentWithName(this List<Department> departments, string name)
        {
            return departments.Any(department => department.Name.Equals(name));
        }

        public static User GetUserByLogin(this List<User> users, string login)
        {
            return users.FirstOrDefault(user => user.Login.Equals(login));
        }

        public static int ToInt(this String str)
        {
            return Convert.ToInt16(str);
        }

        public static int? ToIntStrict(this String str)
        {
            if (str == null)
            {
                return null;
            }

            return Convert.ToInt16(str);
        }

        ///<summary>
        /// Bind the list to a datasource and set the text field and value field
        ///</summary>
        ///<param name="list">this</param>
        ///<param name="dataSource">The datasource</param>
        ///<param name="textField">The text field</param>
        ///<param name="valueField">The value field</param>
        ///<typeparam name="T">Type of data in the data source</typeparam>
        public static void DataBind<T>(this DropDownList list, List<T> dataSource, string textField, string valueField )
        {
            list.Items.Clear();
            list.DataSource = dataSource;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        ///<summary>
        /// Bind the list to a datasource and set the text field and value field and add an empty item at the start of the list
        ///</summary>
        ///<param name="list">this</param>
        ///<param name="dataSource">The datasource</param>
        ///<param name="textField">The text field</param>
        ///<param name="valueField">The value field</param>
        ///<typeparam name="T">Type of data in the data source</typeparam>
        public static void DataBindWithEmptyElement<T>(this DropDownList list, List<T> dataSource, string textField, string valueField)
        {
            list.Items.Clear();
            list.Items.Add(new ListItem(string.Empty, string.Empty));
            list.AppendDataBoundItems = true;
            list.DataSource = dataSource;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        ///<summary>
        /// An SQL Operation. Necessary to be checked by verified
        ///</summary>
        /// <see cref="Extensions.Verified"/>
        public delegate void SqlOperation();

        ///<summary>
        /// Execute an SQL Operation and check for error. If there is an error, make the label visible, add text of error in it and log the error. This can 
        /// be other kind of operation, but only SqlException is checked. 
        ///</summary>
        ///<param name="page">The page on which we invoke this extension method</param>
        ///<param name="operation">The operation we want to execute</param>
        ///<param name="label">The error label</param>
        public static void Verified(this Page page, SqlOperation operation, Label label)
        {
            try
            {
                operation();
            }
            catch (SqlException exception)
            {
                label.Visible = true;

                if(exception.Message.StartsWith("Timeout expired."))
                {
                    label.Text = "Cet objet est verrouillé par un autre utilisateur, vous ne pourrez le modifier que lorsqu'il aura terminé. ";

                    LogManager.GetLogger(page.GetType().FullName).Debug("Concurrency modification exception");
                    LogManager.GetLogger(page.GetType().FullName).DebugException("SQL Exception on a page : ", exception);
                } 
                else
                {
                    label.Text = "Erreur de base de données : " + exception.Message;

                    LogManager.GetLogger(page.GetType().FullName).DebugException("SQL Exception on a page : ", exception);
                }
            }
        }
    }
}
