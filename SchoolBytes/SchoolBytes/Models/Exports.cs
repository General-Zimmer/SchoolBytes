using IronXL;
using SchoolBytes.Models;
using SchoolBytes.util;
using System.Collections.Generic;
using System.Linq;

public class Exports
{
    internal List<ExportData> ExportData {  get; set; }
    public Exports()
    {
        ExportData = new List<ExportData>();
    }

    public WorkBook ConvertToXls()
    {
        WorkBook workbook = new WorkBook();


        return workbook;
    }


}

public class ExportsBuilder
{
    private DBConnection db = DBConnection.getDBContext();
    private Exports exports;
    private string className = "All";
    private Participant participant = null;
    private float percentageCriteria = 101f;
    private List<Participant> participants = null;

    public ExportsBuilder()
    {
        this.exports = new Exports();
    }

    public ExportsBuilder ForClass(string className)
    {
        this.className = className;
        return this;
    }

    public ExportsBuilder ForParticipant(Participant participant)
    {
        this.participant = participant;
        return this;
    }
    public ExportsBuilder ForParticipant(List<Participant> participants)
    {
        this.participants = participants;
        return this;
    }

    public ExportsBuilder WithAttendanceBelow(float percent)
    {
        this.percentageCriteria = percent;
        return this;
    }

    public Exports Build()
    {
        //if queury for participants is set
        if(participant != null)
        {
            this.exports.ExportData.Add(produceInfo(participant));
        }

        //if queury for participants is set
        if (participants != null)
        {
            foreach(Participant p in participants)
            {
                this.exports.ExportData.Add(produceInfo(p));
            }

        }
        return this.exports;
    }



    private ExportData produceInfo(Participant p)
    {
        ExportData ed = new ExportData();
        ed.participant = p;
        //setting up container
        ed.attendanceContainer = new AttendanceContainer();
        List<Course> courses;
        if (className == "All")
        {

           courses = DatabaseUtils.GetCoursesByParticipant(p);
        }
        else
        {
            courses = DatabaseUtils.GetCourseByName(className);
        }

        courses.ForEach(course =>
        {
            ed.attendanceContainer.Attendances.Add(new CourseAttendance(course));
        });
        ed.attendanceContainer.CalcOverallAttendance(p);


        return ed;
    }


}

internal class ExportData
{
    public Participant participant { get; set; }
    public AttendanceContainer attendanceContainer { get; set; }

}

internal class AttendanceContainer
{
    public float OverallAttendance {  get; set; }
    public List<CourseAttendance> Attendances { get; set; }

    public void CalcOverallAttendance(Participant p)
    {
        Attendances.ForEach(attendance => attendance.CalcCourseAttendance(p));
        OverallAttendance = Attendances.Sum(a => a.Attendance) / Attendances.Count;
    }

}

internal class CourseAttendance
{
    public float Attendance { get; set; }
    public Course Course { get; set; }

    public CourseAttendance(Course c)
    {
        Course = c;
    }

    public void CalcCourseAttendance(Participant participant)
    {
        float attended = Course.CoursesModules.Sum(cm => cm.Registrations.Where(r => r.participant == participant && r.Attendance).Count());
        float notAttended = Course.CoursesModules.Sum(cm => cm.Registrations.Where(r => r.participant == participant && !r.Attendance).Count());

        Attendance = (attended)/(notAttended+attended);
    }

} 