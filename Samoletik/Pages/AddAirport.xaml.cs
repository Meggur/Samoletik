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
    /// Логика взаимодействия для AddAirport.xaml
    /// </summary>
    public partial class AddAirport : Window
    {
        private readonly string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";
        public AddAirport()
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
            INSERT INTO ""Airports"" (""AirportName"", ""City"", ""Country"", ""ContactInformation"",""IDOperatingStatus"")
            VALUES (@AirportName, @City, @Country, @ContactInformation,@IDOperatingStatus);";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

     
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AirportName", airportName);
                        command.Parameters.AddWithValue("@City", city);
                        command.Parameters.AddWithValue("@Country", country);
                        command.Parameters.AddWithValue("@ContactInformation", contactInformation);
                        command.Parameters.AddWithValue("@IDEmployees", GetNextEmployeeId());
                        command.Parameters.AddWithValue("@IDOperatingStatus", 1);

                        int rowsAffected = command.ExecuteNonQuery();

                         
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Аэропорт успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить аэропорт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении аэропорта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private int GetNextEmployeeId()
        {

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COALESCE(MAX(\"IDAirport\"), 0) FROM \"Airports\"";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {

                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            int maxId = Convert.ToInt32(result);
                            return maxId + 1;
                        }
                        else
                        {

                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при получении следующего ID сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = "";
            txtfamilia.Text = "";
            txtoth.Text = "";
            txtdata.Text = "";
        }
    }
}
