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
    /// Логика взаимодействия для Pilot.xaml
    /// </summary>
    public partial class Pilot : Page
    {
        public ObservableCollection<Airports> Employees { get; set; }
        public Pilot()
        {
            InitializeComponent();
            DataContext = this;
            Employees = new ObservableCollection<Airports>();
            LoadAirports();
        }
        private void LoadAirports()
        {
            try
            {
                string connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM \"Airports\"";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Airports
                                {
                                    AirportName = reader.GetString(reader.GetOrdinal("AirportName")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? string.Empty : reader.GetString(reader.GetOrdinal("Country")),
                                    ContactInformation = reader.GetString(reader.GetOrdinal("ContactInformation")),
                                       };
                                Employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке аэропортов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddAirport addSotr = new AddAirport();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadAirports();
            }
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProduct.SelectedItem != null)
            {
                EditAirports editWindow = new EditAirports();
                editWindow.DataContext = LViewProduct.SelectedItem;
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadAirports();
                }
            }
        }

        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            Pilot newAdminPage = new Pilot();
            NavigationService.Navigate(newAdminPage);
        }
    }
}
