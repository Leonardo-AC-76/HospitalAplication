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
    /// Lógica de interacción para ActualizarEnfermeros.xaml
    /// </summary>
    public partial class ActualizarEnfermeros : Window
    {
        SqlConnection conexionSql;

        private int IdEnfermero;

        public ActualizarEnfermeros(int idEnfermero)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarDoctores();

            cargarIslas();

            this.IdEnfermero = idEnfermero;
        }

        private void btn_Guardar_enfermero_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "update Enfermero set Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, " +
                    "Dni = @Dni, Telefono = @Telefono, Id_Isla_residencia = @IslaResidencia, Fecha_Alta = @fechaAlta, Id_Supervisor = @IdSupervisor" +
                    " where Id = " + IdEnfermero;

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdEnfermero", txt_id.Text);
                    sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);
                    sqlCommand.Parameters.AddWithValue("@Dni", txt_dni.Text);
                    sqlCommand.Parameters.AddWithValue("@Telefono", txt_telefono.Text);
                    sqlCommand.Parameters.AddWithValue("@IslaResidencia", cb_islas.SelectedIndex + 10);
                    sqlCommand.Parameters.AddWithValue("@fechaAlta", dp_fechaAlta.Text);
                    sqlCommand.Parameters.AddWithValue("@IdSupervisor", cb_doctor.SelectedIndex + 1);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();
                }

                MessageBox.Show("Has enviado la actualización del enfermero");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
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

                    cb_doctor.DisplayMemberPath = "Nombre";
                    cb_doctor.SelectedValuePath = "Id";
                    cb_doctor.ItemsSource = dt_especialidades.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cargarIslas()
        {
            try
            {
                string consulta = "select Nombre from Islas";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_islas = new DataTable();

                    sqlDataAdapter.Fill(dt_islas);

                    cb_islas.DisplayMemberPath = "Nombre";
                    cb_islas.SelectedValuePath = "Id";
                    cb_islas.ItemsSource = dt_islas.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
