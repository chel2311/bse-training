namespace CustomerManagerDB
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;

        public Customer(int id, string name, string phone, int age, string email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Age = age;
            Email = email;
        }
    }
}
