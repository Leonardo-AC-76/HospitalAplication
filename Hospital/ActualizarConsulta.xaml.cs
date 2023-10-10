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
    /// Lógica de interacción para ActualizarConsulta.xaml
    /// </summary>
    public partial class ActualizarConsulta : Window
    {
        SqlConnection conexionSql;

        private int IdConsulta;

        public ActualizarConsulta(int id)
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            this.IdConsulta = id;
        }

        private void btn_Guardar_consulta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "update Consulta set Id_Paciente = @IdPaciente, Fecha_Consulta = @FechaConsulta, Diagnostico = @Diagnostico, Id_Doctor = @IdDoctor where Id = " + IdConsulta ;

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
