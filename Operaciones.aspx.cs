using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMSR
{
    public partial class Operaciones : System.Web.UI.Page
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

        protected void btnUbicar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Ubicar.aspx");
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Consultar.aspx");
        }

        protected void btnMovimientos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Movimientos.aspx");
        }

        protected void btnAdministrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx");
        }

    }
}