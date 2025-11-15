<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListadoCitas.aspx.cs" Inherits="ClinicaWeb.ListadoCitas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <title>Listado de Citas</title>

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
                        <a class="nav-link active" href="ListadoCitas.aspx">Listado</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="AgendarCita.aspx">Agendar</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#" data-bs-toggle="modal" data-bs-target="#modalAyuda">Ayuda</a>
                    </li>
                </ul>

            </div>
        </div>
    </nav>

    <form id="form1" runat="server">

        <div class="container">

            <div class="row">
                <div class="col-md-4">

                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">Filtros</h5>
                        </div>

                        <div class="card-body">

                            <!-- FILTRO PACIENTE -->
                            <label class="form-label fw-bold">Paciente</label>
                            <asp:DropDownList ID="ddlFiltroPaciente" runat="server" CssClass="form-select"></asp:DropDownList>

                            <!-- FILTRO DOCTOR -->
                            <label class="form-label fw-bold mt-3">Doctor</label>
                            <asp:DropDownList ID="ddlFiltroDoctor" runat="server" CssClass="form-select"></asp:DropDownList>

                            <!-- FILTRO ESTADO -->
                            <label class="form-label fw-bold mt-3">Estado</label>
                            <asp:DropDownList ID="ddlFiltroEstado" runat="server" CssClass="form-select"></asp:DropDownList>

                            <div class="d-flex justify-content-between mt-4">
                                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
                            </div>

                        </div>
                    </div>

                </div>

                <div class="col-md-8">

                    <div class="card shadow-sm border-0">
                        <div class="card-header bg-primary text-white d-flex justify-content-between">
                            <h4 class="mb-0">Citas Programadas</h4>
                            <asp:Button ID="btnNueva" runat="server" Text="Agendar Cita"
                                CssClass="btn btn-light text-primary fw-bold" OnClick="btnNueva_Click" />
                        </div>

                        <div class="card-body">

                            <!-- Mensajes -->
                            <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success d-block" Visible="false"></asp:Label>
                            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>

                            <!-- GRID -->
                            <asp:GridView
                                ID="gvCitas"
                                runat="server"
                                AutoGenerateColumns="False"
                                AllowPaging="True"
                                PageSize="5"
                                AllowSorting="True"
                                CssClass="table table-hover"
                                HeaderStyle-CssClass="table-primary"
                                RowStyle-CssClass="align-middle"
                                OnPageIndexChanging="gvCitas_PageIndexChanging"
                                OnSorting="gvCitas_Sorting"
                                OnRowCommand="gvCitas_RowCommand">

                                <Columns>

                                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" SortExpression="Paciente" />
                                    <asp:BoundField DataField="Doctor" HeaderText="Doctor" SortExpression="Doctor" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
                                    <asp:BoundField DataField="Hora" HeaderText="Hora" SortExpression="Hora" />

                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <%# Eval("EstadoHtml") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:Button ID="btnCancelar" runat="server"
                                                Text="Cancelar"
                                                CommandName="Cancelar"
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="btn btn-danger btn-sm" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

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

    <!-- MODAL AYUDA -->
    <div class="modal fade" id="modalAyuda" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Acerca del Sistema</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <p>
                        Este sistema permite gestionar citas médicas con filtros avanzados,
                        paginación, ordenamiento y control de disponibilidad.
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

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
