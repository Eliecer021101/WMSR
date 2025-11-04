using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WMSR
{
    public partial class Conformar : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
                lblMensaje.Text = "🔎 Ingrese un lote y presione Buscar.";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarGridPiezas();
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string lote = txtLote.Text.Trim();
                string reo = txtReopera.Text.Trim();
                string pieza = txtPieza.Text.Trim();
                int usuario = Convert.ToInt32(Session["UsuarioId"]);

                if (string.IsNullOrEmpty(lote) || string.IsNullOrEmpty(pieza))
                {
                    lblMensaje.Text = "⚠ Ingrese el lote y el código de pieza.";
                    lblMensaje.CssClass = "msg-err";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLecturaPieza", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCod", lote);
                    cmd.Parameters.AddWithValue("@BarCodReo", reo);
                    cmd.Parameters.AddWithValue("@NumeroPieza", pieza);
                    cmd.Parameters.AddWithValue("@usuario_id", usuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMensaje.Text = $"✅ Pieza {pieza} registrada correctamente.";
                lblMensaje.CssClass = "msg-ok";

                // Refrescar tabla automáticamente
                ActualizarGridPiezas();

                txtPieza.Text = "";
                txtPieza.Focus();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "❌ Error al registrar lectura: " + ex.Message;
                lblMensaje.CssClass = "msg-err";
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                string lote = txtLote.Text.Trim();
                string reo = txtReopera.Text.Trim();
                int usuario = Convert.ToInt32(Session["UsuarioId"]);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_VerificarYConformarLote", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCod", lote);
                    cmd.Parameters.AddWithValue("@BarCodReo", reo);
                    cmd.Parameters.AddWithValue("@usuario_id", usuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMensaje.Text = "✅ Lote verificado y conformado correctamente.";
                lblMensaje.CssClass = "msg-ok";
                ActualizarGridPiezas();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "❌ Error al conformar lote: " + ex.Message;
                lblMensaje.CssClass = "msg-err";
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                int usuario = Convert.ToInt32(Session["UsuarioId"]);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_LimpiarConfirmacionesUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", usuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMensaje.Text = "🧹 Confirmaciones eliminadas correctamente.";
                lblMensaje.CssClass = "msg-ok";

                gvPendientes.DataSource = null;
                gvPendientes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "❌ Error al limpiar confirmaciones: " + ex.Message;
                lblMensaje.CssClass = "msg-err";
            }
        }

        private void ActualizarGridPiezas()
        {
            try
            {
                string lote = txtLote.Text.Trim();
                string reo = txtReopera.Text.Trim();
                int usuario = Convert.ToInt32(Session["UsuarioId"]);

                if (string.IsNullOrEmpty(lote))
                    return;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarPiezasLoteConformado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCod", lote);
                    cmd.Parameters.AddWithValue("@BarCodReo", reo);
                    cmd.Parameters.AddWithValue("@usuario_id", usuario);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    try
                    {
                        da.Fill(dt);

                        // Si no hubo error y hay datos, enlazamos el GridView
                        gvPendientes.DataSource = dt;
                        gvPendientes.DataBind();

                        // Mostrar resumen
                        int confirmadas = dt.Select("Estado = 'Confirmada'").Length;
                        lblMensaje.Text = $"✅ {confirmadas} de {dt.Rows.Count} piezas confirmadas del lote {lote}-{reo}.";
                        lblMensaje.CssClass = "msg-ok";
                    }
                    catch (SqlException ex)
                    {
                        // Si el SP lanza RAISERROR (lote no ubicado)
                        lblMensaje.Text = ex.Message;
                        lblMensaje.CssClass = "msg-warn"; // color naranja o amarillo
                        gvPendientes.DataSource = null;
                        gvPendientes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "❌ Error al buscar piezas: " + ex.Message;
                lblMensaje.CssClass = "msg-err"; // rojo
            }
        }


        protected void gvPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = e.Row.Cells[3].Text.Trim();

                if (estado == "Confirmada")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#d4edda"); // verde claro
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#155724");
                }
                else if (estado == "Sin confirmar")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f8d7da"); // rojo claro
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#721c24");
                }
            }
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }
    }
}
