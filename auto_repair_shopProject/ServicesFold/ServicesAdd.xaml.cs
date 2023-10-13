using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace auto_repair_shopProject.ServicesFold
{
    public partial class ServicesAdd : Page
    {
        private Services _services = new Services();

        public ServicesAdd(Services services)
        {
            InitializeComponent();

            if (services != null)
                _services = services;

            name.MaxLength = 60;
            desc.MaxLength = 60;
            price.MaxLength = 3;

            DataContext = _services;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_services.name_service))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(_services.description))
                errors.AppendLine("Введите примечание");
            if (string.IsNullOrWhiteSpace(_services.price.ToString()))
                errors.AppendLine("Введите цену");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_services.id_service == 0)
                auto_repair_shopEntities.GetContext().Services.Add(_services);

            try
            {
                auto_repair_shopEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void name_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^0-9]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
