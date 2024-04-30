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
    /// Логика взаимодействия для EditPass.xaml
    /// </summary>
    public partial class EditPass : Window
    {
        private readonly string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";
        public EditPass()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                string firstName = txtImya.Text;
                string secondName = txtfamilia.Text;
                string lastName = txtoth.Text;
                string dateOfBirth = txtdata.Text;
                string phoneNumber = txtNumber.Text;
                string emailAdress = txtEmail.Text;
                string seriesAndPassportNumber = txtSeriesAndPassportNumber.Text;
                string nationality = txtNationality.Text;
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(secondName) || string.IsNullOrWhiteSpace(lastName) ||
              string.IsNullOrWhiteSpace(dateOfBirth) || string.IsNullOrWhiteSpace(emailAdress) || string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (firstName.Length > 30)
                {
                    MessageBox.Show("Поле 'Имя' должно содержать не более 30 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (nationality.Length > 22)
                {
                    MessageBox.Show("Поле 'Национальность' должно содержать не более 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (seriesAndPassportNumber.Length > 11)
                {
                    MessageBox.Show("Поле 'Серия и номер паспорта' должно содержать не более 11 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (secondName.Length > 40)
                {
                    MessageBox.Show("Поле 'Фамилия' должно содержать не более 40 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (lastName.Length > 43)
                {
                    MessageBox.Show("Поле 'Отчество' должно содержать не более 43 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (emailAdress.Length > 255)
                {
                    MessageBox.Show("Поле 'Почта' должно содержать не более 255 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                if (phoneNumber.Length > 16)
                {
                    MessageBox.Show("Поле 'Номер телефона' должно содержать не более 16 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string query = @"
            UPDATE ""Passengers"" 
            SET 
            ""FirstName"" = @FirstName, 
            ""SecondName"" = @SecondName, 
            ""LastName"" = @LastName, 
            ""DateOfBirth"" = @DateOfBirth, 
            ""PhoneNumber"" = @PhoneNumber, 
            ""EmailAdress"" = @EmailAdress, 
            ""SeriesAndPassportNumber"" = @SeriesAndPassportNumber, 
            ""Nationality"" = @Nationality
            WHERE ""SeriesAndPassportNumber"" = @SeriesAndPassportNumber;";

                
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                   
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@SecondName", secondName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dateOfBirth));
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@EmailAdress", emailAdress);
                        command.Parameters.AddWithValue("@SeriesAndPassportNumber", seriesAndPassportNumber);
                        command.Parameters.AddWithValue("@Nationality", nationality);

                      
                        int rowsAffected = command.ExecuteNonQuery();

                       
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Пассажир успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Пассажир с указанным серией и номером паспорта не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при редактировании пассажира: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = "";
            txtfamilia.Text = "";
            txtoth.Text = "";
            txtdata.Text = "";
            txtNumber.Text = "";
            txtEmail.Text = "";
            txtSeriesAndPassportNumber.Text = "";
            txtNationality.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string seriesAndPassportNumber = txtSeriesAndPassportNumber.Text;

                if (string.IsNullOrWhiteSpace(seriesAndPassportNumber))
                {
                    MessageBox.Show("Пожалуйста, введите серию и номер паспорта для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                string query = @"
            DELETE FROM ""Passengers"" 
            WHERE ""SeriesAndPassportNumber"" = @SeriesAndPassportNumber;";
 
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
 
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SeriesAndPassportNumber", seriesAndPassportNumber);
 
                        int rowsAffected = command.ExecuteNonQuery();
 
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Пассажир успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Пассажир с указанной серией и номером паспорта не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении пассажира: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
