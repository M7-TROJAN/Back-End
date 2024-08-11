using EF015.QueryData.Data;
using Microsoft.EntityFrameworkCore;

namespace C04.RelatedData.Eager
{
    class Program
    {
        public static void Main(string[] args)
        {
            //using (var context = new AppDbContext())
            //{
            //    var sectionId = 1;

            //    var sectionQuery = context.Sections
            //        .Include(x => x.Participants)
            //        .Where(x => x.Id == sectionId);

            //    Console.WriteLine(sectionQuery.ToQueryString());


            //    //DECLARE @__sectionId_0 int = 1;

            //    //SELECT[s].[Id], [s].[CourseId], [s].[InstructorId], [s].[ScheduleId], [s].[SectionName], [s].[EndDate], [s].[StartDate],
            //    //[s].[EndTime], [s].[StartTime], 
            //    //[t0].[SectionId], [t0].[ParticipantId], [t0].[Id], [t0].[FName], [t0].[LName], [t0].[Company], [t0].[JobTitle],
            //    //[t0].[IsIntern], [t0].[University], [t0].[YearOfGraduation], [t0].[Discriminator]
            //    //FROM[Sections] AS[s]
            //    //LEFT JOIN(
            //    //    SELECT[e].[SectionId], [e].[ParticipantId], [t].[Id], [t].[FName], [t].[LName], [t].[Company],
            //    //    [t].[JobTitle], [t].[IsIntern], [t].[University], [t].[YearOfGraduation], [t].[Discriminator]
            //    //    FROM [Enrollments] AS [e]
            //    //    INNER JOIN (
            //    //        SELECT[p].[Id], [p].[FName], [p].[LName], [c].[Company], [c].[JobTitle], [i].[IsIntern],
            //    //        [i].[University], [i].[YearOfGraduation], CASE
            //    //            WHEN [i].[Id] IS NOT NULL THEN N'Individual'
            //    //            WHEN[c].[Id] IS NOT NULL THEN N'Corporate'
            //    //        END AS[Discriminator]
            //    //        FROM[Particpants] AS[p]
            //    //        LEFT JOIN[Coporates] AS[c] ON[p].[Id] = [c].[Id]
            //    //        LEFT JOIN[Individuals] AS[i] ON[p].[Id] = [i].[Id]
            //    //    ) AS[t] ON[e].[ParticipantId] = [t].[Id]
            //    //) AS[t0] ON[s].[Id] = [t0].[SectionId]
            //    //WHERE[s].[Id] = @__sectionId_0
            //    //ORDER BY[s].[Id], [t0].[SectionId], [t0].[ParticipantId]


            //    var section = sectionQuery.FirstOrDefault();
            //    Console.WriteLine($"section: {section.SectionName}");
            //    Console.WriteLine($"--------------------");
            //    foreach (var participant in section.Participants)
            //        Console.WriteLine($"[{participant.Id}] {participant.FName} {participant.LName}");

            //}

            using (var context = new AppDbContext())
            {
                var sectionId = 1;

                var sectionQuery = context.Sections
                    .Include(x => x.Instructor)
                    .ThenInclude(x => x.Office)
                    .Where(x => x.Id == sectionId);

                Console.WriteLine(sectionQuery.ToQueryString());


                var section = sectionQuery.FirstOrDefault();

                Console.WriteLine($"section: {section.SectionName} " +
                    $"[{section.Instructor.FName} " +
                    $"{section.Instructor.LName} " +
                    $"({section.Instructor.Office.OfficeName})]");

            }

            Console.ReadKey();
        }
    }
}
