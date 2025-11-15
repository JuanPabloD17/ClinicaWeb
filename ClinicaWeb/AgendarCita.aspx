<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgendarCita.aspx.cs" Inherits="ClinicaWeb.AgendarCita" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <title>Agendar Nueva Cita</title>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

</head>
<body style="background-color:#f8f9fa;">

    <!-- NAVBAR -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary mb-4">
        <div class="container-fluid">

            <a class="navbar-brand fw-bold" href="ListadoCitas.aspx">Sistema de Citas</a>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                    data-bs-target="#navbarNav" aria-controls="navbarNav"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarNav">

                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="ListadoCitas.aspx">Listado</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="AgendarCita.aspx">Agendar</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#" data-bs-toggle="modal" data-bs-target="#modalAyuda">Ayuda</a>
                    </li>
                </ul>

            </div>
        </div>
    </nav>

    <form id="form1" runat="server">

        <div class="container py-5">

            <div class="row justify-content-center">
                <div class="col-md-7">

                    <div class="card shadow-sm border-0">

                        <div class="card-header bg-primary text-white">
                            <h4 class="mb-0">Agendar Nueva Cita</h4>
                        </div>

                        <!-- Mensajes -->
                        <div class="alert alert-success" id="msgOk" runat="server" visible="false"></div>
                        <div class="alert alert-danger" id="msgError" runat="server" visible="false"></div>

                        <div class="card-body">

                            <div class="mb-3">
                                <label class="form-label">Paciente</label>
                                <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                    ControlToValidate="ddlPaciente"
                                    InitialValue=""
                                    ErrorMessage="Seleccione un paciente."
                                    ValidationGroup="Guardar"
                                    CssClass="text-danger" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Doctor</label>
                                <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDoctor" runat="server"
                                    ControlToValidate="ddlDoctor"
                                    InitialValue=""
                                    ErrorMessage="Seleccione un doctor."
                                    ValidationGroup="Guardar"
                                    CssClass="text-danger" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Fecha</label>
                                <asp:TextBox ID="txtFecha" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                    ControlToValidate="txtFecha"
                                    ErrorMessage="Seleccione una fecha."
                                    ValidationGroup="Guardar"
                                    CssClass="text-danger" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Hora</label>
                                <asp:TextBox ID="txtHora" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvHora" runat="server"
                                    ControlToValidate="txtHora"
                                    ErrorMessage="Seleccione una hora."
                                    ValidationGroup="Guardar"
                                    CssClass="text-danger" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Motivo</label>
                                <asp:TextBox ID="txtMotivo" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMotivo" runat="server"
                                    ControlToValidate="txtMotivo"
                                    ErrorMessage="Ingrese un motivo."
                                    ValidationGroup="Guardar"
                                    CssClass="text-danger" />
                            </div>

                            <asp:ValidationSummary ID="vsErrores" runat="server"
                                ValidationGroup="Guardar"
                                CssClass="text-danger mb-3" />

                            <div class="d-flex justify-content-between">

                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="Guardar Cita"
                                    CssClass="btn btn-primary"
                                    ValidationGroup="Guardar"
                                    OnClick="btnGuardar_Click" />

                                <asp:Button ID="btnVolver" runat="server"
                                    Text="Volver al Listado"
                                    CssClass="btn btn-secondary"
                                    CausesValidation="False"
                                    OnClick="btnVolver_Click" />

                            </div>

                        </div>
                    </div>

                </div>
            </div>

        </div>

        <!-- FOOTER -->
        <footer class="text-center py-3 mt-4" style="color:#6c757d; font-size:0.9rem;">
            Desarrollado por <strong>Juan Pablo Domínguez</strong> — 2025<br />
            Sistema de Agendamiento de Citas · Prueba Técnica
        </footer>

    </form>

    <!-- MODAL -->
    <div class="modal fade" id="modalAyuda" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Acerca del Sistema</hh5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <p>
                        Este sistema permite registrar citas médicas de manera rápida y eficiente,
                        verificando disponibilidad de pacientes y doctores.
                    </p>

                    <p class="fw-bold mt-3">Autor</p>
                    <p>Juan Pablo Domínguez — Ingeniero de Sistemas</p>
                </div>

                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>

            </div>
        </div>
    </div>

    <!-- Bootstrap JS  -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
