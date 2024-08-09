using EF013.TPH.Data;
using EF013.TPH.Entities;

namespace EF013.TPH
{
    class Program
    {
        public static void Main(string[] args)
        {
            var participant1 = new Individual
            {
                Id = 1,
                FName = "Mahmoud",
                LName = "Mattar",
                University = "MIT",
                YearOfGraduation = 2020,
                IsIntern = true
            };

            var participant2 = new Coporate
            {
                Id = 2,
                FName = "Asmaa",
                LName = "Adel",
                Company = "Microsoft",
                JobTitle = "Software Engineer"
            };

            using (var context = new AppDbContext())
            {
                context.Particpants.Add(participant1);
                context.Particpants.Add(participant2);
                context.SaveChanges();
            }

            using (var context = new AppDbContext())
            {
                Console.WriteLine("Coporate Participants");
                foreach (var participant in context.Set<Participant>().OfType<Coporate>())
                {
                    Console.WriteLine(participant);
                }
                Console.WriteLine("Individual Participants");

                foreach (var participant in context.Set<Participant>().OfType<Individual>())
                {
                    Console.WriteLine(participant);
                }
            }

            Console.ReadKey();
        }
    }
}

