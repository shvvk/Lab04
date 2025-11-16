using System.Linq;
using System.Runtime.CompilerServices;

namespace Lab04
{
    internal class Program
    {
        public abstract class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Person()
            {
                FirstName = string.Empty;
                LastName = string.Empty;
            }
            public Person(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public override string ToString()
            {
                return $"{FirstName} {LastName}";
                ;
            }
        }

        public class Author : Person
        {
            public string Nationality { get; set; }
            public Author() : base()
            {
                Nationality = string.Empty;
            }
            public Author(string firstName, string lastName, string nationality) : base(firstName, lastName)
            {
                Nationality = nationality;
            }

            public override string ToString()
            {
                return $"{FirstName} {LastName} {Nationality}";
            }
        }

        public class Librarian : Person
        {
            public DateTime HireDate { get; set; }
            public decimal Salary { get; set; }
            public Librarian() : base()
            {
                HireDate = DateTime.MinValue;
                Salary = 0;
            }
            public Librarian(string firstName, string lastName, DateTime hireDate, decimal salary) : base(firstName, lastName)
            {
                HireDate = hireDate;
                Salary = salary;
            }
            public override string ToString()
            {
                return $"{base.ToString} {HireDate} {Salary}";
            }
        }

        public interface IPrintable
        {
            public abstract void Print();
        }
        public interface IBorrowable
        {
            public abstract void Borrow();
            public abstract void Return();
            public abstract void IsAvailable();
        }
        public interface IBarCodeGenerator
        {
            public abstract string GenerateBarCode();
        }

        public abstract class Item : IBarCodeGenerator
        {
            public DateTime DateOfIssue { get; set; }
            public int Id { get; set; }
            public string Publisher { get; set; }
            public string Title { get; set; }

            public bool Avilable { get; set; } = true;

            public virtual string GenerateBarCode()
            {
                return string.Empty;
            }
            public Item()
            {
                DateOfIssue = DateTime.MinValue;
                Id = 0;
                Publisher = string.Empty;
                Title = string.Empty;
            }

            public Item(DateTime dateOfIssue, int id, string publisher, string title)
            {
                DateOfIssue = dateOfIssue;
                Id = id;
                Publisher = publisher;
                Title = title;
            }

            public override string ToString()
            {
                return $"{Title} {Id} {Publisher} {DateOfIssue}";
            }
        }

        public class  Journal : Item, IBorrowable
        {
            public int Number { get; set;}
            public Journal() : base()
            {
                Number = 0;
            }

            public Journal(string title, int id, string publisher, DateTime dateOfIssue, int number) : base(dateOfIssue, id, publisher, title)
            {
                Number = number;
            }

            

            public void Borrow()
            {
                Avilable = false;
                Console.WriteLine($"Journal {Title} borrowed.");
            }

            public void Return()
            {
                Avilable = true;
                Console.WriteLine($"Journal {Title} returned.");
            }

            public void IsAvailable()
            {
                if (Avilable)
                {
                    Console.WriteLine($"Journal {Title} is available.");
                }
                else
                {
                    Console.WriteLine($"Journal {Title} is not available.");
                }
            }

            public override string GenerateBarCode()
            {
                return $"J-{Id}-{DateOfIssue}-{Number}";
            }

            public override string ToString()
            {
                return $"{Title}/{Id}/{Publisher}/{DateOfIssue}/{Number}";
            }
        }

        public class Book : Item, IBorrowable, IPrintable
        {
            public List<Author> Authors { get; set; }
            public int PageCount { get; set; }
            public Book() : base()
            {
                Authors = new List<Author>();
                PageCount = 0;
            }
            public Book(string title, int id, string publisher, DateTime dateOfIssue, int pageCount, List<Author> authors) : base(dateOfIssue, id, publisher, title)
            {
                Authors = authors;
                this.PageCount = pageCount;
            }

            public void Print()
            {
                Console.WriteLine($"Printing Book: {Title}");
                Console.WriteLine($"printing will use {PageCount} pages");
            }
            public void Borrow()
            {
                Avilable = false;
                Console.WriteLine($"Book {Title} borrowed.");
            }

            public void Return()
            {
                Avilable = true;
                Console.WriteLine($"Book {Title} returned.");
            }

            public void IsAvailable()
            {
                if (Avilable)
                {
                    Console.WriteLine($"Book {Title} is available.");
                }
                else
                {
                    Console.WriteLine($"Book {Title} is not available.");
                }
            }


            public void AddAuthor(Author author)
            {
                Authors.Add(author);
            }
            public void RemoveAuthor(Author author)
            {
                Authors.Remove(author);
            }

