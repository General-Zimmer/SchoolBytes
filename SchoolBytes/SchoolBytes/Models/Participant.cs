public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public Participant()
    {

    }
    public Participant(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

}