using LibraryApp.code;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
        public StartPage()
        {
            InitializeComponent();
            LoadStartUpData();
        }

        private async void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in OpacityBox.Children)
                child.IsEnabled = false;
            OpacityBox.Opacity = 0.4;
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;
            Task new_task = await Task.Run(() => Task.FromResult(TryLogin(login, password)));
            foreach (UIElement child in OpacityBox.Children)
                child.IsEnabled = true;
            OpacityBox.Opacity = 1;
        }

        private async Task TryLogin(string Login, string password)
        {
            try
            {
                SaveStartUpData();

                User? user = null;
                string message = "";
                (user, message) = LibraryCore.EnteredUserIsCorrect(Login, password);
                if (user != null)
                {
                    var role = LibraryCore.GetUserRole(user);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
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
                    });

                }
                else
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        MessageBox.Show(message);
                    });
                }
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
                    await Application.Current.Dispatcher.InvokeAsync(() => {
                    save["UserLogin"] = SaveLoginCheckBox.IsChecked == true ? LoginTextBox.Text : "";
                    save["RememberLogin"] = SaveLoginCheckBox.IsChecked == true ? "True" : "False";
                    });
                    
                    await JsonSerializer.SerializeAsync<Dictionary<string, string>>(fs, save);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadStartUpData()
        {

            var config = new ConfigurationBuilder().AddUserSecrets<LoginDataClass>().Build();

            var secretProvider = config.Providers.First();
            secretProvider.TryGet("ServerConnection", out var ServerConnection);
            secretProvider.TryGet("ServerLogin", out var ServerLogin);
            secretProvider.TryGet("ServerPassword", out var ServerPassword);
            if (ServerConnection == null)
            {
                MessageBox.Show("Завершите настройку серверя для продолжения.");
                Environment.Exit(0);
            }
            SetServer(ServerConnection, ServerLogin, ServerPassword);

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
                        SaveLoginCheckBox.IsChecked = Convert.ToBoolean(load["RememberLogin"]);
                        LoginTextBox.Text = load["UserLogin"];
                    }
                }
            }
            catch (FileNotFoundException)
            { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }




        private void SetServer(string serverName, string? Username = "", string? Password = "")
        {
            LibraryCore.SetServer(serverName, Username, Password);
        }
    }
}
