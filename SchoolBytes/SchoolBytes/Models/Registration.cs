using System.Collections.Generic;
using SchoolBytes.Models;

public class  Registration
{
    public Participant participant { get; set; }
    public virtual List<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

    public Registration(Participant participant, List<CourseModule> courseModules)
    {
        this.participant = participant;
        CourseModules = courseModules;
    }

    public Registration(Participant newParticipant, CourseModule courseModule)
    {

    }
}

