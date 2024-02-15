namespace InheritanceFullExample
{
    public class Manger : Employee
    {
        private const decimal allowanceRate = 0.05m;

        public Manger(int id, string name, decimal loggedHours, decimal wage)
            : base(id, name, loggedHours, wage) { }
        public override decimal CalculateSalary()
        {
            // get the  basic Salary
            decimal basicSalary = base.CalculateSalary();

            // Calculate manager's allowance (5% of total salary)
            decimal managerAllowance = CalculateAllowance();

            return basicSalary + managerAllowance;
        }

        private decimal CalculateAllowance()
        {
            return allowanceRate * base.CalculateSalary();
        }
        public override string ToString()
        {
            return base.ToString() +
                $"\nAllowance: {CalculateAllowance():C}" +
                $"\nNet Salary: {this.CalculateSalary():C}";
        }

    }
}