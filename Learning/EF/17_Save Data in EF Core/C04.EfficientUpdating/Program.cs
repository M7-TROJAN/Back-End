using C04.EfficientUpdating.Data;
using C04.EfficientUpdating.Helpers;
using Microsoft.EntityFrameworkCore;

namespace C04.EfficientUpdating
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Increase_Book_Price_By_10Percent_For_Author1_Typical_Implementation();
            // Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_Implementation();
            // Delete_Book_With_Title_Start_With_Book_EF7AnUp_Implementation();
            Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_RawSql();

            Console.ReadKey();

        }

        public static void Increase_Book_Price_By_10Percent_For_Author1_Typical_Implementation()
        {
            Console.WriteLine($">>>> Sample: {nameof(Increase_Book_Price_By_10Percent_For_Author1_Typical_Implementation)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var author1Books = context.Books.Where(x => x.AuthorId == 1);

                foreach (var book in author1Books) // Deffered Execution
                {
                    book.Price *= 1.1m;
                }

                context.SaveChanges();
            }
        }
        public static void Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_Implementation()
        {
            Console.WriteLine($">>>> Sample: {nameof(Increase_Book_Price_By_10Percent_For_Author1_Typical_Implementation)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                context.Books.Where(x => x.AuthorId == 1)
                       .ExecuteUpdate(b => b.SetProperty(p => p.Price, p => p.Price * 1.1m));
            }
        }
        public static void Delete_Book_With_Title_Start_With_Book_EF7AnUp_Implementation()
        {
            Console.WriteLine($">>>> Sample: {nameof(Delete_Book_With_Title_Start_With_Book_EF7AnUp_Implementation)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                context.Books.Where(x => x.Title.StartsWith("Book")).ExecuteDelete();
            }
        }

        public static void Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_RawSql()
        {
            Console.WriteLine($">>>> Sample: {nameof(Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_RawSql)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var authorId = 1;

                context.Database.ExecuteSql($"UPDATE dbo.Books SET Price = Price * 1.1 WHERE AuthorId = {authorId}");
            }
        }
    }
}
