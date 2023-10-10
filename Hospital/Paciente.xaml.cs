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
    /// Lógica de interacción para Paciente.xaml
    /// </summary>
    public partial class Paciente : Window
    {
        SqlConnection conexionSql;

        public Paciente()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);
        }

        private void mostrarPacientes()
        {

            try
            {
                string consulta = "select Id, concat(Id, ' - ', Nombre, ' ', Apellido1, ' ', ISNULL(Apellido2, ' --- '), ' ', Fecha_Nacimiento, ' ', Telefono, ' ', Email, ' ', Direccion) as infoPaciente from Paciente";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_pacientes = new DataTable();

                    sqlDataAdapter.Fill(dt_pacientes);

                    lct_paciente.DisplayMemberPath = "infoPaciente";
                    lct_paciente.SelectedValuePath = "Id";
                    lct_paciente.ItemsSource = dt_pacientes.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_insertar_paciente_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string consulta = "insert into Paciente values (@Id, @Nombre, @Apellido1, @Apellido2, @FechaNacimiento, @Telefono, @Email, @Direccion)";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@Id", txt_id.Text);
                    sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);
                    sqlCommand.Parameters.AddWithValue("@FechaNacimiento", dp_fechaNacimiento.Text);
                    sqlCommand.Parameters.AddWithValue("@Telefono", txt_telefono.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txt_email.Text);
                    sqlCommand.Parameters.AddWithValue("@Direccion", txt_direccion.Text);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarPacientes();
                }

                MessageBox.Show("Has enviado la información del paciente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_salir_paciente_Click(object sender, RoutedEventArgs e)
        {            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_mostrar_paciente_Click(object sender, RoutedEventArgs e)
        {
            mostrarPacientes();
        }

        private void btn_borrar_paciente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show($"Estas borrando el paciente de id = {lct_paciente.SelectedValue.ToString()}");

                string consulta = "delete from Paciente where Id = @idPaciente;";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idPaciente", lct_paciente.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarPacientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_actualizar_paciente_Click(object sender, RoutedEventArgs e)
        {
            ActualizarPaciente actualizarPaciente = new ActualizarPaciente(Convert.ToInt32(lct_paciente.SelectedValue));

            try
            {
                string consulta = "select * from Paciente where Id = @idPaciente";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    //conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idPaciente", lct_paciente.SelectedValue);

                    DataTable dt_actualizaConsultas = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaConsultas);

                    actualizarPaciente.txt_id.Text = dt_actualizaConsultas.Rows[0]["Id"].ToString();
                    actualizarPaciente.txt_nombre.Text = dt_actualizaConsultas.Rows[0]["Nombre"].ToString();
                    actualizarPaciente.txt_apellido1.Text = dt_actualizaConsultas.Rows[0]["Apellido1"].ToString();
                    actualizarPaciente.txt_apellido2.Text = dt_actualizaConsultas.Rows[0]["Apellido2"].ToString();
                    actualizarPaciente.dp_fechaNacimiento.Text = dt_actualizaConsultas.Rows[0]["Fecha_Nacimiento"].ToString();
                    actualizarPaciente.txt_telefono.Text = dt_actualizaConsultas.Rows[0]["Telefono"].ToString();
                    actualizarPaciente.txt_email.Text = dt_actualizaConsultas.Rows[0]["Email"].ToString();
                    actualizarPaciente.txt_direccion.Text = dt_actualizaConsultas.Rows[0]["Direccion"].ToString();
                    //conexionSql.Close();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarPaciente.ShowDialog();

            mostrarPacientes();
        }
    }
}
