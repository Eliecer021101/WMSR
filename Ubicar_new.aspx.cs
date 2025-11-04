using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMSR
{
    public partial class Ubicar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // ✅ Ejecutar SP de ubicación
        private void EjecutarUbicacion(int albRecCod, string zona)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UbicarPiezasDesdeAlb", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@AlbRecCod", SqlDbType.Int).Value = albRecCod;
                cmd.Parameters.Add("@zona", SqlDbType.VarChar, 50).Value = zona;

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

        protected void btnUbicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAlbaran.Text))
            {
                lblResultado.Text = "Por favor ingrese el código de albarán.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (!int.TryParse(txtAlbaran.Text, out int albRecCod))
            {
                lblResultado.Text = "El código de albarán debe ser un número válido.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (listZonas.SelectedItem == null)
            {
                lblResultado.Text = "Por favor seleccione una zona.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string zonaSeleccionada = listZonas.SelectedItem.Text;
            EjecutarUbicacion(albRecCod, zonaSeleccionada);
        }

        // ✅ Imprimir todas las etiquetas directo
        private void ImprimirEtiquetasPorRollo(int albRecCod)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GenerarZPLEtiquetasPorAlbaran", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AlbRecCod", SqlDbType.Int).Value = albRecCod;

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string zpl = reader["ZPL_Tiquete"].ToString();
                        EnviarAImpresora(zpl); // 🔹 usa método centralizado
                    }
                }
            }

            lblResultado.Text = "✅ Etiquetas generadas e impresas por rollo.";
            lblResultado.ForeColor = System.Drawing.Color.Green;
        }

        // ✅ Método centralizado de impresión (solo por IP)
        private void EnviarAImpresora(string zpl)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            // 🔹 Obtener la IP real del cliente
            string clientIp = Request.UserHostAddress;

            string ip = "";
            int puerto = 9100;

            // 🔹 Consultar impresora en SQL según IP del cliente
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ObtenerImpresoraPorPC", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PcIP", clientIp);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            ip = dr["ZebraIP"].ToString();
                            puerto = Convert.ToInt32(dr["ZebraPort"]);
                        }
                        else
                        {
                            throw new Exception("⚠️ No se encontró impresora configurada para este equipo con IP: " + clientIp);
                        }
                    }
                }
            }

            // 🔹 Enviar el ZPL a la impresora encontrada
            using (TcpClient client = new TcpClient())
            {
                client.Connect(ip, puerto);
                using (StreamWriter writer = new StreamWriter(client.GetStream(), System.Text.Encoding.ASCII))
                {
                    writer.Write(zpl);
                    writer.Flush();
                }
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtAlbaran.Text, out int albRecCod))
            {
                try
                {
                    ImprimirEtiquetasPorRollo(albRecCod);
                }
                catch (Exception ex)
                {
                    lblResultado.Text = "Error al imprimir: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblResultado.Text = "Por favor, introduce un número de albarán válido.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
            }
        }

        // ✅ Obtener etiquetas desde el SP para previsualizar
        private DataTable ObtenerEtiquetas(int albRecCod)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GenerarZPLEtiquetasPorAlbaran", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AlbRecCod", SqlDbType.Int).Value = albRecCod;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        // ✅ Previsualizar tiquetes
        protected void btnPrevisualizar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtAlbaran.Text, out int albRecCod))
            {
                DataTable dt = ObtenerEtiquetas(albRecCod);

                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    lblResultado.Text = "Se generaron " + dt.Rows.Count + " etiquetas. Verifique antes de imprimir.";
                    lblResultado.ForeColor = System.Drawing.Color.Orange;

                    Session["EtiquetasPendientes"] = dt;
                }
                else
                {
                    lblResultado.Text = "⚠️ No hay etiquetas para este albarán.";
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblResultado.Text = "⚠️ Debe ingresar un número de albarán válido.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
            }
        }

        // ✅ Imprimir solo una fila desde el GridView
        protected void gvPrevisualizacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ImprimirFila")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dt = Session["EtiquetasPendientes"] as DataTable;

                if (dt != null && index < dt.Rows.Count)
                {
                    string zpl = dt.Rows[index]["ZPL_Tiquete"].ToString();

                    try
                    {
                        EnviarAImpresora(zpl);
                        lblResultado.Text = "✅ Etiqueta Nº " + dt.Rows[index]["numero_pieza"] + " enviada a la impresora.";
                        lblResultado.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblResultado.Text = "⚠️ Error al imprimir la etiqueta: " + ex.Message;
                        lblResultado.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        // ✅ Imprimir todas las etiquetas confirmadas
        protected void btnConfirmarImpresion_Click(object sender, EventArgs e)
        {
            if (Session["EtiquetasPendientes"] is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string zpl = row["ZPL_Tiquete"].ToString();
                    EnviarAImpresora(zpl);
                }

                lblResultado.Text = "✅ Todas las etiquetas enviadas a la impresora.";
                lblResultado.ForeColor = System.Drawing.Color.Green;

                Session.Remove("EtiquetasPendientes");
            }
            else
            {
                lblResultado.Text = "⚠️ No hay etiquetas cargadas para imprimir.";
                lblResultado.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }
    }
}
