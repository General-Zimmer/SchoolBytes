public class Participant
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int Id { get; set; }

    public Participant()
    {

    }
    public Participant(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public override string ToString()
    {
        return Name;
    }

}