<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentManagement.aspx.cs" Inherits="projectStudent.Pages.StudentManagement" %>

<!--esse codigo abaixo para funcionar o ajax que instalamos pelo nuget-->
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>



    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <link href="~/Content/Site.css" rel="stylesheet" type="text/css" />



    <style type="text/css">
        .selected-row {
            background-color: #f0f8ff !important; /* Cor de destaque para a linha selecionada */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="row g-3 p-3 me-0">
        <!--esse ScriptManager é para funcionar o ajax que instalamos pelo nuget
                ele tem que etsa dentro de um form para funcionar-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <h1 class="p-3 mb-2 bg-primary text-white">Cadastrar Aluno</h1>
        <div class="col-auto">
            <asp:Label Text="Nome:" runat="server" AssociatedControlID="txtName" />
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>
        <div class="col-auto">
            <asp:Label Text="Age:" runat="server" AssociatedControlID="txtAge" />
            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" />
        </div>
        <div class="col-auto">
            <asp:Label Text="Data Nascimento:" runat="server" AssociatedControlID="txtBirthDate" />
            <asp:TextBox ID="txtBirthDate" TextMode="Date" runat="server" CssClass="form-control" />
        </div>
        <div class="col-auto">
            <asp:Label Text="Cursos" runat="server" AssociatedControlID="txtCourses" />
            <asp:DropDownList ID="txtCourses" runat="server" CssClass="form-control">
                <asp:ListItem Selected="True" Value="ADS">ADS</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-12 mt-3 px-5 ">
            <div class="row d-flex justify-content-center">
                <asp:Button CssClass="btn btn-success w-25 me-2" Text="Cadastrar" OnClick="Register" runat="server" />
                <!--Deletar e editar-->
                <asp:HiddenField ID="hdnStudentId" runat="server" Value="" />
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-primary w-25" OnClick="EditStudent" />
                <asp:Button ID="btnDeletar" runat="server" Text="Deletar" CssClass="btn btn-danger w-50 mt-1" OnClick="DeleteStudent" OnClientClick="return confirm('Tem certeza que deseja deletar este aluno?');" />

            </div>
            <div class="row">
                <div class="col-12">
                </div>
            </div>
        </div>
        <div>
            <asp:Label ID="lblMensagem" runat="server" ForeColor="Green" />
        </div>








        <!--Campo de busca-->


        <div class="p-3">
            <asp:Label Text="Buscar por nome" runat="server" />
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />

            <ajaxToolkit:AutoCompleteExtender ID="autoCompleteSearch" runat="server" TargetControlID="txtSearch"
                ServicePath="~/Services/AutoCompleteService.asmx" ServiceMethod="GetStudentsName" MinimumPrefixLength="1" CompletionSetCount="5" EnableCaching="true" CompletionInterval="100" />

            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-2" OnClick="SearchStudents" />

        </div>
    </form>





    <div>
        <asp:Panel ID="tableStudents" runat="server" Visible="false" CssClass="p-3">
            <table class="table 100">
                <thead>
                    <tr>
                        <th scope="col">Id</th>
                        <th scope="col" class="">Name</th>
                        <th scope="col" class="">Age</th>
                        <th scope="col" class="">Birth Date</th>
                        <th scope="col" class="">Course</th>
                        <!--Editar deletar-->
                        <th>Ação</th>

                    </tr>
                </thead>
                <tbody class="table-group-divider">
                    <asp:Repeater ID="repeaterStudents" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Id") %></td>
                                <td><%# Eval("Name") %></td>
                                <td><%# Eval("Age") %></td>
                                <td><%# Eval("BirthDate", "{0:dd/MM/yyyy}") %></td>
                                <td><%# Eval("Course") %></td>
                                <!--Editar deletar-->

                                <td><a href="javascript:void(0);" onclick="loadFormData('<%# Eval("Id") %>', '<%# Eval("Name") %>', '<%# Eval("Age") %>', '<%# Eval("BirthDate", "{0:yyyy-MM-dd}") %>', '<%# Eval("Course") %>')">Selecionar</a></td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function selectRow(row) {
            // Remove a classe de seleção de qualquer outra linha
            var table = row.parentNode;
            for (var i = 0; i < table.rows.length; i++) {
                table.rows[i].classList.remove('selected-row');
            }
            // Adiciona a classe de seleção à linha clicada
            row.classList.add('selected-row');
        }

        function loadFormData(id, name, age, birthDate, course) {
            document.getElementById('<%= hdnStudentId.ClientID %>').value = id;
            document.getElementById('<%= txtName.ClientID %>').value = name;
            document.getElementById('<%= txtAge.ClientID %>').value = age;
            document.getElementById('<%= txtBirthDate.ClientID %>').value = birthDate;

            var ddlCourses = document.getElementById('<%= txtCourses.ClientID %>');
            for (var i = 0; i < ddlCourses.options.length; i++) {
                if (ddlCourses.options[i].value === course) {
                    ddlCourses.selectedIndex = i;
                    break;
                }
            }
        }
    </script>
</body>
</html>
