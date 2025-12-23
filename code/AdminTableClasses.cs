using System.ComponentModel;

namespace LibraryApp.code
{
    class AdminTableClasses
    {

        private class BookRow : INotifyPropertyChanged
        {
            int id;
            string? name;
            string? author;
            string? description;
            DateOnly? releaseDate;
            int? pageCount;
            int? count;

            public BookRow(int id, string? Name, string? Author, string? Description, DateOnly? ReleaseDate, int? PageCount, int? Count)
            {
                this.id = id;
                this.Name = Name;
                this.Author = Author;
                this.Description = Description;
                this.ReleaseDate = ReleaseDate;
                this.PageCount = PageCount;
                this.Count = Count;
            }

            public int Id
            {
                get => id;
                set { id = value; OnPropertyChanged(nameof(id)); }
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


        private class AuthorRow : INotifyPropertyChanged
        {
            int id;
            string? name;

            public AuthorRow(int id, string? Name)
            {
                this.id = id;
                this.Name = Name;
            }

            public int Id
            {
                get => id;
                set { id = value; OnPropertyChanged(nameof(id)); }
            }

            public string? Name
            {
                get => name;
                set { name = value; OnPropertyChanged(nameof(name)); }
            }


            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }


        private class UserRow : INotifyPropertyChanged
        {
            int id;
            string? name;
            int book_count;

            public UserRow(int id, string? Name)
            {
                this.id = id;
                this.Name = Name;
                ICollection<Book>? authors_books = LibraryCore.GetBooksByAuthorId(id);
                if (authors_books == null)
                    book_count = 0;
                else 
                    book_count = authors_books.Count;
            }

            public int Id
            {
                get => id;
                set { id = value; OnPropertyChanged(nameof(id)); }
            }

            public string? Name
            {
                get => name;
                set { name = value; OnPropertyChanged(nameof(name)); }
            }


            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
