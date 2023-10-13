using System;
using System.Linq;
using System.Windows;

namespace auto_repair_shopProject.ServicesFold
{
    public partial class ServicesWind : Window
    {
        private Clients _client;
        private Cars _car;

        public ServicesWind(Clients client, Cars car)
        {
            InitializeComponent();

            _client = client;
            _car = car;

            DataService();
        }

        private void DataService()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            LViewService.ItemsSource = auto_repair_shopEntities.GetContext().Services.ToList();
        }

        private void LViewService_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var context = auto_repair_shopEntities.GetContext();

            if (LViewService.SelectedItem is Services selectedService)
            {
                var order = new Orders
                {
                    id_client = _client.id_client,
                    id_service = selectedService.id_service,
                    id_car = _car.id_car,
                    date_order = DateTime.Now,
                    execution_status = 0,
                    price = selectedService.price,
                };

                context.Orders.Add(order);
                context.SaveChanges();
                MessageBox.Show("Заказ успешно создан");
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow(_client);
            wind.Show();
            Close();
        }

    }
}
