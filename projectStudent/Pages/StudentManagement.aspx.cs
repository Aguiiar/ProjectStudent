using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using projectStudent.Data;
using projectStudent.Models;

namespace projectStudent.Pages
{
    public partial class StudentManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


           
            if (!IsPostBack)
            {
                LoadCoursesFromApi();
                UpdateTable(); 
            }
        }


  
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

                if (filteredlist.Any()) 
                {
                    repeaterStudents.DataSource = filteredlist;
                    repeaterStudents.DataBind();
                    tableStudents.Visible = true;
                    lblMensagem.Text = ""; 
                }
                else
                {
                    
                    lblMensagem.Text = "Nenhum aluno encontrado com o nome: " + txtSearch.Text;
                    lblMensagem.ForeColor = System.Drawing.Color.Yellow;
                    tableStudents.Visible = false; 
                    repeaterStudents.DataSource = null; 
                    repeaterStudents.DataBind();
                }
            }
        }

      
        private void LoadCoursesFromApi()
        {
            using (var client = new WebClient())
            {
              
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
                var al = new Models.Student
                {
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    BirthDate = DateTime.Parse(txtBirthDate.Text),
                    Course = txtCourses.SelectedValue
                };
                context.Students.Add(al);
                context.SaveChanges(); 
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
                        UpdateTable(); 
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

        
        private void UpdateTable()
        {
            using (var context = new StudentContext())
            {
               
                List<Models.Student> studentsFromDb = context.Students.ToList();

                
                var list = studentsFromDb.Select(s => new
                {
                    id = s.Id, 
                    name = s.Name, 
                    age = s.Age, 
                    birthDate = s.BirthDate, 
                    course = s.Course 
                }).ToList();

                repeaterStudents.DataSource = list;
                repeaterStudents.DataBind();

            
                tableStudents.Visible = list.Any();
            }
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtBirthDate.Text = "";
            txtCourses.SelectedIndex = 0; 
            txtSearch.Text = "";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtBirthDate.Text) ||
                txtCourses.SelectedIndex == 0) 
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