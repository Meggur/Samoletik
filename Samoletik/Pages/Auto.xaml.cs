using System;
using System.Collections.Generic;
using System.IO;
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
 
using Npgsql;
using Samoletik.Model;

namespace Samoletik.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auto.xaml
    /// </summary>
    public partial class Auto : Page
    {
        private readonly string connectionString;
        public Auto()
        {
            InitializeComponent();
            connectionString = "Host=localhost;Database=Samoletik;Username=postgres;Password=18042005;Persist Security Info=True";

        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password.Trim();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT re.\"RoliEmployees\" FROM \"Employee\" e INNER JOIN \"RoliEmployees\" re ON e.\"IDRoliEmployees\" = re.\"IDRoliEmployees\" WHERE e.\"Login\" = @Login";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string roleEmployee = reader["RoliEmployees"].ToString();
                                Console.WriteLine("Роль сотрудника из базы данных: " + roleEmployee);
                                Employee polzov = new Employee();
                                polzov.RoliEmployees = new RoliEmployees { RoliEmployees1 = roleEmployee };
                                if (polzov.RoliEmployees != null && polzov.RoliEmployees.RoliEmployees1 != null)
                                {
                                    string roleName = polzov.RoliEmployees.RoliEmployees1.ToString();
                                    MessageBox.Show("Вы вошли под: " + roleName);
                                    LoadForm(roleName, polzov);
                                }
                                else
                                {
                                    throw new Exception("У сотрудника не указана роль.");
                                }
                            }

                            else
                            {
                                throw new Exception("Пользователь с указанным логином и паролем не найден.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadForm(string roleName, Employee polzov)
        {
            switch (roleName)
            {
                case "Пилот":
                    NavigationService.Navigate(new Pilot());
                    break;
                case "Бортпроводник":
                    NavigationService.Navigate(new FlightAttendant());
                    break;
                case "Диспетчер":
                    NavigationService.Navigate(new Dispatcher());
                    break;
                case "Директор":
                    NavigationService.Navigate(new Director());
                    break;
                case "Администратор":
                    NavigationService.Navigate(new Admin());
                    break;
                case "Инженер":
                    NavigationService.Navigate(new Engineer());
                    break;
                case "Уборщик":
                    NavigationService.Navigate(new Cleaner());
                    break;
                case "Метеоролог":
                    NavigationService.Navigate(new Meteorologist());
                    break;
                case "Связист":
                    NavigationService.Navigate(new Telecommunications());
                    break;
                case "Зам.Директор":
                    NavigationService.Navigate(new DeputyDirector());
                    break;
                case "Кассир":
                    NavigationService.Navigate(new Cashier());
                    break;
                default:
                    MessageBox.Show("Неверная роль пользователя.");
                    break;
            }
        }

    }
}
