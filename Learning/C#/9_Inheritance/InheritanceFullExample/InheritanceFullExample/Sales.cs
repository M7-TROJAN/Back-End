namespace InheritanceFullExample
{
    public class Sales : Employee
    {
        protected decimal CommissionPercentage { get; set; }

        protected decimal SalesVolume { get; set; }

        public Sales(int id, string name, decimal loggedHours, decimal wage, decimal salesVolume, decimal commissionPercentage)
            : base(id, name, loggedHours, wage)
        {
            this.SalesVolume = salesVolume;

            // Convert commissionPercentage to a value between 0 and 1 if it's greater than or equal to 1
            // if it's a negative number make it 0
            if (commissionPercentage >= 1)
                commissionPercentage = Math.Round(commissionPercentage / 100, 2);
            else if (commissionPercentage < 0)
                commissionPercentage = 0;

            this.CommissionPercentage = commissionPercentage;
        }
        public override decimal CalculateSalary()
        {
            // get the  basic Salary
            decimal basicSalary = base.CalculateSalary();

            return basicSalary + CalculateBouns();
        }

        private decimal CalculateBouns()
        {
            return SalesVolume * CommissionPercentage;
        }

        public override string ToString()
        {
            return base.ToString() +
                $"\nCommission: {CommissionPercentage}" +
                $"\nBouns: {this.CalculateBouns():C}" +
                $"\nNet Salary: {this.CalculateSalary():C}";
        }

    }
}