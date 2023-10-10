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
    /// Lógica de interacción para ActualizarHospitalizacion.xaml
    /// </summary>
    public partial class ActualizarHospitalizacion : Window
    {
        SqlConnection conexionSql;

        private int IdConsulta;

        public ActualizarHospitalizacion(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            this.IdConsulta = id;

            cargarPacientes();

            cargarDoctores();
        }

        private void cargarPacientes()
        {
            try
            {
                string consulta = "select Nombre from Paciente";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_especialidades = new DataTable();


                    sqlDataAdapter.Fill(dt_especialidades);

                    cb_pacientes.DisplayMemberPath = "Nombre";
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
                string consulta = "select Nombre from Doctor";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_especialidades = new DataTable();


                    sqlDataAdapter.Fill(dt_especialidades);

                    cb_doctores.DisplayMemberPath = "Nombre";
                    cb_doctores.SelectedValuePath = "Id";
                    cb_doctores.ItemsSource = dt_especialidades.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn_Guardar_hospitalizacion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "update Hospitalizacion set Id_Paciente = @IdPaciente, Id_DoctorResponsable = @IdDoctor, Habitacion = @habitacion, " +
                    "Cama = @Cama,  Fecha_Ingreso = @fechaIngreso, Fecha_Alta = @fechaAlta where Id = " + IdConsulta;

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdPaciente", cb_pacientes.SelectedIndex + 1);
                    sqlCommand.Parameters.AddWithValue("@IdDoctor", cb_doctores.SelectedIndex + 1);
                    sqlCommand.Parameters.AddWithValue("@habitacion", txt_habitacion.Text);
                    sqlCommand.Parameters.AddWithValue("@Cama", txt_cama.Text);
                    sqlCommand.Parameters.AddWithValue("@fechaIngreso", dp_fechaIngreso.Text);
                    sqlCommand.Parameters.AddWithValue("@fechaAlta", dp_fechaAlta.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();
                }

                MessageBox.Show("Has enviado la actualización de la hospitalización");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }
}
