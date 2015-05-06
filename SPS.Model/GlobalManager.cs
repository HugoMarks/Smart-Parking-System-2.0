namespace SPS.Model
{
    public class GlobalManager : IUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public int Id { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public Address Address { get; set; }

        public string Password { get; set; }

        public Parking Parking { get; set; }
    }
}
