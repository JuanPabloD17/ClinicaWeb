using System;
using System.Linq;

namespace ClinicaWeb
{
    public partial class PruebaDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new ClinicaDBEntities())
            {
                var total = db.Doctor.Count();
                Response.Write("Doctores en BD: " + total);
            }
        }
    }
}
