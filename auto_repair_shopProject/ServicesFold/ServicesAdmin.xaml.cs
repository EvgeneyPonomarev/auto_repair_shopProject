using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace auto_repair_shopProject.ServicesFold
{
    public partial class ServicesAdmin : Page
    {
        public ServicesAdmin()
        {
            InitializeComponent();
        }

        private void UpdateServices()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            gridService.ItemsSource = auto_repair_shopEntities.GetContext().Services.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridService.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить сервис?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (gridService.SelectedItem is Services selectedServices)
                    {
                        auto_repair_shopEntities.GetContext().Services.Remove(selectedServices);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        UpdateServices();
                    }
                }
            }
            else
                MessageBox.Show("Выберите запись для удаления");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServicesAdd(null));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (gridService.SelectedItem != null)
            {
                if (gridService.SelectedItem is Services selectedService)
                    NavigationService.Navigate(new ServicesAdd(selectedService));
            }
            else
                MessageBox.Show("Выберите запись для изменения");
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateServices();
        }
    }
}
