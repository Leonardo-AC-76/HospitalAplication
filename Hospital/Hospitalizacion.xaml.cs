using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace Hospital
{
    /// <summary>
    /// Lógica de interacción para Hospitalizacion.xaml
    /// </summary>
    public partial class Hospitalizacion : Window
    {
        SqlConnection conexionSql;

        public Hospitalizacion()
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarPacientes();
            cargarDoctores();
        }

        private void btn_salir_hospitalizacion_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_insertar_Hospitalizacion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "insert into Hospitalizacion values (@Id_Paciente, @Id_DoctorResponsable, @Habitacion, @Cama, @Fecha_Ingreso, @Fecha_Alta)";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@Id_Paciente", txt_idPaciente.Text);
                    sqlCommand.Parameters.AddWithValue("@Id_DoctorResponsable", txt_idDoctor.Text);
                    sqlCommand.Parameters.AddWithValue("@Habitacion",txt_habitacion.Text);
                    sqlCommand.Parameters.AddWithValue("@Cama", txt_cama.Text);
                    sqlCommand.Parameters.AddWithValue("@Fecha_Ingreso", dp_fechaIngreso.Text);
                    sqlCommand.Parameters.AddWithValue("@Fecha_Alta", dp_fechaAlta.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarHospitalizaciones();
                }

                MessageBox.Show("Has enviado la información de la Hospitalización");
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
                   
                    consulta = "select concat(h.Id, ' - ', p.Nombre, ' ', p.Apellido1, ' ', Habitacion, ' ', Cama, ' ', Fecha_Ingreso, ' ', Fecha_Alta) as infoConsulta from Hospitalizacion as h" +
                            " inner join Paciente as p on h.Id_Paciente = p.Id" +
                            " where p.Id = " + id + ";";
                }
                else
                {
                    id = ((DataRowView)cb_doctores.SelectedItem).Row.ItemArray[0].ToString();
                    id = id.Split(' ')[0];
                
                    consulta = "select concat(h.Id, ' - ', d.Nombre, ' ', e.Nombre, ' ', Habitacion, ' ', Cama, ' ', Fecha_Ingreso, ' ', Fecha_Alta) as infoConsulta from Hospitalizacion as h" +
                                " inner join Doctor as d on h.Id_DoctorResponsable = d.Id " +
                                " inner join Especialidad as e on d.Id_especialidad = e.Id" +
                                " where d.Id = " + id + ";";
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
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

        private void btn_paciente_Click(object sender, RoutedEventArgs e)
        {
            muestraConsulta("Paciente");
        }

        private void btn_doctor_Click(object sender, RoutedEventArgs e)
        {
            muestraConsulta("Doctor");
        }

        private void mostrarHospitalizaciones()
        {
            try
            {
                string consulta = "select h.*, concat(h.Id, ' - ', p.Nombre, ' ', d.Nombre, ' ', e.Nombre, ' ', h.Habitacion, ' '," +
                                    " h.Cama, ' ', h.Fecha_Ingreso, ' ', h.Fecha_Alta) as infoConsulta from Paciente as p" +
                                    " inner join Hospitalizacion as h on p.Id = h.Id_Paciente" +
                                    " inner join Doctor as d on h.Id_DoctorResponsable = d.Id" +
                                    " inner join Especialidad as e on d.Id_especialidad = e.Id";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_consultas = new DataTable();

                    sqlDataAdapter.Fill(dt_consultas);

                    lct_hospitalizacion.DisplayMemberPath = "infoConsulta";
                    lct_hospitalizacion.SelectedValuePath = "Id";
                    lct_hospitalizacion.ItemsSource = dt_consultas.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_mostrar_hospitalizacion_Click(object sender, RoutedEventArgs e)
        {
            mostrarHospitalizaciones();
        }

        private void btn_borrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show($"Estas borrando la hospitalización de id = {lct_hospitalizacion.SelectedValue.ToString()}");

                string consulta = "delete from Hospitalizacion where Id = @idHospitalizacion;";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idHospitalizacion", lct_hospitalizacion.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarHospitalizaciones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_actualizar_consulta_Click(object sender, RoutedEventArgs e)
        {
            ActualizarHospitalizacion actualizarHospitalizacion = new ActualizarHospitalizacion(Convert.ToInt32(lct_hospitalizacion.SelectedValue));

            try
            {
                string consulta = "select * from Hospitalizacion where Id = @idHospitalizacion";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@idHospitalizacion", lct_hospitalizacion.SelectedValue);

                    DataTable dt_actualizaConsultas = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaConsultas);

                    int paciente = Convert.ToInt32(dt_actualizaConsultas.Rows[0]["Id_Paciente"].ToString());
                    int doctor = Convert.ToInt32(dt_actualizaConsultas.Rows[0]["Id_DoctorResponsable"].ToString());

                    actualizarHospitalizacion.cb_pacientes.SelectedIndex = paciente - 1;
                    actualizarHospitalizacion.cb_doctores.SelectedIndex = doctor - 1;
                    actualizarHospitalizacion.txt_habitacion.Text = dt_actualizaConsultas.Rows[0]["Habitacion"].ToString();
                    actualizarHospitalizacion.txt_cama.Text = dt_actualizaConsultas.Rows[0]["Cama"].ToString();
                    actualizarHospitalizacion.dp_fechaIngreso.Text = dt_actualizaConsultas.Rows[0]["Fecha_Ingreso"].ToString();
                    actualizarHospitalizacion.dp_fechaAlta.Text = dt_actualizaConsultas.Rows[0]["Fecha_Alta"].ToString();                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarHospitalizacion.ShowDialog();

            mostrarHospitalizaciones();
        }
    }
}
