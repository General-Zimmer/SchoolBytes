using System.Collections.Generic;
using SchoolBytes.Models;

public class  Registration
{
    public int Id { get; set; } 
    public virtual Participant participant { get; set; }
    public virtual CourseModule CourseModule { get; set; }
    public Registration()
    {
            
    }

    public Registration(Participant newParticipant, CourseModule courseModule)
    {
        this.participant = newParticipant;
        this.CourseModule = courseModule;
    }
}
