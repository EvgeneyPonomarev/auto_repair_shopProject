using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace auto_repair_shopProject.SpecialitiesFold
{
    public partial class SpecialitiesAdd : Page
    {
        private Specialities _specialities = new Specialities();

        public SpecialitiesAdd(Specialities selectedSpec)
        {
            InitializeComponent();

            if (selectedSpec != null)
                _specialities = selectedSpec;

            name.MaxLength = 20;

            DataContext = _specialities;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_specialities.name_specialties))
                errors.AppendLine("Введите название");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_specialities.id_specialties == 0)
                auto_repair_shopEntities.GetContext().Specialities.Add(_specialities);

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

        private void name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
