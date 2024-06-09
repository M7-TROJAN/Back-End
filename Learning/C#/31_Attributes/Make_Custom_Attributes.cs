using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<Player>
            {
                new Player("Lionel Messi", 9, 18, 4, 85, 990),
                new Player("Christiano Ronaldo", 9, 18, 4, 94, 999),
                new Player("Mohamed Salah", 7, 16, 4, 84, 1010)
            };

            var errors = ValidatePlayers(players);

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("All player attributes are valid.");
            }

            Console.ReadKey();
        }

        static List<Error> ValidatePlayers(List<Player> players)
        {
            var errors = new List<Error>();

            foreach (var player in players)
            {
                Type type = player.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (var property in properties)
                {
                    var skillAttribute = property.GetCustomAttribute<SkillAttribute>();

                    if (skillAttribute != null)
                    {
                        var value = property.GetValue(player);
                        if (!skillAttribute.IsValid(value))
                        {
                            errors.Add(new Error(property.Name, $"Value of {property.Name} ({value}) is not valid. It should be between {skillAttribute.Minimum} and {skillAttribute.Maximum}."));
                        }
                    }
                }
            }

            return errors;
        }
    }

    class Player
    {
        public string Name { get; set; }

        [Skill("Ball Control", 1, 10)]
        public int BallControl { get; set; } // 1 - 10

        [Skill("Dribbling", 1, 20)]
        public int Dribbling { get; set; } // 1 - 20

        [Skill("Power", 1, 1000)]
        public int Power { get; set; } // 1 - 1000

        [Skill("Speed", 1, 100)]
        public int Speed { get; set; } // 1 - 100

        [Skill("Passing", 1, 5)]
        public int Passing { get; set; } // 1 - 5

        public Player(string name, int ballControl, int dribbling, int passing, int speed, int power)
        {
            Name = name;
            BallControl = ballControl;
            Dribbling = dribbling;
            Passing = passing;
            Speed = speed;
            Power = power;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Ball Control: {BallControl}, Dribbling: {Dribbling}, Passing: {Passing}, Speed: {Speed}, Power: {Power}";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SkillAttribute : Attribute
    {
        public SkillAttribute(string name, int minimum, int maximum)
        {
            Name = name;
            Minimum = minimum;
            Maximum = maximum;
        }

        public string Name { get; }
        public int Minimum { get; }
        public int Maximum { get; }

        public bool IsValid(object value)
        {
            if (value is int intValue)
            {
                return intValue >= Minimum && intValue <= Maximum;
            }
            return false;
        }
    }

    public class Error
    {
        public string Field { get; }
        public string Details { get; }

        public Error(string field, string details)
        {
            Field = field;
            Details = details;
        }

        public override string ToString()
        {
            return $"{{\"{Field}\": \"{Details}\"}}";
        }
    }
}
