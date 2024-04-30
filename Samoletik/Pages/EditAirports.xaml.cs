using Npgsql;
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
using System.Windows.Shapes;

namespace Samoletik.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditAirports.xaml
    /// </summary>
    public partial class EditAirports : Window
    {
        private readonly string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";
        public EditAirports()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                string airportName = txtImya.Text;
                string city = txtfamilia.Text;
                string country = txtoth.Text;
                string contactInformation = txtdata.Text;
                if (string.IsNullOrWhiteSpace(airportName) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (txtImya.Text.Length > 30)
                {
                    MessageBox.Show("Поле 'Название аэропорта' должно содержать не более 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtfamilia.Text.Length > 40)
                {
                    MessageBox.Show("Поле 'Город' должно содержать не более 40 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtoth.Text.Length > 40)
                {
                    MessageBox.Show("Поле 'Страна' должно содержать не более 40 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string query = @"
            UPDATE ""Airports""
            SET ""AirportName"" = @AirportName, 
                ""City"" = @City, 
                ""Country"" = @Country, 
                ""IDOperatingStatus""=@IDOperatingStatus,
                ""ContactInformation"" = @ContactInformation
            WHERE ""AirportName"" = @AirportName;";

              
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AirportName", airportName);
                        command.Parameters.AddWithValue("@City", city);
                        command.Parameters.AddWithValue("@Country", country);
                        command.Parameters.AddWithValue("@ContactInformation", contactInformation);
                        command.Parameters.AddWithValue("@IDOperatingStatus", 1);

                        int rowsAffected = command.ExecuteNonQuery();
 
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные аэропорта успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные аэропорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении данных аэропорта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = "";
            txtfamilia.Text = "";
            txtoth.Text = "";
            txtdata.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            
                string airportName = txtImya.Text;
                if (string.IsNullOrWhiteSpace(airportName))
                {
                    MessageBox.Show("Пожалуйста, введите название аэропорта для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string query = @"DELETE FROM ""Airports"" WHERE ""AirportName"" = @AirportName;";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AirportName", airportName);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Аэропорт успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                          
                           DialogResult=true;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить аэропорт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении аэропорта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
