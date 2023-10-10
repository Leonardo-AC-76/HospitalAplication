using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital
{
    /// <summary>
    /// Lógica de interacción para Consulta.xaml
    /// </summary>
    public partial class Consulta : Window
    {
        SqlConnection conexionSql;
        public Consulta()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarPacientes();
            cargarDoctores();
        }

        private void btn_insertar_consulta_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string consulta = "insert into Consulta values (@IdPaciente, @FechaConsulta, @Diagnostico, @IdDoctor)";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdPaciente", txt_idPaciente.Text);
                    sqlCommand.Parameters.AddWithValue("@FechaConsulta", dp_fechaConsulta.Text);
                    sqlCommand.Parameters.AddWithValue("@Diagnostico", txt_diagnostico.Text);
                    sqlCommand.Parameters.AddWithValue("@IdDoctor", txt_idDoctor.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarConsultas();
                }

                MessageBox.Show("Has enviado la información de la consulta");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_salir_consulta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void mostrarConsultas()
        {
            try
            {
                string consulta = "select c.*, concat(c.Id, ' - ', d.Nombre, ' ', Fecha_consulta, ' ', Diagnostico, ' ', p.Nombre) as infoConsulta from Paciente as p" +
                                   " inner join Consulta as c on p.Id = c.Id_Paciente" +
                                   " inner join Doctor as d on c.Id_Doctor = d.Id";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_consultas = new DataTable();

                    sqlDataAdapter.Fill(dt_consultas);

                    lct_consulta.DisplayMemberPath = "infoConsulta";
                    lct_consulta.SelectedValuePath = "Id";
                    lct_consulta.ItemsSource = dt_consultas.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void cargarPacientes()
        {
            try
            {
                string consulta = "select concat(id, ' ', Nombre) as info from Paciente";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_especialidades = new DataTable();


                    sqlDataAdapter.Fill(dt_especialidades);

                    cb_pacientes.DisplayMemberPath = "info";
                    cb_pacientes.SelectedValuePath = "Id";
                    cb_pacientes.ItemsSource = dt_especialidades.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cargarDoctores()
        {
            try
            {
                string consulta = "select concat(id, ' ', Nombre) as info from Doctor";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_especialidades = new DataTable();


                    sqlDataAdapter.Fill(dt_especialidades);

                    cb_doctores.DisplayMemberPath = "info";
                    cb_doctores.SelectedValuePath = "Id";
                    cb_doctores.ItemsSource = dt_especialidades.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_mostrar_consulta_Click(object sender, RoutedEventArgs e)
        {
            mostrarConsultas();
        }

        private void btn_doctor_Click(object sender, RoutedEventArgs e)
        {
            muestraConsulta("Doctor");
        }

        private void btn_paciente_Click(object sender, RoutedEventArgs e)
        {
            muestraConsulta("Paciente"); 
        }

        private void muestraConsulta(string dato)
        {
            string id = "";
            string consulta;

            try
            {
                if (dato == "Paciente")
                {
                    id = ((DataRowView)cb_pacientes.SelectedItem).Row.ItemArray[0].ToString();
                    id = id.Split(' ')[0];

                    consulta = "select concat(c.Id, ' - ', p.Nombre, ' ', Fecha_consulta, ' ', Diagnostico) as infoConsulta from Paciente as p" +
                            " inner join Consulta as c on p.Id = c.Id_Paciente" +
                            " where p.Id = " + id + ";";
                }
                else
                {
                    id = ((DataRowView)cb_doctores.SelectedItem).Row.ItemArray[0].ToString();
                    id = id.Split(' ')[0];

                    consulta = "select concat(c.Id, ' - ', d.Nombre, ' ', e.Nombre, ' ', Fecha_consulta, ' ', Diagnostico) as infoConsulta from Consulta as c" +
                                " inner join Doctor as d on c.Id_Doctor = d.Id " +
                                " inner join Especialidad as e on d.Id_especialidad = e.Id" +
                                " where d.Id = " + id + ";";
                }

                //SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    //sqlCommand.Parameters.AddWithValue("@pacienteId", lct_listado.SelectedValue);

                    DataTable dt_especialidades = new DataTable();

                    sqlDataAdapter.Fill(dt_especialidades);

                    lct_listado.DisplayMemberPath = "infoConsulta";
                    lct_listado.SelectedValuePath = "Id";
                    lct_listado.ItemsSource = dt_especialidades.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_borrar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                MessageBox.Show($"Estas borrando la consulta de id = {lct_consulta.SelectedValue.ToString()}");

                string consulta = "delete from Consulta where Id = @idConsulta;";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idConsulta", lct_consulta.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarConsultas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void btn_actualizar_consulta_Click(object sender, RoutedEventArgs e)
        {
            ActualizarConsulta actualizarConsulta = new ActualizarConsulta(Convert.ToInt32(lct_consulta.SelectedValue)) ;

            try
            {
                string consulta = "select * from Consulta where Id = @idConsulta";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    //conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdConsulta", lct_consulta.SelectedValue);

                    DataTable dt_actualizaConsultas = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaConsultas);

                    actualizarConsulta.txt_idPaciente.Text = dt_actualizaConsultas.Rows[0]["Id_Paciente"].ToString();
                    actualizarConsulta.dp_fechaConsulta.Text = dt_actualizaConsultas.Rows[0]["Fecha_Consulta"].ToString();
                    actualizarConsulta.txt_diagnostico.Text = dt_actualizaConsultas.Rows[0]["Diagnostico"].ToString();
                    actualizarConsulta.txt_idDoctor.Text = dt_actualizaConsultas.Rows[0]["Id_Doctor"].ToString();
                   
                    //conexionSql.Close();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarConsulta.ShowDialog();

            mostrarConsultas();
        }
    }
}
