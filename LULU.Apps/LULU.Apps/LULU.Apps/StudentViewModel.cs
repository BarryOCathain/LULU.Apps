using System;
using System.Collections.Generic;
using System.Text;
using LULU.Model;
using LULU.Model.Common;

namespace LULU.Apps
{

    class StudentViewModel
    {
        private Droid.LULUService.StudentServiceEndpoint _client;

        public StudentViewModel()
        {
            _client = new Droid.LULUService.StudentServiceEndpoint();
        }

        public Student GetStudentByStudentNumber(string studentNumber)
        {
            Student student = null;
            student = Serializers<Student>.Deserialize(_client.SearchStudentByStudentNumber(studentNumber));
            return student;
        }
    }
}
