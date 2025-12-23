using LibraryApp.code;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
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
    public partial class AdminPage : Page
    {
        public Dictionary<string,string> Databases = new Dictionary<string, string>() 
        { 
            { "Книги", "book" }, 
            { "Авторы", "author" }, 
            { "Читатели", "readers" },
            { "Долги", "loans" },
            { "Пользователи", "users" },
        };
        public AdminPage()
        {
            InitializeComponent();
            DBSelectList.ItemsSource = Databases;
            EditButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            CreateButton.IsEnabled = false;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            dynamic Pair = DBSelectList.SelectedItem;
            var key = Pair.Value;
            
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult exit = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Удалить элемент?", MessageBoxButton.YesNo);
            if (!(exit == MessageBoxResult.Yes))
                return;
            dynamic selected_value = DataBaseTable.SelectedItem;
            dynamic Pair = DBSelectList.SelectedItem;
            var selected_item = selected_value.Id;
            var key = Pair.Value;
            try
            {

                switch (key)
                {
                    case "book":
                        LibraryCore.DeleteBookById(selected_item);
                        break;
                    case "author":
                        LibraryCore.DeleteAuthorById(selected_item);
                        break;
                    case "readers":
                        LibraryCore.DeleteReaderById(selected_item);
                        break;
                    case "loans":
                        LibraryCore.DeleteLoanById(selected_item);
                        break;
                    case "users":
                        LibraryCore.DeleteUserById(selected_item);
                        break;
                }

                
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении. Скорее всего у объекта есть связи, не дающии удалить. \n\n\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении. {ex}");
            }
            finally
            {
                UpdateTable();
            }
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult exit = MessageBox.Show("Выйти с аккаунта?", "Выход из аккаунта.", MessageBoxButton.YesNo);
            if (exit == MessageBoxResult.Yes) 
                NavigationService.Navigate(new StartPage());
        }

        private void DBSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateButton.IsEnabled = true;
            EditButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            FirstTextBox.Visibility = Visibility.Collapsed;
            SecondTextBox.Visibility = Visibility.Collapsed;
            ThirdTextBox.Visibility = Visibility.Collapsed;
            FirstTextBox.Text = "";
            SecondTextBox.Text = "";
            ThirdTextBox.Text = "";
            FirstLabel.Content = "";
            SecondLabel.Content = "";
            ThirdLabel.Content = "";
            DataBaseTable.Columns.Clear();


            dynamic Pair = DBSelectList.SelectedItem;
            var key = Pair.Value;
            switch (key)
            {
                case "book":
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    ThirdTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Название";
                    SecondLabel.Content = "Автор";
                    ThirdLabel.Content = "Дата написания";
                    DataBaseTable.Columns.Add(DataColumnCreate("ID", "Id", 0.2));
                    DataBaseTable.Columns.Add(DataColumnCreate("Название", "Name"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Автор", "Author"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Описание", "Description"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Дата написания", "ReleaseDate"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Cтраниц", "PageCount"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Количество", "Count"));
                    DataBaseTable.ItemsSource = GetBooksRows(FirstTextBox.Text, SecondTextBox.Text, ThirdTextBox.Text);
                    break;
                case "author":
                    FirstTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Имя";
                    DataBaseTable.Columns.Add(DataColumnCreate("ID","Id",0.2));
                    DataBaseTable.Columns.Add(DataColumnCreate("Имя","Name"));
                    DataBaseTable.ItemsSource = GetAuthorRows(FirstTextBox.Text);
                    break;
                case "readers":
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Имя";
                    SecondLabel.Content = "Телефон";
                    DataBaseTable.Columns.Add(DataColumnCreate("ID", "Id", 0.2));
                    DataBaseTable.Columns.Add(DataColumnCreate("Имя", "Name"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Телефон", "Phone"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Всего долгов", "Loan_Count"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Вернуто Долгов", "Returned_Loan_Count"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Просроченные Долги", "Expired_Loan_Count"));
                    DataBaseTable.ItemsSource = GetReaderRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
                case "loans":
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Читатель";
                    SecondLabel.Content = "Книга";
                    DataBaseTable.Columns.Add(DataColumnCreate("ID", "Id", 0.2));
                    DataBaseTable.Columns.Add(DataColumnCreate("Читатель", "Reader"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Книга", "Book"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Время взятия", "BurrowTime"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Время Возрата", "ReturnTime"));
                    DataBaseTable.ItemsSource = GetLoanRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
                case "users":
                    FirstTextBox.Visibility = Visibility.Visible;
                    SecondTextBox.Visibility = Visibility.Visible;
                    FirstLabel.Content = "Логин";
                    SecondLabel.Content = "Роль";
                    DataBaseTable.Columns.Add(DataColumnCreate("ID", "Id", 0.2));
                    DataBaseTable.Columns.Add(DataColumnCreate("Логин", "Name"));
                    DataBaseTable.Columns.Add(DataColumnCreate("Роль", "Role",0.5));
                    DataBaseTable.ItemsSource = GetUsersRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
            }

            UpdateTable();
        }

        private void UpdateTable()
        {


            dynamic Pair = DBSelectList.SelectedItem;
            var key = Pair.Value;
            switch (key)
            {
                case "book":
                    DataBaseTable.ItemsSource = GetBooksRows(FirstTextBox.Text, SecondTextBox.Text, ThirdTextBox.Text);
                    break;
                case "author":
                    DataBaseTable.ItemsSource = GetAuthorRows(FirstTextBox.Text);
                    break;
                case "readers":
                    DataBaseTable.ItemsSource = GetReaderRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
                case "loans":
                    DataBaseTable.ItemsSource = GetLoanRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
                case "users":
                    DataBaseTable.ItemsSource = GetUsersRows(FirstTextBox.Text, SecondTextBox.Text);
                    break;
            }
        }

        private DataGridTextColumn DataColumnCreate(string Header, string Binding, double size = 1)
        {
            DataGridTextColumn new_column = new DataGridTextColumn();
            new_column.Header = Header;
            new_column.Binding = new Binding(Binding);
            new_column.Width = new DataGridLength(size, DataGridLengthUnitType.Star);
            new_column.ElementStyle = (Style)FindResource("CenterStyle");
            return new_column;

        }


        private List<AuthorRow> GetAuthorRows(string Author_Name = "")
        {
            List<Author> authors = LibraryCore.GetAllAuthors().ToList();
            if (!string.IsNullOrEmpty(Author_Name))
                authors = authors.Where(a => a.Name.ToLower().Contains(Author_Name.ToLower())).ToList();
            List<AuthorRow> authorrows = new List<AuthorRow>();
            foreach (var author in authors)
                authorrows.Add(new AuthorRow(author.ID,author.Name));
            return authorrows;
        }

        private List<UserRow> GetUsersRows(string User_Login = "", string User_Role = "")
        {
            List<User> users = LibraryCore.GetAllUsers().ToList();
            if (!string.IsNullOrEmpty(User_Login))
                users = users.Where(a => a.Login.Contains(User_Login)).ToList();
            if (!string.IsNullOrEmpty(User_Role))
                users = users.Where(a => a.Login.ToLower().Contains(User_Role.ToLower())).ToList();
            List<UserRow> userrows = new List<UserRow>();
            foreach (var user in users)
                userrows.Add(new UserRow(user.ID,user.Login,user.UserType.Name));
            return userrows;
        }

        private List<BookRowsecond> GetBooksRows(string Name = "", string Author = "", string Date = "")
        {
            List<Book> books = LibraryCore.GetAllBooks().ToList();
            if (!string.IsNullOrEmpty(Name))
                books = books.Where(b => b.Name.ToLower().Contains(Name.ToLower())).ToList();
            if (!string.IsNullOrEmpty(Author))
                books = books.Where(b => b.Author.Name.ToLower().Contains(Author.ToLower())).ToList();
            if (!string.IsNullOrEmpty(Date))
                books = books.Where(b => b.ReleaseDate.ToShortDateString().Contains(Date)).ToList();
            List<BookRowsecond> booksrows = new List<BookRowsecond>();
            foreach (var book in books)
                booksrows.Add(new BookRowsecond(book.ID,book.Name,book.Author.Name,book.Description,book.ReleaseDate,book.PageCount,(book.Amount != null ? book.Amount.Amount : 0 )));
            return booksrows;
        }

        private List<LoanRow> GetLoanRows(string Reader = "", string Book = "")
        {
            List<Loan> loans = LibraryCore.GetAllLoans().ToList();
            if (!string.IsNullOrEmpty(Reader))
                loans = loans.Where(l => (l.Reader.Name + " " + l.Reader.Phone).ToLower().Contains(Reader.ToLower())).ToList();
            if (!string.IsNullOrEmpty(Book))
                loans = loans.Where(l => (l.Book.Name + " " + l.Book.Author.Name).ToLower().Contains(Book.ToLower())).ToList();
            List<LoanRow> booksrows = new List<LoanRow>();
            foreach (var loan in loans)
                booksrows.Add(new LoanRow(loan,loan.Reader,loan.Book));
            return booksrows;
        }

        private List<ReaderRow> GetReaderRows(string Name = "", string Phone = "")
        {
            List<Reader> readers = LibraryCore.GetAllReaders().ToList();
            if (!string.IsNullOrEmpty(Name))
                readers = readers.Where(r => r.Name.ToLower().Contains(Name.ToLower())).ToList();
            if (!string.IsNullOrEmpty(Phone))
                readers = readers.Where(r => r.Phone.Contains(Phone)).ToList();
            List<ReaderRow> booksrows = new List<ReaderRow>();
            foreach (var reader in readers)
                booksrows.Add(new ReaderRow(reader.ID,reader.Name,reader.Phone));
            return booksrows;
        }


        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTable();
        }

        private void DataBaseTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
        }
    }
}
