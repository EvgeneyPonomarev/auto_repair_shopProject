using auto_repair_shopProject.CarsFold;
using auto_repair_shopProject.LogIn;
using auto_repair_shopProject.ProfileFold;
using auto_repair_shopProject.SelectFold;
using System.Linq;
using System.Windows;

namespace auto_repair_shopProject
{
    public partial class MainWindow : Window
    {
        Clients _client;

        public MainWindow(Clients client)
        {
            InitializeComponent();
            _client = client;

            ClientCars();
        }

        private void ClientCars()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            LViewCar.ItemsSource = auto_repair_shopEntities.GetContext().Cars.Where(p => p.id_client == _client.id_client).ToList();
        }

        private void LViewCar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (LViewCar.SelectedItem != null)
            {
                if (LViewCar.SelectedItem is Cars selectedCar)
                {
                    SelectWind wind = new SelectWind(_client, selectedCar);
                    wind.Show();
                    Close();
                }
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (LViewCar.SelectedItem != null)
            {
                if (LViewCar.SelectedItem is Cars selectedCar)
                {
                    CreateCarWind wind = new CreateCarWind(_client, selectedCar);
                    wind.Show();
                    Close();
                }
            }
        }

        private void profile_Click(object sender, RoutedEventArgs e)
        {
            Profile wind = new Profile(_client);
            wind.Show();
            Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (LViewCar.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить машину?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (LViewCar.SelectedItem is Cars selectedCar)
                    {
                        auto_repair_shopEntities.GetContext().Cars.Remove(selectedCar);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        ClientCars();
                    }
                }
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            LogWind wind = new LogWind();
            wind.Show();
            Close();
        }
    }
}
