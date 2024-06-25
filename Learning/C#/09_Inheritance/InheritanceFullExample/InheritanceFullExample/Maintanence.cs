namespace InheritanceFullExample
{
    public class Maintanence : Employee
    {
        private const decimal hardshipAllowance = 100m; //(بدل المشقة)

        public Maintanence(int id, string name, decimal loggedHours, decimal wage) 
            :base(id, name, loggedHours, wage) { } 
        public override decimal CalculateSalary()
        {
            // get the  basic Salary
            decimal basicSalary = base.CalculateSalary();

            return basicSalary + hardshipAllowance;
        }

        public override string ToString()
        {
            return base.ToString() +
                $"\nHardShip: {hardshipAllowance}" +
                $"\nNet Salary: {this.CalculateSalary():C}";
        }


    }
}