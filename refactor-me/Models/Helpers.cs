using System;
using System.Data.SqlClient;
using System.Web;

namespace Xero.RefactoringExercise.WebApi.Models
{
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", MakeAbsolutePath("~/App_Data"));
            return new SqlConnection(connstr);
        }

        private static string MakeAbsolutePath(string relativePath)
        {
            return HttpContext.Current == null
                ? string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, relativePath.TrimStart('~', '/', '\\'))
                : HttpContext.Current.Server.MapPath(relativePath);
        }
    }
}