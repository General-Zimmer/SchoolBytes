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
        WorkBook workbook = WorkBook.Create(ExcelFileFormat.XLS);
        WorkSheet sheet = workbook.CreateWorkSheet("Attendance Report");
        
        //ws headers
        sheet["A1"].Value = "Participant Name";
        sheet["B1"].Value = "Course Name";
        sheet["C1"].Value = "Attendance (%)";
        sheet["D1"].Value = "Overall Attendance (%)";

   
        var sortedData = ExportData.OrderBy(ed => ed.participant.Name).ToList();

        int row = 2; // Start from the second row for data
        foreach (var export in sortedData)
        {
            string participantName = export.participant.Name;

            float overallAttendance = export.attendanceContainer.OverallAttendance * 100;

            bool isFirstCourseForParticipant = true;

            foreach (var courseAttendance in export.attendanceContainer.Attendances)
            {
                if (isFirstCourseForParticipant)
                {
                    sheet[$"A{row}"].Value = participantName;
                    isFirstCourseForParticipant = false;
                }

                sheet[$"B{row}"].Value = courseAttendance.Course.Name;
                sheet[$"C{row}"].Value = (courseAttendance.Attendance * 100).ToString("0.00");

                row++;
            }

            // Add the overall attendance row for the participant
            sheet[$"D{row - 1}"].Value = overallAttendance.ToString("0.00");
        }


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

    //not used in first iteration. Will be implemented properly at a later point
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