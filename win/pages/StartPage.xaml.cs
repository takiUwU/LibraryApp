using System;
using System.Collections;
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

namespace LibraryApp.win.pages
{
    public partial class StartPage : Page
    {
        //  26.203.160.220\SQLEXPRESS,1433   taki     4444
        public StartPage()
        {
            InitializeComponent();
            LoadStartUpData();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (settings_page.Visibility == Visibility.Collapsed)
                settings_page.Visibility = Visibility.Visible;
            //MainFrame.MainFrame.Core.RegisterNewUser("Admin1", "4444", "Admin");
            //MainFrame.MainFrame.Core.RegisterNewUser("Lib1", "1234", "Librarian");
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            TryLogin(LoginTextBox.Text, PasswordTextBox.Password);
        }

        private void TryLogin(string Login, string password)
        {
            try
            {
                SaveStartUpData();
                if (settings_page.Visibility == Visibility.Visible)
                    throw new Exception("Завершите настройки сервера.");

                User? user = null;
                string message = "";
                (user, message) = LibraryCore.EnteredUserIsCorrect(Login, password);
                if (user != null)
                {
                    var role = LibraryCore.GetUserRole(user);
                    switch (role)
                    {
                        case "Admin":
                            NavigationService.Navigate(new AdminPage());
                            break;
                        case "Librarian":
                            NavigationService.Navigate(new LibrarianPage());
                            break;
                        default:
                            throw new Exception("Произошла ошибка при понятии роли!");
                    }

                }
                else
                    MessageBox.Show(message);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show($"Проблема произошла при попытки связи с базе данных.\n\n Ошибка: {ex.Message}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void SaveStartUpData()
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\LibraryApp";
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            try
            {                
                using (FileStream fs = new FileStream($"{path}\\enterdata.json", FileMode.OpenOrCreate))
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
            catch (Exception e){
                MessageBox.Show(e.Message);
            }
        }

        private void LoadStartUpData()
        {
            try
            {
                var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\LibraryApp";
                if (!Directory.Exists(path))
                    return;
                using (FileStream fs = new FileStream($"{path}\\enterdata.json", FileMode.Open))
                {
                    Dictionary<string, string>? load = JsonSerializer.Deserialize<Dictionary<string, string>>(fs);
                    if (load != null)
                    {
                        ServerLinkTextBox.Text = load["ServerLink"];
                        ServerNameTextBox.Text = load["ServerName"];
                        ServerPasswordTextBox.Password = load["ServerPassword"];
                        SaveLoginCheckBox.IsChecked = Convert.ToBoolean(load["RememberLogin"]);
                        LoginTextBox.Text = load["UserLogin"];

                        SetServer(load["ServerLink"], load["ServerName"], load["ServerPassword"]);
                    }
                }
            }
            catch { }

        }

        private void Server_Button_Click(object sender, RoutedEventArgs e)
        {
            settings_page.Visibility = Visibility.Collapsed;
            SetServer(ServerLinkTextBox.Text, ServerNameTextBox.Text, ServerPasswordTextBox.Password);
            SaveStartUpData();
        }

        private void SetServer(string serverName, string Username, string Password)
        {
            LibraryCore.SetServer(ServerLinkTextBox.Text, ServerNameTextBox.Text, ServerPasswordTextBox.Password);
        }
    }
}
