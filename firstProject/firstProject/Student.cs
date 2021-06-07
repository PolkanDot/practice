using System.Collections.Generic;

namespace firstProject
{
    public class Student
    {
        public string Name;
        public int Age;
        public List<int> Marks;

        public Student( string name, int age )
        {
            Name = name;
            Age = age;
            Marks = new List<int>();
        }

        public void AddMark( int mark )
        {
            Marks.Add( mark );
        }

        public double GetAverageScore()
        {
            double result = 0;
            foreach ( var mark in Marks )
            {
                result += mark;
            }

            return result / Marks.Count;
        }

        public override string ToString()
        {
            return $"Имя студента: {Name}";
        }
    }
}
