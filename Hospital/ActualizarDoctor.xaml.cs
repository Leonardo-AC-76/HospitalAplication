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
    /// Lógica de interacción para ActualizarDoctor.xaml
    /// </summary>
    public partial class ActualizarDoctor : Window
    {
        SqlConnection conexionSql;

        private int IdDoctor;

        public ActualizarDoctor(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarEspecialidades();

            this.IdDoctor = id;
        }

        private void btn_Guardar_Doctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "update Doctor set Id = @IdDoctor, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, " +
                    "Id_especialidad = @IdEspecialidad where Id = " + IdDoctor;

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdDoctor", txt_id.Text);
                    sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);
                    sqlCommand.Parameters.AddWithValue("@IdEspecialidad", cb_especialidades.SelectedIndex + 1);
                   

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();
                }

                MessageBox.Show("Has enviado la actualización de la consulta");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }

        private void cargarEspecialidades()
        {

            try
            {
                string consulta = "select Nombre from Especialidad";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_especialidades = new DataTable();

                    sqlDataAdapter.Fill(dt_especialidades);

                    cb_especialidades.DisplayMemberPath = "Nombre";
                    cb_especialidades.SelectedValuePath = "Id";
                    cb_especialidades.ItemsSource = dt_especialidades.DefaultView;
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
