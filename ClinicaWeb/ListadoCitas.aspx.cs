using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class ListadoCitas : Page
    {
        private string SortField
        {
            get => ViewState["SortField"] as string ?? "Fecha";
            set => ViewState["SortField"] = value;
        }

        private string SortDirection
        {
            get => ViewState["SortDirection"] as string ?? "ASC";
            set => ViewState["SortDirection"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltros();
                CargarCitas();
            }
        }


        // 1.CARGA DE FILTROS
        private void CargarFiltros()
        {
            using (var db = new ClinicaDBEntities())
            {
                // PACIENTES
                ddlFiltroPaciente.DataSource = db.Paciente
                    .Select(p => new { p.Id, Nombre = p.Nombre + " " + p.Apellido })
                    .ToList();
                ddlFiltroPaciente.DataTextField = "Nombre";
                ddlFiltroPaciente.DataValueField = "Id";
                ddlFiltroPaciente.DataBind();
                ddlFiltroPaciente.Items.Insert(0, new ListItem("-- Todos --", ""));

                // DOCTORES
                ddlFiltroDoctor.DataSource = db.Doctor
                    .Select(d => new { d.Id, Nombre = d.Nombre + " " + d.Apellido })
                    .ToList();
                ddlFiltroDoctor.DataTextField = "Nombre";
                ddlFiltroDoctor.DataValueField = "Id";
                ddlFiltroDoctor.DataBind();
                ddlFiltroDoctor.Items.Insert(0, new ListItem("-- Todos --", ""));

                // ESTADO
                ddlFiltroEstado.Items.Clear();
                ddlFiltroEstado.Items.Add(new ListItem("-- Todos --", ""));
                ddlFiltroEstado.Items.Add(new ListItem("Programada", "Programada"));
                ddlFiltroEstado.Items.Add(new ListItem("Cancelada", "Cancelada"));
            }
        }
        // 2.CARGAR CITAS + FILTROS + SORTING
        private void CargarCitas()
        {
            try
            {
                using (var db = new ClinicaDBEntities())
                {
                    var query = db.Cita.AsQueryable();

                    // FILTRO POR PACIENTE
                    if (!string.IsNullOrEmpty(ddlFiltroPaciente.SelectedValue))
                    {
                        int idP = int.Parse(ddlFiltroPaciente.SelectedValue);
                        query = query.Where(c => c.IdPaciente == idP);
                    }

                    // FILTRO POR DOCTOR
                    if (!string.IsNullOrEmpty(ddlFiltroDoctor.SelectedValue))
                    {
                        int idD = int.Parse(ddlFiltroDoctor.SelectedValue);
                        query = query.Where(c => c.IdDoctor == idD);
                    }

                    // FILTRO POR ESTADO
                    if (!string.IsNullOrEmpty(ddlFiltroEstado.SelectedValue))
                    {
                        string est = ddlFiltroEstado.SelectedValue;
                        query = query.Where(c => c.Estado == est);
                    }

                    // CARGAMOS RESULTADOS
                    var datosBD = query
                        .Select(c => new
                        {
                            c.Id,
                            Paciente = c.Paciente.Nombre + " " + c.Paciente.Apellido,
                            Doctor = c.Doctor.Nombre + " " + c.Doctor.Apellido,
                            c.FechaHora,
                            c.Estado
                        })
                        .ToList();

                    var datosFormateados = datosBD
                        .Select(d => new
                        {
                            d.Id,
                            d.Paciente,
                            d.Doctor,
                            Fecha = d.FechaHora.ToString("yyyy-MM-dd"),
                            Hora = d.FechaHora.ToString("HH:mm"),
                            d.Estado,
                            EstadoHtml = d.Estado == "Programada"
                                ? "<span class='badge bg-success'>Programada</span>"
                                : "<span class='badge bg-danger'>Cancelada</span>"
                        })
                        .ToList();

                    // ORDENAMIENTO
                    if (SortDirection == "ASC")
                        datosFormateados = datosFormateados.OrderBy(x => x.GetType().GetProperty(SortField).GetValue(x)).ToList();
                    else
                        datosFormateados = datosFormateados.OrderByDescending(x => x.GetType().GetProperty(SortField).GetValue(x)).ToList();

                    gvCitas.DataSource = datosFormateados;
                    gvCitas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar citas.";
                lblError.Visible = true;
            }
        }


        // 3. BOTÓN CANCELAR CITA
        protected void gvCitas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                if (!int.TryParse(e.CommandArgument.ToString(), out int idCita))
                {
                    MostrarError("ID de cita inválido.");
                    return;
                }

                try
                {
                    using (var db = new ClinicaDBEntities())
                    {
                        var cita = db.Cita.Find(idCita);
                        if (cita == null)
                        {
                            MostrarError("La cita no existe o fue eliminada.");
                            return;
                        }

                        // Si ya está cancelada, no hacer nada más
                        if (cita.Estado == "Cancelada")
                        {
                            MostrarError("Esta cita ya fue cancelada previamente.");
                            return;
                        }

                        // Regla de 24 horas
                        double horasRestantes = (cita.FechaHora - DateTime.Now).TotalHours;

                        if (horasRestantes < 24)
                        {
                            MostrarError("Solo se pueden cancelar citas con al menos 24 horas de antelación.");
                            return;
                        }

                        // Cancelar cita
                        cita.Estado = "Cancelada";
                        db.SaveChanges();

                        // Mensaje de éxito
                        MostrarSuccess("La cita fue cancelada exitosamente.");

                        CargarCitas();


                        double horas = (cita.FechaHora - DateTime.Now).TotalHours;

                        if (horas < 24)
                        {
                            MostrarError("Solo se pueden cancelar citas con al menos 24 horas de antelación.");
                            return;
                        }

                        // Cancelar
                        cita.Estado = "Cancelada";
                        db.SaveChanges();
                    }

                    MostrarSuccess("La cita fue cancelada exitosamente.");
                    CargarCitas();
                }
                catch
                {
                    MostrarError("Error al cancelar la cita.");
                }
            }
        }


        // 4.PAGINACIÓN
        protected void gvCitas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCitas.PageIndex = e.NewPageIndex;
            CargarCitas();
        }

        // 5.SORTING
        protected void gvCitas_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortField == e.SortExpression)
                SortDirection = SortDirection == "ASC" ? "DESC" : "ASC";
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            CargarCitas();
        }


        // 6.FILTRO Y LIMPIAR

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarCitas();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlFiltroPaciente.SelectedIndex = 0;
            ddlFiltroDoctor.SelectedIndex = 0;
            ddlFiltroEstado.SelectedIndex = 0;

            SortField = "Fecha";
            SortDirection = "ASC";

            CargarCitas();
        }


        // 7.BOTÓN NUEVA CITA

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgendarCita.aspx");
        }
        // 8.MENSAJES 

        private void MostrarSuccess(string msg)
        {
            lblSuccess.Text = msg;
            lblSuccess.Visible = true;
            lblError.Visible = false;
        }

        private void MostrarError(string msg)
        {
            lblError.Text = msg;
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }
    }
}
