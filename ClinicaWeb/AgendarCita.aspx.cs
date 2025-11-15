using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace ClinicaWeb
{
    public partial class AgendarCita : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPacientes();
                CargarDoctores();
            }
        }


        // CARGA DE PACIENTES
        private void CargarPacientes()
        {
            using (var db = new ClinicaDBEntities())
            {
                ddlPaciente.DataSource = db.Paciente
                    .Select(p => new { p.Id, Nombre = p.Nombre + " " + p.Apellido })
                    .ToList();

                ddlPaciente.DataTextField = "Nombre";
                ddlPaciente.DataValueField = "Id";
                ddlPaciente.DataBind();

                ddlPaciente.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
        }


        // CARGA DE DOCTORES
        private void CargarDoctores()
        {
            using (var db = new ClinicaDBEntities())
            {
                ddlDoctor.DataSource = db.Doctor
                    .Select(d => new { d.Id, Nombre = d.Nombre + " " + d.Apellido })
                    .ToList();

                ddlDoctor.DataTextField = "Nombre";
                ddlDoctor.DataValueField = "Id";
                ddlDoctor.DataBind();

                ddlDoctor.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
        }

        // BOTÓN GUARDAR
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            msgOk.Visible = false;
            msgError.Visible = false;

            try
            {
                // 1. Validación de formato
                if (!DateTime.TryParse($"{txtFecha.Text} {txtHora.Text}", out DateTime fechaHora))
                {
                    MostrarError("Debe seleccionar fecha y hora válidas.");
                    return;
                }

                if (fechaHora < DateTime.Now)
                {
                    MostrarError("No se pueden agendar citas en el pasado.");
                    return;
                }

                // 2. Validación de selección
                if (string.IsNullOrEmpty(ddlPaciente.SelectedValue))
                {
                    MostrarError("Debe seleccionar un paciente.");
                    return;
                }

                if (string.IsNullOrEmpty(ddlDoctor.SelectedValue))
                {
                    MostrarError("Debe seleccionar un doctor.");
                    return;
                }

                int idPaciente = int.Parse(ddlPaciente.SelectedValue);
                int idDoctor = int.Parse(ddlDoctor.SelectedValue);

                // Intervalo mínimo entre citas
                int duracionMinutos = 30;

                using (var db = new ClinicaDBEntities())
                {
                    // 3. Regla: No doble exacto para paciente
                    bool pacienteExacto = db.Cita.Any(c =>
                        c.IdPaciente == idPaciente &&
                        c.FechaHora == fechaHora &&
                        c.Estado == "Programada");

                    if (pacienteExacto)
                    {
                        MostrarError("Este paciente ya tiene una cita en esa fecha y hora.");
                        return;
                    }

                    // 4. Regla: No doble exacto para doctor
                    bool doctorExacto = db.Cita.Any(c =>
                        c.IdDoctor == idDoctor &&
                        c.FechaHora == fechaHora &&
                        c.Estado == "Programada");

                    if (doctorExacto)
                    {
                        MostrarError("El doctor no está disponible en esa fecha y hora.");
                        return;
                    }

                    // 5. Regla avanzada: Rangos para paciente
                    bool pacienteRango = db.Cita.Any(c =>
                        c.IdPaciente == idPaciente &&
                        c.Estado == "Programada" &&
                        (DbFunctions.DiffMinutes(c.FechaHora, fechaHora) ?? 99999) < duracionMinutos &&
                        (DbFunctions.DiffMinutes(c.FechaHora, fechaHora) ?? 99999) > -duracionMinutos
                    );

                    if (pacienteRango)
                    {
                        MostrarError($"El paciente tiene otra cita en un rango menor a {duracionMinutos} minutos.");
                        return;
                    }

                    // 6. Regla avanzada: Rangos para doctor
                    bool doctorRango = db.Cita.Any(c =>
                        c.IdDoctor == idDoctor &&
                        c.Estado == "Programada" &&
                        (DbFunctions.DiffMinutes(c.FechaHora, fechaHora) ?? 99999) < duracionMinutos &&
                        (DbFunctions.DiffMinutes(c.FechaHora, fechaHora) ?? 99999) > -duracionMinutos
                    );

                    if (doctorRango)
                    {
                        MostrarError($"El doctor tiene otra cita en un rango menor a {duracionMinutos} minutos.");
                        return;
                    }
 
                    // 7. GUARDAR LA CITA
                    var cita = new Cita
                    {
                        IdPaciente = idPaciente,
                        IdDoctor = idDoctor,
                        FechaHora = fechaHora,
                        Motivo = txtMotivo.Text,
                        Estado = "Programada"
                    };

                    db.Cita.Add(cita);
                    db.SaveChanges();
                }

                MostrarExito("La cita fue registrada exitosamente.");
                LimpiarCampos();
            }
            catch (FormatException)
            {
                MostrarError("Revise los valores ingresados. Hay formatos incorrectos.");
            }
            catch (Exception)
            {
                MostrarError("No se pudo registrar la cita. Verifique la información.");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoCitas.aspx");
        }

        // MÉTODOS AUXILIARES DE UI
        private void MostrarExito(string mensaje)
        {
            msgOk.InnerText = mensaje;
            msgOk.Visible = true;
            msgError.Visible = false;
        }

        private void MostrarError(string mensaje)
        {
            msgError.InnerText = mensaje;
            msgError.Visible = true;
            msgOk.Visible = false;
        }

        private void LimpiarCampos()
        {
            ddlPaciente.SelectedIndex = 0;
            ddlDoctor.SelectedIndex = 0;
            txtFecha.Text = "";
            txtHora.Text = "";
            txtMotivo.Text = "";
        }
    }
}
