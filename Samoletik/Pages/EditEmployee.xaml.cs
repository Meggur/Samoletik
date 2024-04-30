using Npgsql;
using Samoletik.Model;
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
    /// Логика взаимодействия для EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        private readonly string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";
        public Employee Employee { get; set; }
        public EditEmployee()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string phoneNumber = txtNumber.Text;

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("Пожалуйста, введите номер телефона сотрудника для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string query = @"
            DELETE FROM ""Employee"" 
            WHERE ""PhoneNumber"" = @PhoneNumber;";

             
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
 
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                       
                        int rowsAffected = command.ExecuteNonQuery();

                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Сотрудник успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник с указанным номером телефона не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = "";
            txtfamilia.Text = "";
            txtoth.Text = "";
            txtdata.Text = "";
            txtEmail.Text = "";
            txtNumber.Text = "";
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
                UPDATE ""Employee"" 
                SET 
                ""FirstName"" = @FirstName, 
                ""SecondName"" = @SecondName, 
                ""LastName"" = @LastName, 
                ""DateOfBirth"" = @DateOfBirth, 
                ""PhoneNumber"" = @PhoneNumber, 
                ""EmailAdress"" = @EmailAdress
                WHERE ""PhoneNumber"" = @PhoneNumber;";
                // Создаем подключение к базе данных
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                   

                    // Создаем команду с параметрами
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@SecondName", secondName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dateOfBirth));
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@EmailAdress", emailAdress);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Сотрудник успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при редактировании сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
