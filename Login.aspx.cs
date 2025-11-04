using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WMSR
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si el usuario ya inició sesión, redirigir al menú principal
            if (!IsPostBack && Session["UsuarioId"] != null)
            {
                Response.Redirect("Operaciones.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                lblMensaje.Text = "⚠️ Ingrese usuario y contraseña.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario_Usuarios", usuario);
                cmd.Parameters.AddWithValue("@clave_Usuarios", clave);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int idUsuario = Convert.ToInt32(reader["id_Usuarios"]);

                    if (idUsuario > 0)
                    {
                        // ✅ Guardar datos en la sesión
                        Session["UsuarioId"] = idUsuario;
                        Session["UsuarioNombre"] = reader["nombre_Usuarios"].ToString();
                        Session["UsuarioLogin"] = reader["usuario_Usuarios"].ToString();
                        Session["Cargo"] = reader["cargo_Usuarios"].ToString();
                        Session["PerfilId"] = reader["id_Perfil"].ToString();

                        // Redirigir al menú principal
                        Response.Redirect("Operaciones.aspx");
                    }
                    else
                    {
                        lblMensaje.Text = "⚠️ Usuario o contraseña incorrectos.";
                    }
                }
                else
                {
                    lblMensaje.Text = "⚠️ Usuario o contraseña incorrectos.";
                }
            }
        }
    }
}