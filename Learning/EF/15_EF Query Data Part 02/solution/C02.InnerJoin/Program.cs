﻿using C02.InnerJoin.QueryData.Data;
using Microsoft.EntityFrameworkCore;

namespace C02.InnerJoin
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // query syntax
                var resultQuerySyntax = (from c in context.Courses.AsNoTracking()
                                         join s in context.Sections.AsNoTracking()
                                               on c.Id equals s.CourseId
                                         select new
                                         {
                                             c.CourseName,
                                             DateRange = s.DateRange.ToString(),
                                             TimeSlot = s.TimeSlot.ToString()
                                         }).ToList();

                // SELECT [c].[CourseName], [s].[Id], [s].[EndDate], [s].[StartDate], [s].[EndTime], [s].[StartTime]
                // FROM [Courses] AS [c]
                // INNER JOIN [Sections] AS [s] ON [c].[Id] = [s].[CourseId];


                // method syntax
                var resultMethodSyntax =
                    context.Courses.AsNoTracking()
                    .Join(context.Sections.AsNoTracking(),
                        c => c.Id,
                        s => s.CourseId,
                        (c, s) => new
                        {
                            c.CourseName,
                            DateRange = s.DateRange.ToString(),
                            TimeSlot = s.TimeSlot.ToString()
                        }
                    ).ToList();

                //  SELECT [c].[CourseName], [s].[Id], [s].[EndDate], [s].[StartDate], [s].[EndTime], [s].[StartTime]
                //      FROM [Courses] AS [c]
                //      INNER JOIN [Sections] AS [s] ON [c].[Id] = [s].[CourseId];

            }

            Console.ReadKey();
        }
    }
}