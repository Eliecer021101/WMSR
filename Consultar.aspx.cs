using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WMSR
{
    public partial class Consultar : System.Web.UI.Page
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

        private void ObtenerDatos(string codigo, string tipoConsulta, string codigoReopera = "0")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WMSRConnection"].ConnectionString;
            string storedProcedure = "";

            switch (tipoConsulta)
            {
                case "albaran":
                    storedProcedure = "sp_ConsultarAlbaranConUbicaciones";
                    break;
                case "lote":
                    storedProcedure = "sp_ConsultarUbicacionPorBarCod";
                    break;
                case "piezas":
                    storedProcedure = "sp_ConsultarUbicacionRollo";
                    break;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 🔹 Asignar parámetros según tipo de consulta
                switch (tipoConsulta)
                {
                    case "albaran":
                        cmd.Parameters.AddWithValue("@AlbRecCod", codigo);
                        break;

                    case "lote":
                        cmd.Parameters.AddWithValue("@BarCod", codigo);

                        int codigoReoperaInt = 0;
                        if (!string.IsNullOrEmpty(codigoReopera) && int.TryParse(codigoReopera, out int val))
                            codigoReoperaInt = val;

                        cmd.Parameters.AddWithValue("@BarCodReo", codigoReoperaInt);
                        break;

                    case "piezas":
                        cmd.Parameters.AddWithValue("@numero_pieza", codigo);
                        break;
                }

                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    try
                    {
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        // ⚠️ Captura exacta de RAISERROR personalizado desde SQL
                        string sqlMessage = ex.Errors.Count > 0 ? ex.Errors[0].Message : ex.Message;

                        lblResultado.Text = sqlMessage;

                        // 🎨 Asignar color automático según el emoji del mensaje SQL
                        if (sqlMessage.StartsWith("⚠️"))
                            lblResultado.CssClass = "msg-warn";
                        else if (sqlMessage.StartsWith("❌"))
                            lblResultado.CssClass = "msg-err";
                        else
                            lblResultado.CssClass = "msg-ok";

                        gvDatos.DataSource = null;
                        gvDatos.DataBind();
                        return;
                    }

                    // ⚙️ Validar si hay datos
                    if (dt.Rows.Count == 0)
                    {
                        lblResultado.Text = "⚠️ No se encontraron registros.";
                        lblResultado.CssClass = "msg-warn";
                        gvDatos.DataSource = null;
                        gvDatos.DataBind();
                        return;
                    }

                    // 🧩 Generar columnas dinámicamente según tipo de consulta
                    gvDatos.AutoGenerateColumns = false;
                    gvDatos.Columns.Clear();

                    switch (tipoConsulta)
                    {
                        case "albaran":
                            gvDatos.Columns.Add(new BoundField { DataField = "CliNom", HeaderText = "Cliente" });
                            gvDatos.Columns.Add(new BoundField { DataField = "AlbRef", HeaderText = "Referencia" });
                            gvDatos.Columns.Add(new BoundField { DataField = "zona", HeaderText = "Zona" });
                            gvDatos.Columns.Add(new BoundField { DataField = "celda", HeaderText = "Celda" });
                            gvDatos.Columns.Add(new BoundField { DataField = "piezas_en_celda", HeaderText = "Piezas en Celda" });
                            break;

                        case "lote":
                            gvDatos.Columns.Add(new BoundField { DataField = "CliNom", HeaderText = "Cliente" });
                            gvDatos.Columns.Add(new BoundField { DataField = "BarColNom", HeaderText = "Color" });
                            gvDatos.Columns.Add(new BoundField { DataField = "AlbRef", HeaderText = "Referencia" });
                            gvDatos.Columns.Add(new BoundField { DataField = "zona", HeaderText = "Zona" });
                            gvDatos.Columns.Add(new BoundField { DataField = "celda", HeaderText = "Celda" });
                            gvDatos.Columns.Add(new BoundField { DataField = "piezas_en_celda", HeaderText = "Piezas en Celda" });
                            break;

                        case "piezas":
                            gvDatos.Columns.Add(new BoundField { DataField = "numero_pieza", HeaderText = "N° Pieza" });
                            gvDatos.Columns.Add(new BoundField { DataField = "albaran", HeaderText = "N° Albarán" });
                            gvDatos.Columns.Add(new BoundField { DataField = "zona", HeaderText = "Zona" });
                            gvDatos.Columns.Add(new BoundField { DataField = "celda", HeaderText = "Celda" });
                            break;
                    }

                    // 🔹 Enlazar resultados al GridView
                    gvDatos.DataSource = dt;
                    gvDatos.DataBind();

                    lblResultado.Text = "✅ Consulta completada correctamente.";
                    lblResultado.CssClass = "msg-ok";
                }
                catch (Exception ex)
                {
                    lblResultado.Text = "❌ Error al consultar: " + ex.Message;
                    lblResultado.CssClass = "msg-err";
                    gvDatos.DataSource = null;
                    gvDatos.DataBind();
                }
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            {
                string codigoAlbaran = txtAlbaran.Text.Trim();
                string codigoLote = txtLote.Text.Trim();
                string codigoPiezas = txtPiezas.Text.Trim();
                string codigoReopera = txtReopera.Text.Trim();

                int inputsLlenos = 0;
                if (!string.IsNullOrEmpty(codigoAlbaran)) inputsLlenos++;
                if (!string.IsNullOrEmpty(codigoLote)) inputsLlenos++;
                if (!string.IsNullOrEmpty(codigoPiezas)) inputsLlenos++;

                if (inputsLlenos == 0)
                {
                    lblResultado.Text = "Por favor, ingrese un código.";
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else if (inputsLlenos > 1)
                {
                    lblResultado.Text = "Por favor, ingrese solo un código a la vez.";
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (!string.IsNullOrEmpty(codigoAlbaran))
                {
                    ObtenerDatos(codigoAlbaran, "albaran");
                    Session["UltimoCampoBusqueda"] = "albaran";//captura campo de busqueda
                }
                else if (!string.IsNullOrEmpty(codigoLote))
                {
                    ObtenerDatos(codigoLote, "lote",codigoReopera);
                    Session["UltimoCampoBusqueda"] = "lote";//captura campo de busqueda
                }
                else if (!string.IsNullOrEmpty(codigoPiezas))
                {
                    ObtenerDatos(codigoPiezas, "piezas");
                    Session["UltimoCampoBusqueda"] = "piezas";//captura campo de busqueda
                }
                else
                {
                    lblResultado.Text = "Ingrese un código en alguno de los campos.";
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //captura el ultimo campo de busqueda utilizado
            string ultimoCampo = Session["UltimoCampoBusqueda"] as string;

            //limpiar campos de entrada
            txtLote.Text = string.Empty;
            txtAlbaran.Text = string.Empty;
            txtPiezas.Text = string.Empty;

            //limpiar etuquetas o mensajes
            lblResultado.Text = string.Empty;

            //limpiar el datagridview
            gvDatos.DataSource = null;
            gvDatos.DataBind();

            //devolver al ultimo campo utilizado
            if (! string.IsNullOrEmpty(ultimoCampo))
            {
                switch(ultimoCampo.ToLower())
                {
                    case "lote":
                        txtLote.Focus();
                        break;
                    case "piezas":
                        txtPiezas.Focus();
                        break;
                    case "albaran":
                        txtAlbaran.Focus();
                        break;
                }
            }
            else
            {
                //devuelve al primero si nunca se hizo busqueda
                txtLote.Focus();
            }
            
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operaciones.aspx");
        }
    }
}