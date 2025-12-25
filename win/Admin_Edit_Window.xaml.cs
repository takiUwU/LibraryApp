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

        List<int>? books_id;
        List<int>? readers_id;
        List<int>? authors_id;
        public Admin_Edit_Window(string mode, dynamic? target = null)
        {
            InitializeComponent();
            this.mode = mode;
            ModeLabel.Content = target != null ? "РЕДАКТИРОВАНИЕ" : "СОЗДАНИЕ";


            switch (this.mode)
            {
                case "book":
                    FirstLabel.Content = "Имя";
                    SecondLabel.Content = "Автор";
                    ThirdLabel.Content = "Описание";
                    FourthLabel.Content = "Дата выхода";
                    FifthLabel.Content = "Страниц";
                    SixthLabel.Content = "Количество";
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondComboBox.Visibility = Visibility.Visible;
                    ThirdTextBox.Visibility = Visibility.Visible;
                    FourthDatePicker.Visibility = Visibility.Visible;
                    FifthTextBox.Visibility = Visibility.Visible;
                    SixthTextBox.Visibility = Visibility.Visible;
                    authors_id = new List<int>();
                    List<string> authors = new List<string>();
                    foreach (var author in LibraryCore.GetAllAuthors())
                    {
                        authors.Add(author.Name);
                        authors_id.Add(author.ID);
                    }
                    SecondComboBox.ItemsSource = authors;
                    SecondComboBox.SelectedIndex = 0;
                    if (target != null)
                    {
                        FirstTextBox.Text = target.Name;
                        SecondComboBox.SelectedIndex = authors.IndexOf(Convert.ToString(target!.Author));
                        ThirdTextBox.Text = target.Description;
                        FourthDatePicker.Text = Convert.ToString(target.ReleaseDate);
                        FifthTextBox.Text = Convert.ToString(target.PageCount);
                        SixthTextBox.Text = Convert.ToString(target.Count);
                    }
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
                    FirstComboBox.Visibility = Visibility.Visible;
                    SecondComboBox.Visibility = Visibility.Visible;
                    ThirdDatePicker.Visibility = Visibility.Visible;
                    FourthDatePicker.Visibility = Visibility.Visible;
                    FifthTextBox.Visibility = Visibility.Visible;

                    FirstLabel.Content = "Читатель";
                    SecondLabel.Content = "Книга";
                    ThirdLabel.Content = "Дата Взятия";
                    FourthLabel.Content = "Дата Возрата";
                    FifthLabel.Content = "Пользователь";
                    List<string> books = new List<string>();
                    List<string> readers = new List<string>();
                    books_id = new List<int>();
                    readers_id = new List<int>();
                    foreach (var reader in LibraryCore.GetAllReaders())
                    {
                        readers.Add(reader.Name + " (" + reader.Phone + ")");
                        readers_id.Add(reader.ID);
                    }
                    foreach (var book in LibraryCore.GetAllBooks())
                    {
                        books.Add(book.Name + " (" + book.Author.Name + ")");
                        books_id.Add(book.ID);
                    }

                    FirstComboBox.ItemsSource = readers;
                    FirstComboBox.SelectedIndex = 0;
                    SecondComboBox.ItemsSource = books;
                    SecondComboBox.SelectedIndex = 0;

                    if (target != null)
                    {
                        FirstComboBox.SelectedIndex = readers.IndexOf(Convert.ToString(target!.Reader));
                        SecondComboBox.SelectedIndex = books.IndexOf(Convert.ToString(target!.Book));
                        ThirdDatePicker.Text = Convert.ToString(target!.BurrowTime);
                        FourthDatePicker.Text = Convert.ToString(target!.ReturnTime);
                        FifthTextBox.Text = Convert.ToString(target.UserId);
                    }

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
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstTextBox.Text, authors_id![SecondComboBox.SelectedIndex], ThirdTextBox.Text, FourthDatePicker.Text, FifthTextBox.Text, SixthTextBox.Text});
                    break;
                case "author":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstTextBox.Text});
                    break;
                case "readers":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstTextBox.Text, SecondTextBox.Text });
                    break;
                case "loans":
                    AdminPage.return_values!.AddRange(new List<dynamic> { readers_id![FirstComboBox.SelectedIndex], books_id![SecondComboBox.SelectedIndex], ThirdDatePicker.Text, FourthDatePicker.Text, (string.IsNullOrEmpty(FifthTextBox.Text) ? "" : FifthTextBox.Text)!});
                    break;
                case "users":
                    AdminPage.return_values!.AddRange(new List<dynamic> { FirstComboBox.Text, SecondTextBox.Text, ThirdTextBox.Text});
                    break;
            }
            Close();
        }
    }
}
