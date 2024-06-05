using System;

namespace Learning
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User("Mahmoud");
            user.PermissionChanged += User_PermissionChanged;

            user.SetPermission(Permissions.Read, true);
            Console.WriteLine("---------------------------");
            user.SetPermission(Permissions.Write, true);
            Console.WriteLine("---------------------------");
            user.SetPermission(Permissions.Write, false);
        }

        private static void User_PermissionChanged(User user, Permissions permission, bool isAdded)
        {
            string action = isAdded ? "granted" : "removed";
            Console.WriteLine($"{user.Name} has had the '{permission}' permission {action}.");

            Console.WriteLine("Current List of Permissions:");
            foreach (Permissions p in Enum.GetValues(typeof(Permissions)))
            {
                if (p != Permissions.None && user.HasPermission(p))
                {
                    Console.WriteLine(p);
                }
            }
        }
    }

    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        Execute = 8
    }

    class User
    {
        public User(string name)
        {
            Name = name;
            Permissions = Permissions.None;
        }

        public event Action<User, Permissions, bool> PermissionChanged;

        public string Name { get; }
        public Permissions Permissions { get; private set; }

        public bool HasPermission(Permissions permission)
        {
            return (Permissions & permission) == permission;
        }

        /// <summary>
        /// Set or unset a permission.
        /// </summary>
        /// <param name="permission">The permission to set or unset.</param>
        /// <param name="value">True to set the permission, false to unset.</param>
        public void SetPermission(Permissions permission, bool value)
        {
            if (permission == Permissions.None)
            {
                throw new ArgumentException("Permission can't be None", nameof(permission));
            }

            bool isAdded = false;

            if (value)
            {
                Permissions |= permission;
                isAdded = true;
            }
            else
            {
                Permissions &= ~permission;
            }

            PermissionChanged?.Invoke(this, permission, isAdded);
        }
    }
}
