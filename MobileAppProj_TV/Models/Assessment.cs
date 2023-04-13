using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppProj_TV.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AssessmentTitle { get; set; }
        public DateTime AssessmentStart { get; set; }
        public DateTime AssessmentEnd { get; set; }
        public string AssessmentType { get; set; }
        public int Course { get; set; }
        public int AssessmentNotification { get; set; }
    }
}
