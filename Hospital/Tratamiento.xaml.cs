using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
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
using System.Configuration;
using System.Reflection;

namespace Hospital
{
    /// <summary>
    /// Lógica de interacción para Tratamiento.xaml
    /// </summary>
    public partial class Tratamiento : Window
    {
        SqlConnection conexionSql;

        public Tratamiento()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarPacientes();
            cargarDoctores();
        }

        private void btn_insertar_tratamiento_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string consulta = "insert into Tratamiento values (@IdPaciente, @Medicamento, @Dosis, @Duracion, @IdDoctor)";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdPaciente", txt_idPaciente.Text);
                    sqlCommand.Parameters.AddWithValue("@Medicamento", txt_medicamento.Text);
                    sqlCommand.Parameters.AddWithValue("@Dosis", txt_dosis.Text);
                    sqlCommand.Parameters.AddWithValue("@Duracion", txt_duracion.Text);
                    sqlCommand.Parameters.AddWithValue("@IdDoctor", txt_idDoctor.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarTratamientos();
                }

                MessageBox.Show("Has enviado la información del tratamiento");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_salir_tratamiento_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void cargarPacientes()
        {
            try
            {
                string consulta = "select  concat(id, ' ', Nombre) as info from Paciente";

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
                string consulta = "select  concat(id, ' ', Nombre) as info from Doctor";

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

                    consulta = "select concat(t.Id, ' - ', p.Nombre, ' ', Medicamento, ' ', Dosis, ' ', Duracion) as infoConsulta from Tratamiento as t" +
                            " inner join Paciente as p on t.Id_Paciente = p.Id" +
                            " where p.Id = " + id + ";";
                }
                else
                {
                    id = ((DataRowView)cb_doctores.SelectedItem).Row.ItemArray[0].ToString();
                    id = id.Split(' ')[0];

                    consulta = "select concat(t.Id, ' - ', d.Nombre, ' ', Medicamento, ' ', Dosis, ' ', Duracion) as infoConsulta from Tratamiento as t" +
                                " inner join Doctor as d on t.Id_Doctor = d.Id " +
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

        private void mostrarTratamientos()
        {

            try
            {
                string consulta = "select t.*, concat (t.id, ' - ', ISNULL(d.Nombre, ' --- '), ' ', Medicamento, ' ', Dosis, ' ',Duracion, ' ',p.Nombre) as infoConsulta from Paciente as p" +
                                " inner join Tratamiento as t on p.Id = t.Id_Paciente" +
                                " left join Doctor as d on t.Id_Doctor = d.Id";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_doctores = new DataTable();

                    sqlDataAdapter.Fill(dt_doctores);

                    lct_tratamientos.DisplayMemberPath = "infoConsulta";
                    lct_tratamientos.SelectedValuePath = "Id";
                    lct_tratamientos.ItemsSource = dt_doctores.DefaultView;
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
                MessageBox.Show($"Estas borrando el tratamiento de id = {lct_tratamientos.SelectedValue.ToString()}");

                string consulta = "delete from tratamiento where Id = @idTratamiento;";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idTratamiento", lct_tratamientos.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarTratamientos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_mostrar_tratamientos_Click(object sender, RoutedEventArgs e)
        {
            mostrarTratamientos();
        }

        private void btn_actualizar_tratamiento_Click(object sender, RoutedEventArgs e)
        {
            ActualizarTratamiento actualizarTratamiento = new ActualizarTratamiento(Convert.ToInt32(lct_tratamientos.SelectedValue));

            try
            {
                string consulta = "select * from Tratamiento where Id = @idTratamiento";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    //conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idTratamiento", lct_tratamientos.SelectedValue);

                    DataTable dt_actualizaConsultas = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaConsultas);

                    actualizarTratamiento.txt_idPaciente.Text = dt_actualizaConsultas.Rows[0]["Id_Paciente"].ToString();
                    actualizarTratamiento.txt_medicamento.Text = dt_actualizaConsultas.Rows[0]["Medicamento"].ToString();
                    actualizarTratamiento.txt_dosis.Text = dt_actualizaConsultas.Rows[0]["Dosis"].ToString();
                    actualizarTratamiento.txt_duracion.Text = dt_actualizaConsultas.Rows[0]["Duracion"].ToString();
                    actualizarTratamiento.txt_idDoctor.Text = dt_actualizaConsultas.Rows[0]["Id_Doctor"].ToString();
                   
                    //conexionSql.Close();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarTratamiento.ShowDialog();

            mostrarTratamientos();
        }
    }
}
