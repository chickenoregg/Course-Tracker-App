using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppProj_TV.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseStatus { get; set; }
        public DateTime CourseStart { get; set; }

        public DateTime CourseEnd { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public int Term { get; set; }
        public string CourseNotes { get; set; }
        public int CourseNotification { get; set; }
    }
}
