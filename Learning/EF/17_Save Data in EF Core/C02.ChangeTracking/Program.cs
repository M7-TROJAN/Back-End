using C02.ChangeTracking.Data;
using C02.ChangeTracking.Helpers;
using C02.ChangeTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace C02.ChangeTracking
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            AsTrackedEntity();
            // AsUnTrackedEntity();

            // Inserting_New_Principal_Author();
            // Inserting_New_Principal_Author_With_Dependent_Book();

            //Attaching_Existing_Author_Principal();

            // Updating_Existing_Author_Principal();

            // Deleting_Existing_Author_Principal();
            // Deleting_Dependent_Book();

            Console.ReadKey();
        }

        public static void AsTrackedEntity()
        {
            Console.WriteLine($">>>> Sample: {nameof(AsTrackedEntity)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var author = context.Authors.First(); // DB Query using LINQ

                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

                author.FName = "Whatever";
                Console.WriteLine("After Changing FName");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

                context.Entry(author).State = EntityState.Modified;
                Console.WriteLine("After Changing State to modified, before save changes");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

                context.SaveChanges();
                Console.WriteLine("After Save Changes");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

            }
        }

        public static void AsUnTrackedEntity()
        {
            Console.WriteLine($">>>> Sample: {nameof(AsUnTrackedEntity)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var author = context.Authors.AsNoTracking().First();
                Console.WriteLine(context.ChangeTracker.DebugView.LongView); // nothing will be tracked
            }
        }
        public static void Inserting_New_Principal_Author()
        {
            Console.WriteLine($">>>> Sample: {nameof(Inserting_New_Principal_Author)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();

            using (var context = new AppDbContext())
            {
                // Mark book  as added
                context.Add(new Author { Id = 1, FName = "Eric", LName = "Evans" });

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);


                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }
        }
        public static void Inserting_New_Principal_Author_With_Dependent_Book()
        {
            Console.WriteLine($">>>> Sample: {nameof(Inserting_New_Principal_Author_With_Dependent_Book)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();

            using (var context = new AppDbContext())
            {
                context.Add(
                    new Author
                    {
                        Id = 1,
                        FName = "Eric",
                        LName = "Evans",
                        Books = new List<Book>
                        {
                          new Book
                          {
                              Id = 1,
                              Title = "Domain-Driven Design: Tackling Complexity in the Heart of Software"
                          },
                          new Book
                          {
                              Id = 2,
                              Title = "Domain-Driven Design Reference: Definitions and Pattern Summaries"
                          }
                        }
                    });

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);


                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }
        }

        public static void Attaching_Existing_Author_Principal()
        {
            Console.WriteLine($">>>> Sample: {nameof(Attaching_Existing_Author_Principal)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var author = new Author { Id = 1, FName = "Eric", LName = "Evans" }; // this is a disconnected entity (it's allready in the database but not tracked by the context)

                context.Attach(author); // attach the entity to the context (it will be tracked)

                author.LName = "Evanzzz";

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }

            Console.ReadKey();
        }

        public static void Updating_Existing_Author_Principal()
        {
            Console.WriteLine($">>>> Sample: {nameof(Updating_Existing_Author_Principal)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                // Mark book  as modified
                context.Update(new Author { Id = 1, FName = "EricAAAAA", LName = "Evans" });

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);


                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }
        }

        public static void Deleting_Existing_Author_Principal()
        {
            Console.WriteLine($">>>> Sample: {nameof(Deleting_Existing_Author_Principal)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                // Mark author as Deleted
                context.Remove(new Author { Id = 1 }); // this will delete the author  with id 1 and all its books

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);


                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }
        }
        public static void Deleting_Dependent_Book()
        {
            Console.WriteLine($">>>> Sample: {nameof(Deleting_Dependent_Book)}");
            Console.WriteLine();

            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var book = DatabaseHelper.GetDisconnectedBook();

                context.Attach(book); // attach the entity to the context (it will be tracked)

                // Mark book  as Deleted
                context.Remove(book); 

                Console.WriteLine("Before SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);


                context.SaveChanges();

                Console.WriteLine("After SaveChanges:");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            }
        }
    }
}
