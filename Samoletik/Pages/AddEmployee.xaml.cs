using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private int? IDRoli;
        private string SelectedRoleName;
        private readonly string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";

        public AddEmployee()
        {
            InitializeComponent();
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = "";
            txtfamilia.Text = "";
            txtoth.Text = "";
            txtdata.Text = "";
            txtSeriesAndPassportNumber.Text = "";
            txtPochta.Text = "";
            cb.SelectedIndex = -1;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cb.SelectedItem;
                SelectedRoleName = selectedComboBoxItem.Content.ToString();
                int roleId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out roleId))
                {
                    IDRoli = roleId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID роли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IDRoli == null)
            {
                MessageBox.Show("Выберите роль сотрудника.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtImya.Text) || string.IsNullOrWhiteSpace(txtfamilia.Text) || string.IsNullOrWhiteSpace(txtoth.Text) ||
                 string.IsNullOrWhiteSpace(txtSeriesAndPassportNumber.Text) || string.IsNullOrWhiteSpace(txtPochta.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtImya.Text.Length > 30)
            {
                MessageBox.Show("Поле 'Имя' должно содержать не более 30 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtfamilia.Text.Length > 40)
            {
                MessageBox.Show("Поле 'Фамилия' должно содержать не более 40 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtoth.Text.Length > 43)
            {
                MessageBox.Show("Поле 'Отчество' должно содержать не более 43 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtSeriesAndPassportNumber.Text.Length > 11)
            {
                MessageBox.Show("Поле 'Серия и номер паспорта' должно содержать не более 11 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPochta.Text.Length > 16)
            {
                MessageBox.Show("Поле 'Номер телефона' должно содержать не более 16 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = @"INSERT INTO ""Employee"" 
                     (""FirstName"", ""SecondName"", ""LastName"", ""DateOfBirth"", ""SeriesAndPassportNumber"", 
                      ""PhoneNumber"", ""EmailAdress"", ""WorkSchedule"", ""WorkStage"", ""IDRoliEmployees"") 
                     VALUES 
                     (@FirstName, @SecondName, @LastName, @DateOfBirth, @SeriesAndPassportNumber, 
                      @PhoneNumber, @EmailAdress, @WorkSchedule, @WorkStage, @IDRoliEmployees);";

            try
            {
               
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                   
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                     
                        command.Parameters.AddWithValue("@FirstName", txtImya.Text);
                        command.Parameters.AddWithValue("@SecondName", txtfamilia.Text);
                        command.Parameters.AddWithValue("@LastName", txtoth.Text);
                        command.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(txtdata.Text));
                        command.Parameters.AddWithValue("@SeriesAndPassportNumber", txtSeriesAndPassportNumber.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", txtPochta.Text);
                        command.Parameters.AddWithValue("@EmailAdress", "");
                        command.Parameters.AddWithValue("@WorkSchedule", "");
                        command.Parameters.AddWithValue("@WorkStage", "");
                        command.Parameters.AddWithValue("@IDRoliEmployees", IDRoli);
                        command.Parameters.AddWithValue("@IDEmployees", GetNextEmployeeId());

                        command.ExecuteNonQuery();
 
                    

                        MessageBox.Show("Сотрудник успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private int GetNextEmployeeId()
        {

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COALESCE(MAX(\"IDEmployees\"), 0) FROM \"Employee\"";
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
    }
}
