using SPS.Model;

public abstract class User
{
	public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Telephone { get; set; }

    public string RG { get; set; }

    public string CPF { get; set; }

    public Address Address { get; set; }

    public string Password { get; set; }

}

