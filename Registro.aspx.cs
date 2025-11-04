using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMSR
{
    public partial class Registro : System.Web.UI.Page
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

        private void LimpiarCampos()
        {
            txtCedula.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCargo.Text = "";
            txtUsuario.Text = "";
            txtClave.Text = "";

        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(txtCedula.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCargo.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text))

            {
                lblResultado.Text = "Todos los campos son obligatorios.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Obtener los valores de los campos
            string cedula = txtCedula.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string cargo = txtCargo.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();
            int idPerfil = int.Parse(ddlPerfil.SelectedValue);

            bool registrado = false;
            string mensaje = "";

            string connStr = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@cedula_Usuarios", cedula);
                    cmd.Parameters.AddWithValue("@nombre_Usuarios", nombre);
                    cmd.Parameters.AddWithValue("@apellido_Usuarios", apellido);
                    cmd.Parameters.AddWithValue("@cargo_Usuarios", cargo);
                    cmd.Parameters.AddWithValue("@usuario_Usuarios", usuario);
                    cmd.Parameters.AddWithValue("@clave_Usuarios", clave);
                    cmd.Parameters.AddWithValue("@id_Perfil", idPerfil);

                    // Parámetros de salida
                    SqlParameter registradoParam = new SqlParameter("@Registrado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(registradoParam);
                    cmd.Parameters.Add(mensajeParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(registradoParam.Value);
                    mensaje = mensajeParam.Value.ToString();
                }
            }

            // Mostrar el resultado
            lblResultado.Text = mensaje;
            lblResultado.ForeColor = registrado ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            // Limpiar campos si se registró correctamente
            if (registrado)
            {
                LimpiarCampos();
            }
        }



        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }
    }
}