            public override string GenerateBarCode()
            {
                return $"B-{Id}-{DateOfIssue}-{Publisher}";
            }
            public override string ToString()
            {
                return $"{Title}/{Id}/{Publisher}/{DateOfIssue}/{PageCount}/{string.Join(", ", Authors)}";
            }
        }

        public interface IItemManagement
        {
            public Item FindItem(Func<Item, bool> predicate);
            public Item FindItemBy(int id);
            public Item FindItemBy(string title);
            public string GetAllItems(string s = "");
        }

        public class Catalog : IItemManagement
        {
            public List<Item> Items;
            public string ThematicDepartment { get; set; }

            public Catalog()
            {
                Items = new List<Item>();
            }

            public Catalog(string thematicDepartment,List<Item>items)
            {
                ThematicDepartment = thematicDepartment;
                Items = items ?? new List<Item>();
            }

            public void AddItem(Item item)
            {
                Items.Add(item);
            }
            public Item FindItem(Func<Item, bool> predicate)
            {
                return Items.Find(item => predicate(item));
            }
            public Item FindItemBy(int id) =>
                FindItem(item => item.Id == id);
            public Item FindItemBy(string title)
            {
                return Items.Find(item => item.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            }
            public string GetAllItems(string s = "")
            {
                var filteredItems = string.IsNullOrEmpty(s) ? Items : Items.FindAll(item => item.Title.Contains(s, StringComparison.OrdinalIgnoreCase));
                return string.Join(Environment.NewLine, filteredItems.ConvertAll(item => item.ToString()));
            }
            public void RemoveItem(Item item)
            {
                Items.Remove(item);
            }

            public override string ToString()
            {
                return $"{ThematicDepartment}: {string.Join(", ", Items.ConvertAll(i => i.ToString()))}";
            }
        }

        public class Library : IItemManagement
        {
            public string Address { get; set; }
            public List<Catalog> Catalogs { get; set; }
            public List<Librarian> Librarians { get; set; }
            
            public Library()
            {
                Catalogs = new List<Catalog>();
                Librarians = new List<Librarian>();
                Address = string.Empty;
            }
            public Library(string address, List<Librarian> librarians, List<Catalog> catalogs)
            {
                Address = address;
                Catalogs = catalogs ?? new List<Catalog>();
                Librarians = librarians ?? new List<Librarian>();
            }

            public void AddCatalog(Catalog catalog)
            {
                Catalogs.Add(catalog);
            }

            public void AddItem(Item item, string thematicDepartment)
            {
                var catalog = Catalogs.Find(c => c.ThematicDepartment.Equals(thematicDepartment, StringComparison.OrdinalIgnoreCase));
                if (catalog != null)
                {
                    catalog.AddItem(item);
                }
                else
                {
                    var newCatalog = new Catalog(thematicDepartment, new List<Item> { item });
                    Catalogs.Add(newCatalog);
                }
            }

            public void AddLibrarian(Librarian librarian)
            {
                Librarians.Add(librarian);
            }
            public Item FindItem(Func<Item, bool> predicate)
            {
                foreach (var catalog in Catalogs)
                {
                    var item = catalog.FindItem(predicate);
                    if (item != null)
                        return item;
                }
                return null;
            }
            public Item? FindItemBy(int id)
            {
                return FindItem(item => item.Id == id);
            }
            public Item FindItemBy(string title)
            {
                return FindItem(item => item.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            }
            public string GetAllItems(string s = "")
            {
                List<string> allItems = new List<string>();
                foreach (var catalog in Catalogs)
                {
                    allItems.Add(catalog.GetAllItems(s));
                }
                return string.Join(Environment.NewLine, allItems);
            }

            public string GetAllLibrarians()
            {
                return string.Join(Environment.NewLine, Librarians.ConvertAll(l => l.ToString()));
            }

            public void RemoveCatalog(Catalog catalog)
            {
                Catalogs.Remove(catalog);
            }

            public void RemoveLibrarian(Librarian librarian)
            {
                Librarians.Remove(librarian);
            }

            public override string ToString()
            {
                return $"Biblioteka : {Address}";
            }
        }

        public class GroupedItemsByKeyReport<TKey, TValue>
        {
            public List<TValue> Items { get; set; }
            public TKey Key { get; set; }
            public GroupedItemsByKeyReport(TKey key, List<TValue> items)
            {
                Key = key;
                Items = items;
            }
            public override string ToString()
            {
                string itemsText = Items != null && Items.Count > 0
                    ? string.Join(Environment.NewLine, Items.ConvertAll(i => i.ToString()))
                    : "No Items";
                return $"Key: {Key}\nItems:\n{itemsText}\n";
            }
        }

