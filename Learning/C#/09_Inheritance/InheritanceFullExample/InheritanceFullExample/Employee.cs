
// Base class representing an Employee
namespace InheritanceFullExample
{
    public class Employee
    {
        // Constants
        private const int MinimumLoggedHours = 176;
        private const decimal OvertimeRate = 1.25m;

        // Properties
        protected int Id { get; set; }
        protected string Name { get; set; } = string.Empty;
        protected decimal LoggedHours { get; set; }
        protected decimal Wage { get; set; }

        // Constructor
        public Employee(int id, string name, decimal loggedHours, decimal wage)
        {
            this.Id = id;
            this.Name = name;
            this.LoggedHours = loggedHours;
            this.Wage = wage;
        }

        // Calculate basic salary based on logged hours and wage
        private decimal CalculateBasicSalary()
        {
            return LoggedHours * Wage;
        }

        // Calculate overtime pay
        private decimal CalculateOvertime()
        {
            //var additionalHours = (LoggedHours - MinimumLoggedHours) > 0 ? (LoggedHours - MinimumLoggedHours) : 0;

            decimal additionalHours = Math.Max(LoggedHours - MinimumLoggedHours, 0);
            return additionalHours * Wage * OvertimeRate;
        }

        // Calculate total salary including basic salary and overtime pay
        public virtual decimal CalculateSalary()
        {
            return CalculateBasicSalary() + CalculateOvertime();
        }

        public override string ToString()
        {
            var type = GetType().ToString().Replace("InheritanceFullExample.", "");
            return $"\n{type}" +
                   $"\nId: {this.Id}" +
                   $"\nName: {this.Name}" +
                   $"\nLogged Hours: {this.LoggedHours}" +
                   $"\nWage: ${this.Wage} / hr" +
                   $"\nBase Salary: {this.CalculateBasicSalary():C}" +
                   $"\nOverTime: {this.CalculateOvertime():C}";

        }
    }
}