using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTS_72220538.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; } // Properti objek Category


    }
    public class CourseWithSelected : Course
    {
        public bool IsSelected { get; set; }
    }
}
