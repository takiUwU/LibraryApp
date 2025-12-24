using LibraryApp.win.pages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryApp.win
{
    public partial class Admin_Edit_Window : Window
    {
        public dynamic? result;
        string mode = "";
        public Admin_Edit_Window(string mode, dynamic? target = null)
        {
            InitializeComponent();
            this.mode = mode;
            ModeLabel.Content = target != null ? "РЕДАКТИРОВАНИЕ" : "СОЗДАНИЕ";


            switch (this.mode)
            {
                case "book":
                    
                    break;
                case "author":
                    FirstLabel.Content = "Имя";
                    FirstTextBox.Visibility = Visibility.Visible;
                    if (target != null)
                        FirstTextBox.Text = target.Name;
                    break;
                case "readers":
                    FirstLabel.Content = "Имя";
                    SecondLabel.Content = "Телефон";
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    if (target != null)
                    {
                        FirstTextBox.Text = target.Name;
                        SecondTextBox.Text = target.Phone;
                    }
                    break;
                case "loans":
                    
                    break;
                case "users":
                    FirstComboBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    ThirdTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Тип";
                    SecondLabel.Content = "Логин";
                    ThirdLabel.Content = "Пароль";
                    List<string> types = new List<string>();
                    foreach (var usertype in LibraryCore.GetAllUserTypes())
                        types.Add(usertype.Name);
                    FirstComboBox.ItemsSource = types;
                    FirstComboBox.SelectedIndex = 0;
                    if (target != null)
                    {
                        FirstComboBox.SelectedIndex = types.IndexOf(Convert.ToString(target!.Role));
                        SecondTextBox.Text = target.Name;
                    }
                    break;
            }
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            switch (mode)
            {
                case "book":

                    break;
                case "author":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstTextBox.Text});
                    break;
                case "readers":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstTextBox.Text, SecondTextBox.Text });
                    break;
                case "loans":

                    break;
                case "users":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstComboBox.Text, SecondTextBox.Text, ThirdTextBox.Text});
                    break;
            }
            Close();
        }
    }
}
