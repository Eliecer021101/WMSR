using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using WMSR.Models;

namespace WMSR
{
    public partial class Prefijar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 Validar sesión activa
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }

        // PREFIJAR POR ALBARÁN
        protected void btnPrefijarAlbaran_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;
            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            // ✅ Validación de campo
            if (string.IsNullOrWhiteSpace(txtAlbaran.Text))
            {
                lblResultado.Text = "⚠️ Debe ingresar un número de albarán.";
                lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            if (!int.TryParse(txtAlbaran.Text, out int albRecCod))
            {
                lblResultado.Text = "⚠️ El campo de albarán debe ser numérico.";
                lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_MoverAlbaranPrefijar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AlbRecCod", albRecCod);
                cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblResultado.Text = "✅ Piezas ubicadas correctamente en la zona seleccionada.";
                    lblResultado.ForeColor = System.Drawing.Color.Green;
                }
                catch (SqlException ex)
                {
                    lblResultado.Text = "⚠️ Error SQL: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
                catch (Exception ex)
                {
                    lblResultado.Text = "⚠️ Error: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        //  PREFIJAR POR LOTE
        protected void btnPrefijarLote_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;
            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            // ✅ Validar campos vacíos
            if (string.IsNullOrWhiteSpace(txtLote.Text) || string.IsNullOrWhiteSpace(txtReopera.Text))
            {
                lblResultado.Text = "⚠️ Debe ingresar ambos campos: Lote y Reoperado.";
                lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            // ✅ Validar que sean numéricos
            if (!int.TryParse(txtLote.Text, out int barCod))
            {
                lblResultado.Text = "⚠️ El campo Lote debe ser numérico.";
                lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            if (!int.TryParse(txtReopera.Text, out int barCodReo))
            {
                lblResultado.Text = "⚠️ El campo Reopera (BarCodReo) debe ser numérico.";
                lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_MoverLotePrefijar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BarCod", barCod);
                cmd.Parameters.AddWithValue("@BarCodReo", barCodReo);
                cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    // ✅ Mensaje de éxito
                    lblResultado.Text = $"✅ Lote {barCod} - {barCodReo} trasladado correctamente de Zona 4 a Zona 5.";
                    lblResultado.ForeColor = System.Drawing.Color.Green;
                }
                catch (SqlException ex)
                {
                    lblResultado.Text = "⚠️ Error SQL: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
                catch (Exception ex)
                {
                    lblResultado.Text = "⚠️ Error: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
