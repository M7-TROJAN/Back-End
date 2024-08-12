using C04.CrossJoin.QueryData.Data;

namespace C04.CrossJoin
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Query syntax
                // ربط كل مدرس مع كل قسم بغض النظر اذا كان يعطيه او لا
                var sectionInstructorQuerySyntax =
                        (from s in context.Sections // 200
                         from i in context.Instructors // 100
                         select new
                         {
                             s.SectionName,
                             i.FullName
                         }).ToList();


                // the code above will translate to the following SQL query

                // SELECT [s].[SectionName], [i].[Id], [i].[FName], [i].[LName], [i].[OfficeId]
                // FROM [Sections] AS [s]
                // CROSS JOIN [Instructors] AS [i]

                Console.WriteLine(sectionInstructorQuerySyntax.Count()); // 20000


                //--------------------------------------------------------------------------------


                // method syntax
                var sectionInstructorMethodSyntax = context.Sections
                 .SelectMany(
                     s => context.Instructors,
                     (s, i) => new { s.SectionName, i.FullName }
                 ).ToList();

                // the code above will translate to the following SQL query

                // SELECT [s].[SectionName], [i].[Id], [i].[FName], [i].[LName], [i].[OfficeId]
                // FROM [Sections] AS [s]
                // CROSS JOIN [Instructors] AS [i]


                Console.WriteLine(sectionInstructorMethodSyntax.Count());
            }

            Console.ReadKey();
        }
    }
}