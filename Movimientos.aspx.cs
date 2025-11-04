using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMSR
{
    public partial class Movimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 Protección de sesión
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx"); // Si no hay sesión, redirige al login
                return;
            }
        }

        protected void btnPrefijar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Prefijar.aspx");
        }
        protected void btnConformar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Conformar.aspx");
        }


    }
}