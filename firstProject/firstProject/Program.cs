using System;
using System.Collections.Generic;

namespace firstProject
{
    class Program
    {
        static void Main( string[] args )
        {
            var group = new Group( "ПС-12" );
            var student = new Student( "Виталий", 19 );
            var student2 = new Student( "Николай", 19 );
            var student3 = new Student( "Сергей", 19 );

            student.AddMark( 5 );
            student.AddMark( 5 );
            student.AddMark( 5 );

            student2.AddMark( 4 );
            student2.AddMark( 4 );
            student2.AddMark( 5 );

            student3.AddMark( 2 );
            student3.AddMark( 2 );
            student3.AddMark( 2 );

            group.AddStudent( student );
            group.AddStudent( student2 );
            group.AddStudent( student3 );

            var commandType = CommandType.KeepWork;

            while ( commandType == CommandType.KeepWork )
            {
                Console.WriteLine( "Enter keepWork for keep work \n" +
                    "Enter stop for stop" );
                var currentCommand = Console.ReadLine();

                if ( currentCommand == "stop" )
                {
                    Console.WriteLine( "Bye" );
                    return;
                }

                Console.WriteLine( "Enter student name" );
                var studentName = Console.ReadLine();

                Student myStudent = group.GetByName( studentName );
                if ( myStudent != null )
                {
                    Console.WriteLine( $"Средняя оценка студента \n" +
                        $"{myStudent}: {myStudent.GetAverageScore()}" );
                }
                else
                {
                    Console.WriteLine( "Такого студента не существует" );
                }
            }
        }
    }
}
