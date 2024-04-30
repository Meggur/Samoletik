using Npgsql;
using Samoletik.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Samoletik.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public Admin()
        {
            InitializeComponent();
            DataContext = this;
            Employees = new ObservableCollection<Employee>();
            LoadEmployee();
        }

        private void LoadEmployee()
        {
            try
            {
                string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM \"Employee\"";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Employee
                                {
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                                    LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? string.Empty : reader.GetString(reader.GetOrdinal("LastName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    EmailAdress = reader.IsDBNull(reader.GetOrdinal("EmailAdress")) ? string.Empty : reader.GetString(reader.GetOrdinal("EmailAdress")),
                                    Login = reader.IsDBNull(reader.GetOrdinal("Login")) ? string.Empty : reader.GetString(reader.GetOrdinal("Login")),
                                    WorkSchedule = reader.IsDBNull(reader.GetOrdinal("WorkSchedule")) ? string.Empty : reader.GetString(reader.GetOrdinal("WorkSchedule")),
                                    WorkStage = reader.IsDBNull(reader.GetOrdinal("WorkStage")) ? string.Empty : reader.GetString(reader.GetOrdinal("WorkStage"))
                                };
                                Employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке сотрудников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            Admin newAdminPage = new Admin();
            NavigationService.Navigate(newAdminPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee addSotr = new AddEmployee();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadEmployee();
            }
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProduct.SelectedItem != null)
            {
                EditEmployee editWindow = new EditEmployee();
                editWindow.DataContext = LViewProduct.SelectedItem;
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadEmployee();
                }
            }
        }
    }
}
