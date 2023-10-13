using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace auto_repair_shopProject.PartsFold
{
    public partial class SparePartsAdd : Page
    {
        private Spare_Parts _parts = new Spare_Parts();

        public SparePartsAdd(Spare_Parts parts)
        {
            InitializeComponent();

            var listService = auto_repair_shopEntities.GetContext().Services.ToList();
            comboService.ItemsSource = listService;

            if (parts != null)
            {
                _parts = parts;

                var service = listService.Where(g => g.id_service.Equals(parts.id_service));
                var serviceEnumerable = service as Services[] ?? service.ToArray();
                if (serviceEnumerable.Any())
                    comboService.SelectedValue = serviceEnumerable.First();
            }

            name.MaxLength = 30;
            desc.MaxLength = 60;
            price.MaxLength = 5;
            presence.MaxLength = 5;

            DataContext = _parts;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_parts.name_parts))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(_parts.description))
                errors.AppendLine("Введите премечание");
            if (string.IsNullOrWhiteSpace(_parts.price.ToString()))
                errors.AppendLine("Введите цену");
            if (string.IsNullOrWhiteSpace(_parts.presence.ToString()))
                errors.AppendLine("Введите количество на складе");

            if (comboService.SelectedItem is Services selectedService)
                _parts.id_service = selectedService.id_service;

            if (comboService.SelectedItem == null)
                errors.AppendLine("Выберите сервис");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_parts.id_part == 0)
                auto_repair_shopEntities.GetContext().Spare_Parts.Add(_parts);

            try
            {
                auto_repair_shopEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (_parts.photo != null || _parts.photo.Length > 0)
            {
                var parts = auto_repair_shopEntities.GetContext().Spare_Parts.Find(_parts.id_part);
                if (parts != null) parts.photo = _parts.photo;
                auto_repair_shopEntities.GetContext().SaveChanges();
            }
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFile.ShowDialog() == true)
            {
                _parts.photo = File.ReadAllBytes(openFile.FileName);
                photo.Source = LoadImage(_parts.photo);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            var image = new BitmapImage();

            using (var stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }

        private void name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^0-9]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
