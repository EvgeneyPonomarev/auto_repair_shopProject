using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace auto_repair_shopProject.PartsFold
{
    public partial class SparePartsWind : Window
    {
        private Clients _client;
        private Cars _car;

        private List<Spare_Parts> cartItems = new List<Spare_Parts>();
        private StackPanel cartPanel;
        private TextBlock totalPriceLabel;

        public SparePartsWind(Clients client, Cars car)
        {
            InitializeComponent();

            _client = client;
            _car = car;

            PartsSort();
            InitializeUI();
        }

        private void PartsSort()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            LViewPart.ItemsSource = auto_repair_shopEntities.GetContext().Spare_Parts.ToList();
        }

        private void Search()
        {
            var currentParts = auto_repair_shopEntities.GetContext().Spare_Parts.ToList();
            currentParts = currentParts.Where(p => p.name_parts.ToString().ToLower().Contains(name.Text.ToLower())).ToList();
            LViewPart.ItemsSource = currentParts;
        }

        private void InitializeUI()
        {
            cartPanel = FindName("CartPanel") as StackPanel;
            totalPriceLabel = FindName("TotalPriceLabel") as TextBlock;
        }

        private void LViewPart_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (LViewPart.SelectedItem is Spare_Parts selectedParts)
            {
                cartItems.Add(selectedParts);
                UpdateCartUI();
            }
        }

        private void UpdateCartUI()
        {
            cartPanel.Children.Clear();
            int? totalCost = 0;

            foreach (var item in cartItems)
            {
                var cartItemUI = new StackPanel();

                var itemName = new TextBlock();
                itemName.Text = item.name_parts;
                cartItemUI.Children.Add(itemName);

                var itemImage = new Image();
                itemImage.Source = ConvertByteArrayToImage(item.photo);
                itemImage.Width = 100;
                itemImage.Height = 100;
                cartItemUI.Children.Add(itemImage);

                var itemPrice = new TextBlock();
                itemPrice.Text = item.price.ToString() + "руб.";
                cartItemUI.Children.Add(itemPrice);

                var deleteButton = new Button();
                deleteButton.Content = "X";
                deleteButton.Width = 50;
                deleteButton.Tag = item;
                deleteButton.Click += DeleteButton_Click;
                cartItemUI.Children.Add(deleteButton);

                cartPanel.Children.Add(cartItemUI);

                totalCost += item.price;
            }

            totalPriceLabel.Text = $"В общем: {totalCost:C}";

            if (cartItems.Any())
            {
                var checkoutButton = new Button();
                checkoutButton.Content = "Оформить";
                checkoutButton.Width = 200;
                checkoutButton.Margin = new Thickness(0, 10, 0, 0);
                checkoutButton.Click += CheckoutButton_Click;
                cartPanel.Children.Add(checkoutButton);

                var clearButton = new Button();
                clearButton.Content = "Очистить";
                clearButton.Width = 100;
                clearButton.Margin = new Thickness(0, 10, 0, 0);
                clearButton.Click += ClearCartButton_Click;
                cartPanel.Children.Add(clearButton);
            }
        }

        private ImageSource ConvertByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Spare_Parts itemToDelete)
            {
                cartItems.Remove(itemToDelete);
                UpdateCartUI();
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание записи в таблице History_Order 
            using (var context = new auto_repair_shopEntities())
            {
                foreach (var item in cartItems)
                {
                    var order = new History_Orders
                    {
                        id_client = _client.id_client,
                        id_car = _car.id_car,
                        id_part = item.id_part,
                    };
                    context.History_Orders.Add(order);
                }
                context.SaveChanges();
                MessageBox.Show("Покупка совершена");
            }

            cartItems.Clear();
            cartPanel.Children.Clear();
            UpdateCartUI();
        }

        private void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            cartItems.Clear();
            cartPanel.Children.Clear();
            UpdateCartUI();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow(_client);
            wind.Show();
            Close();
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void name_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я]");
            e.Handled = rgx.IsMatch(e.Text);
        }
    }
}
