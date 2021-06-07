using System.Collections.Generic;

namespace firstProject
{
    public class Group
    {
        public string GroupName;
        public int GroupCapacity;
        public List<Student> Students;

        public Group( string groupName )
        {
            GroupName = groupName;
            GroupCapacity = 0;
            Students = new List<Student>();
        }

        public Group( string groupName, int groupCapacity, List<Student> students )
        {
            GroupName = groupName;
            GroupCapacity = groupCapacity;
            Students = students;
        }

        public Student GetByName( string name )
        {
            foreach ( var student in Students )
            {
                if ( student.Name == name )
                {
                    return student;
                }
            }

            return null;
        }

        public void AddStudent( Student student )
        {
            Students.Add( student );
        }

        public void DeleteStudent( Student student )
        {
            Students.Remove( student );
        }

        public List<Student> GetStudents()
        {
            return Students;
        }
    }
}
