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
            // 🔒 Protección de sesión
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx"); // Si no hay sesión, redirige al login
                return;
            }
        }

        // ✅ Ejecutar SP de ubicación
        private bool EjecutarUbicacion(int albRecCod, int zona_id, int capacidad_id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;

            // ✅ Obtener el ID del usuario desde la sesión
            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UbicarPiezasDesdeAlb", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AlbRecCod", SqlDbType.Int).Value = albRecCod;
                    cmd.Parameters.Add("@zona_id", SqlDbType.Int).Value = zona_id;
                    cmd.Parameters.Add("@usuario_id", SqlDbType.Int).Value = usuarioId;
                    cmd.Parameters.Add("@capacidad_id", SqlDbType.Int).Value = capacidad_id; // Nuevo parámetro - tipo empaque

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblResultado.Text = "✅ Piezas ubicadas correctamente en la zona seleccionada.";
                lblResultado.ForeColor = System.Drawing.Color.Green;

                return true; // Éxito
            }
            catch (SqlException ex)
            {
                lblResultado.Text = "⚠️ Error SQL: " + ex.Message;
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            catch (Exception ex)
            {
                lblResultado.Text = "⚠️ Error: " + ex.Message;
                lblResultado.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }

        protected void btnUbicar_Click(object sender, EventArgs e)
        {
            // 1️⃣ Validar campos
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

            int zonaSeleccionada = Convert.ToInt32(listZonas.SelectedValue);
            int capacidadId = 1; // valor por defecto

            // 2️⃣ Validar tipo de empaque solo si zona es 1 o 2
            if (zonaSeleccionada == 1 || zonaSeleccionada == 2)
            {
                if (rbRollo.Checked)
                    capacidadId = 2; // ROLLO
                else if (rbTubular.Checked)
                    capacidadId = 3; // TUBULAR
                else if (rbTalego.Checked)
                    capacidadId = 4; // TALEGO
                else
                {
                    lblResultado.Text = "⚠️ Debes seleccionar un tipo de empaque para esta zona (1 o 2).";
                    lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                    return;
                }
            }

            // 3️⃣ Ejecutar procedimiento de ubicación
            bool ubicacionExitosa = EjecutarUbicacion(albRecCod, zonaSeleccionada, capacidadId);

            // 4️⃣ Si fue correcta, generar automáticamente la previsualización de etiquetas
            if (ubicacionExitosa)
            {
                DataTable dt = ObtenerEtiquetas(albRecCod);

                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    lblResultado.Text = "✅ Piezas ubicadas y etiquetas generadas correctamente. (" + dt.Rows.Count + " etiquetas)";
                    lblResultado.ForeColor = System.Drawing.Color.Green;
                    Session["EtiquetasPendientes"] = dt;
                }
                else
                {
                    lblResultado.Text = "⚠️ Piezas ubicadas, pero no se generaron etiquetas para este albarán.";
                    lblResultado.ForeColor = System.Drawing.Color.OrangeRed;
                }
            }
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

                cmd.CommandTimeout = 120;// 2 minutos de espera

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
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

                //limpiar despues de imprimir
                LimpiarCampos();

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
        //metodo limpiar
        protected void LimpiarCampos()
        {
            try
            {
                // 🔹 Limpiar campos de entrada
                txtAlbaran.Text = string.Empty;
                listZonas.ClearSelection();

                // 🔹 Limpiar resultados o mensajes
                lblResultado.Text = string.Empty;
                lblResultado.ForeColor = System.Drawing.Color.Black;

                // 🔹 Limpiar la grilla de etiquetas
                GridView1.DataSource = null;
                GridView1.DataBind();

                // 🔹 Limpiar variables de sesión temporales
                Session.Remove("EtiquetasPendientes");

                // 🔹 Devolver el foco al primer campo
                txtAlbaran.Focus();
            }
            catch (Exception ex)
            {
                lblResultado.Text = "⚠️ Error al limpiar: " + ex.Message;
                lblResultado.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }

    }
}
