namespace Clients.Model
{
    public sealed class UserRole
    {
        public static UserRole Administrator = new(100, "Администратор");

        private UserRole(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        public static UserRole Citizen { get; } = new(1, "Житель");

        public static UserRole Guest { get; } = new(2, "Гость");
    }
}