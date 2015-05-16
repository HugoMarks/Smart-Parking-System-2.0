using SPS.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class User
{
    [Key]
    public int Id { get; set; }

	public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Telephone { get; set; }

    public uint StreetNumber { get; set; }

    public string RG { get; set; }

    public string CPF { get; set; }

    public Address Address { get; set; }

    public string Password { get; set; }
}

