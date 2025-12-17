using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryApp
{
    public partial class StartUpWindow : Window
    {
        //  26.203.160.220\SQLEXPRESS,1433   taki     4444
        LibraryCore Core = null;
        public StartUpWindow()
        {
            InitializeComponent();
            LoadStartUpData();
            Core = new LibraryCore(ServerLinkTextBox.Text, ServerNameTextBox.Text, ServerPasswordTextBox.Password);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (settings_page.Visibility == Visibility.Collapsed)
                settings_page.Visibility = Visibility.Visible;
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            //Core.RegisterNewUser("Admin1","4444","Admin");
            try
            {
                SaveStartUpData();
                if (settings_page.Visibility == Visibility.Visible)
                    throw new Exception("Завершите настройки сервера.");

                bool enter = false;
                string error = "";
                (enter, error) = Core.EnterUserIsCorrect(LoginTextBox.Text, PasswordTextBox.Password);
                if (enter)
                {
                    MessageBox.Show("YES");
                }
                else
                    MessageBox.Show(error);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show($"Проблема произошла при попытки связи с базе данных. Попросите помощи у администраторов.\n\n Ошибка: {ex.Message}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SaveStartUpData()
        {
            try
            {

                using (FileStream fs = new FileStream("enterdata.json", FileMode.OpenOrCreate))
                {
                    Dictionary<string, string> save = new();
                    save["ServerLink"] = ServerLinkTextBox.Text;
                    save["ServerName"] = ServerNameTextBox.Text;
                    save["ServerPassword"] = ServerPasswordTextBox.Password;
                    save["UserLogin"] = SaveLoginCheckBox.IsChecked == true ? LoginTextBox.Text : "";
                    save["RememberLogin"] = SaveLoginCheckBox.IsChecked == true ? "True" : "False";
                    await JsonSerializer.SerializeAsync<Dictionary<string, string>>(fs, save);
                }
            }
            catch { }
        }

        private async void LoadStartUpData()
        {
            try
            {

                using (FileStream fs = new FileStream("enterdata.json", FileMode.Open))
                {
                    Dictionary<string, string>? load = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
                    if (load != null)
                    {
                        ServerLinkTextBox.Text = load["ServerLink"];
                        ServerNameTextBox.Text = load["ServerName"];
                        ServerPasswordTextBox.Password = load["ServerPassword"];
                        SaveLoginCheckBox.IsChecked = Convert.ToBoolean(load["RememberLogin"]);
                        LoginTextBox.Text = load["UserLogin"];
                    }
                }
            }
            catch { }

        }

        private void Server_Button_Click(object sender, RoutedEventArgs e)
        {
            settings_page.Visibility = Visibility.Collapsed;
            Core = new LibraryCore(ServerLinkTextBox.Text, ServerNameTextBox.Text, ServerPasswordTextBox.Password);
        }
    }
}