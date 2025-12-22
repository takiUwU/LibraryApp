using Azure.Core;
using LibraryApp.win.pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

namespace LibraryApp.win
{
    public partial class LibrarianSelectBook : Page
    {
        Reader reader;
        public LibrarianSelectBook(Reader reader)
        {
            InitializeComponent();
            TableLoad();
            this.reader = reader;
            ReaderBoxLabel.Content = $"Телефон: {reader.Phone}\nИмя: {reader.Name}";
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrarianSelectReader() { ReturnMode = false});
        }


        private void TableLoad()
        {
            BooksTable.ItemsSource = CreateBookRowTable();
        }

        private List<BookRow> CreateBookRowTable(string name, string author, DateOnly? date)
        {
            List<BookRow> books = CreateBookRowTable();
            books = books.Where(b => b.Name!.Contains(name) && b.Author!.Contains(author) && (date == null || b.ReleaseDate == date)).ToList();
            return books;
        }

        private List<BookRow> CreateBookRowTable()
        {
            List<Book> books = LibraryCore.GetAllAvailableBooks().ToList();
            List<BookRow> bookRows = new List<BookRow>();
            foreach (var book in books)
                bookRows.Add(new BookRow(book.ID,book.Name,book.Author.Name,book.Description,book.ReleaseDate,book.PageCount, book.Amount!.Amount));
            return bookRows;
        }


        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime datetime;
            DateOnly? date = null;
            if (DateTime.TryParse(DateTextBox.Text, out datetime))
            {
                date = DateOnly.FromDateTime(datetime);

            }
            BooksTable.ItemsSource = CreateBookRowTable(NameTextBox.Text, AuthorTextBox.Text, date);

        }

        private void Give_Book_Button(object sender, RoutedEventArgs e)
        {
            if (BooksTable.SelectedItem == null)
                return;
            BookGive((BookRow)BooksTable.SelectedItem);
        }


        private void BookGive(BookRow book)
        {
            try
            {

            MessageBoxResult result = MessageBox.Show($"Действительно ли вы хотите дать книгу {book.Name} человеку с телефоном {reader.Phone}? У {reader.Name} будет 14 дней для возрата.", "Дать книгу?",MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Book? added_book = LibraryCore.GetBookByID(book.original_id);
                    if (added_book == null)
                        throw new Exception("Ошибка при получении книги.");
                    LibraryCore.CreateALoan(reader, added_book);
                    MessageBox.Show("Книга была успешна отдана в долг!");
                    NavigationService.Navigate(new LibrarianPage());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private class BookRow : INotifyPropertyChanged
        {
            public int original_id = 0;
            string? name;
            string? author;
            string? description;
            DateOnly? releaseDate;
            int? pageCount;
            int? count;

            public BookRow(int original_id, string? Name, string? Author, string? Description, DateOnly? ReleaseDate, int? PageCount, int? Count)
            {
                this.original_id = original_id;
                this.Name = Name;
                this.Author = Author;
                this.Description = Description;
                this.ReleaseDate = ReleaseDate;
                this.PageCount = PageCount;
                this.Count = Count;
            }

            public string? Name 
            {
                get => name;
                set { name = value; OnPropertyChanged(nameof(name)); }
            }


            public string? Author
            {
                get => author;
                set { author = value; OnPropertyChanged(nameof(author)); }
            }

            public string? Description
            {
                get => description;
                set { description = value; OnPropertyChanged(nameof(description)); }
            }

            public DateOnly? ReleaseDate
            {
                get => releaseDate;
                set { releaseDate = value; OnPropertyChanged(nameof(releaseDate)); }
            }

            public int? PageCount
            {
                get => pageCount;
                set { pageCount = value; OnPropertyChanged(nameof(pageCount)); }
            }

            public int? Count
            {
                get => count;
                set { count = value; OnPropertyChanged(nameof(count)); }
            }


            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
