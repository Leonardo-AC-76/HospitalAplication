using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Data;

namespace Hospital
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_doctor_Click(object sender, RoutedEventArgs e)
        {
            Doctor doctor = new Doctor();
            doctor.Show();
            this.Close();
        }

        private void btn_paciente_Click(object sender, RoutedEventArgs e)
        {
            Paciente paciente = new Paciente();
            paciente.Show();
            this.Close();
        }

        private void btn_salir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas cerrar la aplicación?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                // Cerrar la aplicación
                Application.Current.Shutdown();
            }
        }

        private void btn_consulta_Click(object sender, RoutedEventArgs e)
        {
            Consulta consulta = new Consulta();
            consulta.Show();
            this.Close();
        }

        private void btn_tratamiento_Click(object sender, RoutedEventArgs e)
        {
            Tratamiento tratamiento = new Tratamiento();
            tratamiento.Show();
            this.Close();
        }

        private void btn_Hospitalizacion_Click(object sender, RoutedEventArgs e)
        {
            Hospitalizacion hospitalizacion = new Hospitalizacion();
            hospitalizacion.Show();
            this.Close();
        }

        private void btn_enfermeros_Click(object sender, RoutedEventArgs e)
        {
            Enfermeros enfermeros = new Enfermeros();
            enfermeros.Show();
            this.Close();
        }
    }
}
