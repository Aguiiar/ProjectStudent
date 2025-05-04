using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using projectStudent.Models;

namespace projectStudent.Pages
{
    public partial class StudentManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //Carregando nossa api
            if (!IsPostBack)
            {
                LoadCoursesFromApi();
                UpdateTable(); // Carrega os dados da tabela ao carregar a página
            }
        }


        //Metodo Buscar
        protected void SearchStudents(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim().ToLower();

            using (var context = new StudentContext())
            {
                var filteredlist = context.Students
                    .Where(s => s.Name.ToLower().Contains(search))
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Age,
                        s.BirthDate,
                        s.Course
                    }).ToList();

                if (filteredlist.Any()) // Verifica se a lista tem algum item
                {
                    repeaterStudents.DataSource = filteredlist;
                    repeaterStudents.DataBind();
                    tableStudents.Visible = true;
                    lblMensagem.Text = ""; // Limpa qualquer mensagem anterior
                }
                else
                {
                    // Opção A: Não fazer nada (a tabela permanece como estava)
                    // Opção B: Exibir uma mensagem informando que nenhum aluno foi encontrado
                    lblMensagem.Text = "Nenhum aluno encontrado com o nome: " + txtSearch.Text;
                    lblMensagem.ForeColor = System.Drawing.Color.Yellow;
                    tableStudents.Visible = false; // Oculta a tabela se não houver resultados
                    repeaterStudents.DataSource = null; // Limpa o DataSource
                    repeaterStudents.DataBind();
                }
            }
        }

        //Puxando dados da api
        private void LoadCoursesFromApi()
        {
            using (var client = new WebClient())
            {
                //Arrumando erros de ficar com caracteres quebrados nos acentos
                client.Encoding = System.Text.Encoding.UTF8;

                string json = client.DownloadString("https://raw.githubusercontent.com/maykon-oliveira/lista_de_cursos_superiores_do_brasil_api/main/output.json");
                var courses = JsonConvert.DeserializeObject<List<ApiCourse>>(json);

                txtCourses.Items.Clear();
                txtCourses.Items.Add(new ListItem("Selecione seu curso", ""));

                foreach (var course in courses)
                {
                    txtCourses.Items.Add(new ListItem(course.Name, course.Name));
                }
            }
        }

        protected void Register(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            using (var context = new StudentContext())
            {
                var al = new Student
                {
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    BirthDate = DateTime.Parse(txtBirthDate.Text),
                    Course = txtCourses.SelectedValue
                };
                context.Students.Add(al);
                context.SaveChanges(); // Salva as alterações no banco de dados
                lblMensagem.Text = "Successfully registered!";
                lblMensagem.ForeColor = System.Drawing.Color.Green;
                ClearFields();
                UpdateTable();
            }
        }


        protected void EditStudent(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnStudentId.Value))
            {
                int studentIdToEdit = Convert.ToInt32(hdnStudentId.Value);
                if (!ValidateFields())
                {
                    return;
                }
                using (var context = new StudentContext())
                {
                    try
                    {
                        var studentToEdit = context.Students.FirstOrDefault(s => s.Id == studentIdToEdit);
                        if (studentToEdit != null)
                        {
                            studentToEdit.Name = txtName.Text;
                            studentToEdit.Age = int.Parse(txtAge.Text);
                            studentToEdit.BirthDate = DateTime.Parse(txtBirthDate.Text);
                            studentToEdit.Course = txtCourses.SelectedValue;

                            context.SaveChanges();
                            lblMensagem.Text = "Aluno atualizado com sucesso!";
                            lblMensagem.ForeColor = System.Drawing.Color.Green;
                            ClearFields();
                            UpdateTable();
                        }
                        else
                        {
                            lblMensagem.Text = "Aluno não encontrado para edição.";
                            lblMensagem.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao editar o aluno: " + ex.Message;
                        lblMensagem.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else
            {
                lblMensagem.Text = "Selecione um aluno para editar.";
                lblMensagem.ForeColor = System.Drawing.Color.Yellow;
            }
        }

        protected void DeleteStudent(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnStudentId.Value))
            {
                int studentIdToDelete = Convert.ToInt32(hdnStudentId.Value);
                using (var context = new StudentContext())
                {
                    var studentToDelete = context.Students.FirstOrDefault(s => s.Id == studentIdToDelete);
                    if (studentToDelete != null)
                    {
                        context.Students.Remove(studentToDelete);
                        context.SaveChanges();
                        lblMensagem.Text = "Aluno deletado com sucesso!";
                        lblMensagem.ForeColor = System.Drawing.Color.Green;
                        ClearFields();
                        UpdateTable(); // Atualiza a tabela após a exclusão
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não encontrado para exclusão.";
                        lblMensagem.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else
            {
                lblMensagem.Text = "Selecione um aluno para deletar.";
                lblMensagem.ForeColor = System.Drawing.Color.Red;
            }
        }

        //Quando acontecer algo natabela atualizar
        //apertar cadastrar e etc aconetcer algo na tabel
        private void UpdateTable()
        {
            using (var context = new StudentContext())
            {
                // Usando DbSet<Student> diretamente para buscar todos os dados
                List<Student> studentsFromDb = context.Students.ToList();

                // Agora, vamos criar uma lista do tipo anônimo que o seu Repeater espera
                var list = studentsFromDb.Select(s => new
                {
                    id = s.Id, // Certifique-se da capitalização correta da propriedade no seu modelo
                    name = s.Name, // Certifique-se da capitalização correta da propriedade no seu modelo
                    age = s.Age, // Certifique-se da capitalização correta da propriedade no seu modelo
                    birthDate = s.BirthDate, // Certifique-se da capitalização correta da propriedade no seu modelo
                    course = s.Course // Certifique-se da capitalização correta da propriedade no seu modelo
                }).ToList();

                repeaterStudents.DataSource = list;
                repeaterStudents.DataBind();

                //Mostrar tabela se tiver dados
                tableStudents.Visible = list.Any();
            }
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtBirthDate.Text = "";
            txtCourses.SelectedIndex = 0; // Seleciona o primeiro item (Selecione seu curso)
            txtSearch.Text = "";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtBirthDate.Text) ||
                txtCourses.SelectedIndex == 0) // Verifica se um curso foi selecionado
            {
                lblMensagem.Text = "Required Fields!";
                lblMensagem.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            if (!DateTime.TryParse(txtBirthDate.Text, out _))
            {
                lblMensagem.Text = "Invalid date";
                lblMensagem.ForeColor = System.Drawing.Color.Red;
                return false;
            }

            if (!int.TryParse(txtAge.Text, out _))
            {
                lblMensagem.Text = "Invalid Age!";
                lblMensagem.ForeColor = System.Drawing.Color.Red;
                return false;
            }

            return true;
        }
    }
}