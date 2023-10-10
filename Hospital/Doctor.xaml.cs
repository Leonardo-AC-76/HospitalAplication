using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Collections.ObjectModel;

namespace Hospital
{
    /// <summary>
    /// Lógica de interacción para Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        SqlConnection conexionSql;

        public Doctor()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarEspecialidades();
        }

        private void mostrarDoctores()
        {
            try
            {
                string consulta = "select d.*, concat(d.Id, ' - ', d.Nombre, ' ',  Apellido1, ' ', Apellido2, ' - ', e.Nombre) as infoDoctor from Doctor as d" +
                " inner join Especialidad as e on d.Id_especialidad = e.Id";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_doctores = new DataTable();

                    sqlDataAdapter.Fill(dt_doctores);

                    lct_doctor.DisplayMemberPath = "infoDoctor";
                    lct_doctor.SelectedValuePath = "Id";
                    lct_doctor.ItemsSource = dt_doctores.DefaultView;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
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

        private int buscarIdEspecialidad(string nombre)
        {
            string consulta1 = "select id from Especialidad where Nombre = '" + nombre + "'";

            SqlCommand sqlCommand = new SqlCommand(consulta1, conexionSql);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            int id = -1;

            using (sqlDataAdapter)
            {
                conexionSql.Open();

                var resultado = sqlCommand.ExecuteScalar();

                if (resultado != null)
                {
                    id = Convert.ToInt32(resultado);
                }

                conexionSql.Close();
            }

            return id;
        }

        private void btn_insertar_doctor_Click(object sender, RoutedEventArgs e)
        {

            int id = buscarIdEspecialidad(cb_especialidades.Text);

            string consulta = "insert into Doctor values (@Id, @Nombre, @Apellido1, @Apellido2, @Especialidad)";

            SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlDataAdapter)
            {
                conexionSql.Open();

                sqlCommand.Parameters.AddWithValue("@Id", txt_id.Text);
                sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);    
                sqlCommand.Parameters.AddWithValue("@Especialidad", id);

                sqlCommand.ExecuteNonQuery();

                conexionSql.Close();

                mostrarDoctores();
            }


            MessageBox.Show("Has enviado la información del doctor");
        }

        private void btn_salir_doctor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_mostrar_doctor_Click(object sender, RoutedEventArgs e)
        {
            mostrarDoctores();
        }

        private void btn_borrar_doctor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                MessageBox.Show($"Estas borrando el doctor de id = {lct_doctor.SelectedValue.ToString()}");

                string consulta = "delete from Doctor where Id = @idDoctor;";
                
                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idDoctor", lct_doctor.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarDoctores();
                }
            }
            catch (Exception ex)
            {
                string mensajeError = "No podemos borrar el doctor sin borrar la consulta";

                MessageBox.Show(mensajeError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void btn_actualizar_Doctor_Click(object sender, RoutedEventArgs e)
        {
           
            ActualizarDoctor actualizarDoctor = new ActualizarDoctor(Convert.ToInt32(lct_doctor.SelectedValue));

            try
            {
                string consulta = "select * from Doctor where Id = @idDoctor";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@idDoctor", lct_doctor.SelectedValue);

                    DataTable dt_actualizaConsultas = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaConsultas);

                    int especialidad = Convert.ToInt32(dt_actualizaConsultas.Rows[0]["Id_Especialidad"]);

                    actualizarDoctor.txt_id.Text = dt_actualizaConsultas.Rows[0]["Id"].ToString();
                    actualizarDoctor.txt_nombre.Text = dt_actualizaConsultas.Rows[0]["Nombre"].ToString();
                    actualizarDoctor.txt_apellido1.Text = dt_actualizaConsultas.Rows[0]["Apellido1"].ToString();
                    actualizarDoctor.txt_apellido2.Text = dt_actualizaConsultas.Rows[0]["Apellido2"].ToString();
                    actualizarDoctor.cb_especialidades.SelectedIndex = especialidad - 1;                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarDoctor.ShowDialog();

            mostrarDoctores();
        }
    }
}