        static class GroupItemsHelper
        {
            public static List<GroupedItemsByKeyReport<TKey, TValue>>? GroupItemsBy<TKey, TValue>(
            IEnumerable<TValue> items,
            Func<TValue, TKey> keySelector)
            {
                if (items == null || !items.Any())
                    return null;
                var groupedItems = items
                    .GroupBy(keySelector)
                    .Select(g => new GroupedItemsByKeyReport<TKey, TValue>(g.Key, g.ToList()))
                    .ToList();
                return groupedItems;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy operacji podstawowych");
            Console.WriteLine("-----------------------------------------------------------");
            Author a1 = new("Anna", "Tracz", "polska");
            Author a2 = new("Jan", "Bielik", "polska");
            Author a3 = new("Maria", "Miłka", "polska");
            Item i1 = new Book("Kompendium programisty", 2, "PolPress", new DateTime(2065, 12, 06), 500, [a1, a2]);
            ((Book)i1).AddAuthor(a3);
            Item i4 = new Book("Kompendium administratora baz danych", 3, "PolPress", new DateTime(2065, 05, 01), 500, [a3]);
            ((Book)i4).AddAuthor(a1);Item i2 = new Journal("Przegląd techniczny", 1, "MyPress", new DateTime(2060, 06, 01), 1);
            var bookBarCode = ((Book)i1).GenerateBarCode();
            Console.WriteLine($"{i1} \r\n Kod kreskowy: {bookBarCode}");
            var journalBarCode = ((Journal)i2).GenerateBarCode();
            Console.WriteLine($"{i2} \r\n Kod kreskowy: {journalBarCode}");
            List<Item> items1 = [i1, i2, i4];
            Catalog c1 = new("Książki o programowaniu", items1);
            c1.AddItem(new Journal("Wzorce programistyczne", 1, "ITPress", new DateTime(2060, 02, 14), 1));
            Console.WriteLine(c1);
            Console.WriteLine('\n' + c1.GetAllItems("Wszystkie pozycje w katalogu:"));
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy wyszukiwania przedmiotów");
            Console.WriteLine("-----------------------------------------------------------");
            string searchedValue = "Kompendium programisty";
            Item? foundedItemByTitle = c1?.FindItem(item => item.Title == searchedValue);
            Item? foundedItemById = c1?.FindItem(item => item.Id == 1);
            Item? foundedItemByDateRange = c1?.FindItem(
            item => item.DateOfIssue >= new DateTime(2055, 01, 01) &&
            item.DateOfIssue <= new DateTime(2085, 12, 31)
            );
            Console.WriteLine("Wyszukanie po id:\n" + foundedItemById);
            Console.WriteLine("Wyszukanie po tytule:\n" + foundedItemByTitle);
            Console.WriteLine("Wyszukanie po datach:\n" + foundedItemByDateRange);
            Item? foundedItemByIdOld = c1?.FindItemBy(1);
            Item? foundedItemByTitleOld = c1?.FindItemBy(searchedValue);
            Console.WriteLine("Wyszukanie po id (wersja 2):\n" + foundedItemByIdOld);
            Console.WriteLine("Wyszukanie po tytule (wersja 2):\n" + foundedItemByTitleOld);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy dla bibliotek");
            Console.WriteLine("-----------------------------------------------------------");
            Person l1 = new Librarian("Maja", "Kowal", DateTime.Now.Date, 2040);
            Person l2 = new Librarian("Jan", "Morus", DateTime.Now.Date, 2040);
            Library lib1 = new("Częstochowa, Armii Krajowej 36", [(Librarian)l1], []);
            lib1.AddLibrarian((Librarian)l2);
            Console.WriteLine($"Wszyscy bibliotekarze: {lib1.GetAllLibrarians}");
            Catalog c2 = new("Powieści", []);
            lib1.AddCatalog(c2);
            if (c1 != null) lib1.AddCatalog(c1);
            Item i3 = new Book("Głos większości", 4, "Nasze wersy", new DateTime(2061, 03, 08), 800, [a1]);
            lib1.AddItem(i3, "Powieści");
            Console.WriteLine(lib1);
            Console.WriteLine(lib1.GetAllItems("Wszystkie pozycje w bibliotece:"));
            var foundedById = lib1.FindItemBy(4);
            var foundedByTitle = lib1.FindItemBy(searchedValue);
            var foundedByLambda = lib1.FindItem(x => x.Publisher == "ITPress");
            Console.WriteLine(foundedById);
            Console.WriteLine(foundedByTitle);
            Console.WriteLine(foundedByLambda);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy dla dodawania i usuwania przedmiotów");
            Console.WriteLine("-----------------------------------------------------------");
            Catalog c3 = new("Literatura fantastyczna", []);
            Library lib2 = new("Warszawa, Marszałkowska 12", [], [c3]);
            var bookToAdd = new Book("Lot ku centrum", 5, "Super Press", new DateTime(2060, 06, 01), 350, [a1]);
            c3.AddItem(bookToAdd);
            Console.WriteLine("Po dodaniu książki:");
            Console.WriteLine(c3.GetAllItems("Pozycje w katalogu:"));
            c3.RemoveItem(bookToAdd);
            Console.WriteLine("Po usunięciu książki:");
            Console.Write(c3.GetAllItems("Pozycje w katalogu:"));
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy wyszukiwania z bardziej złożonymi warunkami");
            Console.WriteLine("-----------------------------------------------------------");
            var foundByMultipleConditions =lib1?.FindItem(x => x.Publisher == "ITPress" && x.DateOfIssue >= new DateTime(2040, 01, 01));
            Console.WriteLine("Wyszukiwanie po wielu warunkach:");
            Console.WriteLine(foundByMultipleConditions);
            Console.WriteLine("Pozycja zawierająca w tytule 'ę':");
            Console.WriteLine(lib1?.Catalogs[0].FindItem(c =>c.Title.Contains('ę'))?.ToString());
            Console.WriteLine("Pozycja zawierająca w tytule 'ę' (z Expression):");
            //Console.WriteLine(lib1?.Catalogs[0].FindItemWithExpression(c =>c.Title.Contains('ę'))?.ToString());
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy dla błędnych argumentów");
            Console.WriteLine("-----------------------------------------------------------");
            Item? nonExistentItem = lib1?.FindItemBy(999);
            Console.WriteLine("Wyszukiwanie nieistniejącego przedmiotu (ID 999):");
            Console.WriteLine(nonExistentItem?.ToString() ?? "Nie znaleziono przedmiotu");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy grupowania po roku wydania (metoda generyczna)");
            Console.WriteLine("-----------------------------------------------------------");
            lib1 ??= new();
            List<GroupedItemsByKeyReport<int, Item>>? groupedByYear =
            GroupItemsHelper.GroupItemsBy(
            lib1.Catalogs.SelectMany(c => c.Items).ToList(),
            item => item.DateOfIssue.Year
            );
            if (groupedByYear is not null) foreach (var e in
            groupedByYear) Console.WriteLine(e);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy grupowania po wydawcy (metoda generyczna)");
            Console.WriteLine("-----------------------------------------------------------");
            List<GroupedItemsByKeyReport<string, Item>>? groupedByPublisher =
            GroupItemsHelper.GroupItemsBy(
            lib1.Catalogs.SelectMany(c => c.Items).ToList(),
            item => item.Publisher
            );
            if (groupedByPublisher is not null) foreach (var e in groupedByPublisher)
                    Console.WriteLine(e);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy grupowania książek po liczbie stron (m. generyczna)");
            Console.WriteLine("-----------------------------------------------------------");
            List<GroupedItemsByKeyReport<int, Book>>? groupedByPageCount =
            GroupItemsHelper.GroupItemsBy(
            lib1.Catalogs.SelectMany(c => c.Items).OfType<Book>().ToList(),
            item => item.PageCount
            );
            if (groupedByPageCount is not null) foreach (var e in groupedByPageCount)
                    Console.WriteLine(e);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testy grupowania autorów po narodowości (m. generyczna)");
            Console.WriteLine("-----------------------------------------------------------");
            List<GroupedItemsByKeyReport<string, Author>>? groupedByNationality =
            GroupItemsHelper.GroupItemsBy(
            lib1.Catalogs
            .SelectMany(c => c.Items)
            .OfType<Book>()
            .SelectMany(b => b.Authors)
            .Distinct()
            .ToList(),
            item => item.Nationality
            );
            if (groupedByNationality is not null) foreach (var e in groupedByNationality)
                    Console.WriteLine(e);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Testowanie metod wlasnych");
            Console.WriteLine("-----------------------------------------------------------");
            ((Book)i1).Print();
            ((Book)i4).Borrow();
            ((Book)i4).IsAvailable();
            ((Book)i4).Return();
            ((Book)i4).IsAvailable();
        }
    }
}
