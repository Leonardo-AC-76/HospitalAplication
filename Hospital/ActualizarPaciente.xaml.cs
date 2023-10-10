using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Lógica de interacción para ActualizarPaciente.xaml
    /// </summary>
    public partial class ActualizarPaciente : Window
    {

        SqlConnection conexionSql;

        private int IdConsulta;

        public ActualizarPaciente(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            this.IdConsulta = id;
        }

        private void btn_guardar_paciente_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string consulta = "update Paciente set Id = @IdPaciente, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Fecha_Nacimiento = @fechaNacimento, " +
                    "Telefono = @Telefono, Email = @Email, Direccion = @Direccion where Id = " + IdConsulta;

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@IdPaciente", txt_id.Text);
                    sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);
                    sqlCommand.Parameters.AddWithValue("@fechaNacimento", dp_fechaNacimiento.Text);
                    sqlCommand.Parameters.AddWithValue("@Telefono", txt_telefono.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txt_email.Text);
                    sqlCommand.Parameters.AddWithValue("@Direccion", txt_direccion.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();
                }

                MessageBox.Show("Has enviado la actualización del paciente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }
}
