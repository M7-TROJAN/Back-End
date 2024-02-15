namespace InheritanceFullExample
{
    public class Developer : Employee
    {
        private decimal commission = 0.3m;
        private bool AccomplishedTasks { get; set; }

        public Developer(int id, string name, decimal loggedHours, decimal wage, bool accomplishedTasks)
            : base(id, name, loggedHours, wage) 
        {
            AccomplishedTasks = accomplishedTasks;
        }
        public override decimal CalculateSalary()
        {
            // get the  basic Salary
            decimal basicSalary = base.CalculateSalary();

            return basicSalary + CalculateBouns();
        }

        private decimal CalculateBouns()
        {
            return AccomplishedTasks ? commission * base.CalculateSalary() : 0;
        }

        public override string ToString()
        {
            return base.ToString() +
                $"\nTask Compleated: {(AccomplishedTasks ? "Yes" : "No")}" +
                $"\nBouns: {this.CalculateBouns():C}" +
                $"\nNet Salary: {this.CalculateSalary():C}";
        }


    }
}