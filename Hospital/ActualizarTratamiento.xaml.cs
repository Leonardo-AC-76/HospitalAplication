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
    /// Lógica de interacción para ActualizarTratamiento.xaml
    /// </summary>
    public partial class ActualizarTratamiento : Window
    {
        SqlConnection conexionSql;

        private int IdConsulta;


        public ActualizarTratamiento(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            this.IdConsulta = id;
        }

        private void btn_Guardar_Tratamiento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "update Tratamiento set Id_Paciente = @IdPaciente, Medicamento = @Medicamento, Dosis = @Dosis, Duracion = @Duracion, " +
                    "Id_Doctor = @IdDoctor where Id = " + IdConsulta;

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
                }

                MessageBox.Show("Has enviado la actualización de la consulta");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }
}
