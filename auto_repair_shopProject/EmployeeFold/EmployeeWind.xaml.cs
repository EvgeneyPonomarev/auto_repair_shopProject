using auto_repair_shopProject.LogIn;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace auto_repair_shopProject.EmployeeFold
{
    public partial class EmployeeWind : Window
    {
        private Employees _employee;

        public EmployeeWind(Employees employee)
        {
            InitializeComponent();

            _employee = employee;

            UpdateOrders();
        }

        private void UpdateOrders()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            viewNewOrder.ItemsSource = auto_repair_shopEntities.GetContext().Orders.ToList();
        }

        private void viewNewOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewNewOrder.SelectedItem != null)
            {
                if (viewNewOrder.SelectedItem is Orders selectedOrder)
                {
                    if (MessageBox.Show("Вы хотите завершить заказ?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var client = auto_repair_shopEntities.GetContext().Clients.FirstOrDefault(p => p.id_client == selectedOrder.id_client);

                        var complitedOrder = new History_Orders
                        {
                            id_client = client.id_client,
                            id_services = selectedOrder.id_service,
                            id_car = selectedOrder.id_car,
                            id_employer = _employee.id_employee,
                        };

                        auto_repair_shopEntities.GetContext().History_Orders.Add(complitedOrder);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        MessageBox.Show("Заказ завершен");
                    }
                    auto_repair_shopEntities.GetContext().Orders.Remove(selectedOrder);
                    auto_repair_shopEntities.GetContext().SaveChanges();
                    UpdateOrders();
                }
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            LogWind wind = new LogWind();
            wind.Show();
            Close();
        }

        private void allOrders_Click(object sender, RoutedEventArgs e)
        {
            AllOrdersWind wind = new AllOrdersWind(_employee);
            wind.Show();
            Close();
        }
    }
}
