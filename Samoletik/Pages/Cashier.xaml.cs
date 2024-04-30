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
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>
    public partial class Cashier : Page
    {
        public ObservableCollection<Passengers> Employees { get; set; }
        public Cashier()
        {
            InitializeComponent();
            DataContext = this;
            Employees = new ObservableCollection<Passengers>();
            LoadPass();
        }
        private void LoadPass()
        {
            try
            {
                string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM \"Passengers\"";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Passengers
                                {
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                                    LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? string.Empty : reader.GetString(reader.GetOrdinal("LastName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    EmailAdress = reader.IsDBNull(reader.GetOrdinal("EmailAdress")) ? string.Empty : reader.GetString(reader.GetOrdinal("EmailAdress")),
                                    SeriesAndPassportNumber = reader.IsDBNull(reader.GetOrdinal("SeriesAndPassportNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("SeriesAndPassportNumber")),
                                    Nationality = reader.IsDBNull(reader.GetOrdinal("Nationality")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nationality")),
                                   
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPass addSotr = new AddPass();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadPass();
            }
        }

        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            Cashier newAdminPage = new Cashier();
            NavigationService.Navigate(newAdminPage);
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProduct.SelectedItem != null)
            {
                EditPass editWindow = new EditPass();
                editWindow.DataContext = LViewProduct.SelectedItem;
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadPass();
                }
            }
        }
    }
}
