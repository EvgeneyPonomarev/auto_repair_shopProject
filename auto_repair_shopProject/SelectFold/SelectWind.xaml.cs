using auto_repair_shopProject.PartsFold;
using auto_repair_shopProject.ServicesFold;
using System.Windows;

namespace auto_repair_shopProject.SelectFold
{
    public partial class SelectWind : Window
    {
        private Clients _client;
        private Cars _car;

        public SelectWind(Clients client, Cars car)
        {
            InitializeComponent();

            _client = client;
            _car = car;
        }

        private void parts_Click(object sender, RoutedEventArgs e)
        {
            SparePartsWind wind = new SparePartsWind(_client, _car);
            wind.Show();
            Close();
        }

        private void service_Click(object sender, RoutedEventArgs e)
        {
            ServicesWind wind = new ServicesWind(_client, _car);
            wind.Show();
            Close();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow(_client);
            wind.Show();
            Close();
        }
    }
}
