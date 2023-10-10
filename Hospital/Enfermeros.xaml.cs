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
    /// Lógica de interacción para Enfermeros.xaml
    /// </summary>
    public partial class Enfermeros : Window
    {
        SqlConnection conexionSql;

        public Enfermeros()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Hospital.Properties.Settings.HospitalDBConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);

            cargarDoctores();
            cargarIslas();
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

        private void btn_insertar_doctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idDoctor = buscarIdDoctor(cb_doctor.Text);

                int idIslas =  buscarIdIslas(cb_islas.Text);

                string consulta = "insert into Enfermero values (@Nombre, @Apellido1, @Apellido2, @Dni, @Telefono, @IdIsla, @FechaAlta, @IdDoctor)";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@Nombre", txt_nombre.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido1", txt_apellido1.Text);
                    sqlCommand.Parameters.AddWithValue("@Apellido2", txt_apellido2.Text);
                    sqlCommand.Parameters.AddWithValue("@Dni", txt_dni.Text);
                    sqlCommand.Parameters.AddWithValue("@Telefono", txt_telefono.Text);
                    sqlCommand.Parameters.AddWithValue("@IdIsla", idIslas);
                    sqlCommand.Parameters.AddWithValue("@FechaAlta", dp_fechaAlta.Text);
                    sqlCommand.Parameters.AddWithValue("@IdDoctor", idDoctor);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarEnfermeros();
                }

                MessageBox.Show("Has enviado la información del enfermero");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int buscarIdDoctor(string nombre)
        {
            string consulta1 = "select id from Doctor where Nombre = '" + nombre + "'";

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

        private int buscarIdIslas(string nombre)
        {
            string consulta1 = "select id from Islas where Nombre = '" + nombre + "'";

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

        private void btn_mostrar_doctor_Click(object sender, RoutedEventArgs e)
        {
            mostrarEnfermeros();
        }

        private void mostrarEnfermeros()
        {
            try
            {
                string consulta = "select e.*, concat(e.Id, ' - ', e.Nombre, ' ',  e.Dni, ' ',  i.Nombre, ' ', e.Fecha_Alta, ' ', e.Telefono," +
                                   " ' ', d.Nombre) as infoConsulta from Doctor as d" +
                                   " inner join Enfermero as e on d.Id = e.Id_Supervisor" +
                                   " inner join Islas as i on e.Id_Isla_residencia = i.Id";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conexionSql);

                using (sqlDataAdapter)
                {
                    DataTable dt_enfermeros = new DataTable();

                    sqlDataAdapter.Fill(dt_enfermeros);

                    lct_enfermeros.DisplayMemberPath = "infoConsulta";
                    lct_enfermeros.SelectedValuePath = "Id";
                    lct_enfermeros.ItemsSource = dt_enfermeros.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn_salir_doctor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_borrar_enfermeros_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show($"Estas borrando al enfermero de id = {lct_enfermeros.SelectedValue.ToString()}");

                string consulta = "delete from Enfermero where Id = @idEnfermero;";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    conexionSql.Open();

                    sqlCommand.Parameters.AddWithValue("@idEnfermero", lct_enfermeros.SelectedValue);

                    sqlCommand.ExecuteNonQuery();

                    conexionSql.Close();

                    mostrarEnfermeros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_actualizar_enfermeros_Click(object sender, RoutedEventArgs e)
        {
            ActualizarEnfermeros actualizarEnfermeros = new ActualizarEnfermeros(Convert.ToInt32(lct_enfermeros.SelectedValue));

            try
            {
                string consulta = "select * from Enfermero where Id = @idEnfermero";

                SqlCommand sqlCommand = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@idEnfermero", lct_enfermeros.SelectedValue);

                    DataTable dt_actualizaEnfermero = new DataTable();

                    sqlDataAdapter.Fill(dt_actualizaEnfermero);

                    int islas = Convert.ToInt32(dt_actualizaEnfermero.Rows[0]["Id_Isla_residencia"]);
                    int doctor = Convert.ToInt32(dt_actualizaEnfermero.Rows[0]["Id_Supervisor"]);

                    actualizarEnfermeros.txt_id.Text = dt_actualizaEnfermero.Rows[0]["Id"].ToString();
                    actualizarEnfermeros.txt_nombre.Text = dt_actualizaEnfermero.Rows[0]["Nombre"].ToString();
                    actualizarEnfermeros.txt_apellido1.Text = dt_actualizaEnfermero.Rows[0]["Apellido1"].ToString();
                    actualizarEnfermeros.txt_apellido2.Text = dt_actualizaEnfermero.Rows[0]["Apellido2"].ToString();
                    actualizarEnfermeros.txt_dni.Text = dt_actualizaEnfermero.Rows[0]["Dni"].ToString();
                    actualizarEnfermeros.txt_telefono.Text = dt_actualizaEnfermero.Rows[0]["Telefono"].ToString();
                    actualizarEnfermeros.cb_islas.SelectedIndex = islas - 10;
                    actualizarEnfermeros.dp_fechaAlta.Text = dt_actualizaEnfermero.Rows[0]["Fecha_Alta"].ToString();
                    actualizarEnfermeros.cb_doctor.SelectedIndex = doctor - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            actualizarEnfermeros.ShowDialog();

            mostrarEnfermeros();
        }
    }
}
