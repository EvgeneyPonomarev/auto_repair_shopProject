using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace auto_repair_shopProject.SpecialitiesFold
{
    public partial class SpecialitiesAdmin : Page
    {
        public SpecialitiesAdmin()
        {
            InitializeComponent();
        }

        private void UpdateSpecial()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            gridSpecial.ItemsSource = auto_repair_shopEntities.GetContext().Specialities.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridSpecial.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить специальность?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (gridSpecial.SelectedItem is Specialities selectedSpecial)
                    {
                        auto_repair_shopEntities.GetContext().Specialities.Remove(selectedSpecial);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        UpdateSpecial();
                    }
                }
            }
            else
                MessageBox.Show("Выберите запись для удаления");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SpecialitiesAdd(null));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (gridSpecial.SelectedItem != null)
            {
                if (gridSpecial.SelectedItem is Specialities selectedSpec)
                NavigationService.Navigate(new SpecialitiesAdd(selectedSpec));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateSpecial();
        }
    }
}
