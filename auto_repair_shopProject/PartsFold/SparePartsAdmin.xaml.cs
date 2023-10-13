using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace auto_repair_shopProject.PartsFold
{
    public partial class SparePartsAdmin : Page
    {
        public SparePartsAdmin()
        {
            InitializeComponent();
        }

        private void UpdateParts()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            gridParts.ItemsSource = auto_repair_shopEntities.GetContext().Spare_Parts.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridParts.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запчасть?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (gridParts.SelectedItem is Spare_Parts selectedParts)
                    {
                        auto_repair_shopEntities.GetContext().Spare_Parts.Remove(selectedParts);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        UpdateParts();
                    }
                }
            }
            else
                MessageBox.Show("Выберите запись для удаления");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SparePartsAdd(null));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (gridParts.SelectedItem != null)
            {
                if (gridParts.SelectedItem is Spare_Parts selectedParts)
                    NavigationService.Navigate(new SparePartsAdd(selectedParts));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateParts();
        }
    }
}
