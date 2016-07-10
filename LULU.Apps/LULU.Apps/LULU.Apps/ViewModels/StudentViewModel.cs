using System;
using System.Collections.Generic;
using System.Text;
using LULU.Model;
using LULU.Model.Common;

namespace LULU.Apps.ViewModels
{

    class StudentViewModel
    {
#if __ANDROID__
        private Droid.LULUService.StudentServiceEndpoint _client;
#elif __IOS__
        private iOS.LULUService.StudentServiceEndpoint _client;
#endif
        public StudentViewModel()
        {
#if __ANDROID__
            _client = new Droid.LULUService.StudentServiceEndpoint();
#elif __IOS__
            _client = new iOS.LULUService.StudentServiceEndpoint();
#endif
        }

        /// <summary>
        /// Method to return Student object if studentNumber is valid.
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <returns>Student object if studentNumber is valid, null otherwise.</returns>
        public Student GetStudentByStudentNumber(string studentNumber)
        {
            try
            {
                Student student = null;
                student = Serializers<Student>.Deserialize(_client.SearchStudentByStudentNumber(studentNumber));
                return student;
            }
            catch (Exception ex)
            {
                //TODO add log4net and 
            }
            return null;
        }

        /// <summary>
        /// Method to log studednt in to the system
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <param name="password"></param>
        /// <returns>True if studentNumber and password are valid, false otherwise.</returns>
        public bool StudentLogin(string studentNumber, string password)
        {
            try
            {
                bool result = false;
                bool status = false;
                _client.LoginStudent(studentNumber, password, out result, out status);
                return result;
            }
            catch (Exception ex)
            {
                //TODO add log4net and 
            }
            return false;
        }

        /// <summary>
        /// Method to change Student Password.
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>
        /// If an error occurs connecting to the service, or an undetermined error occurs, returns -2.
        /// If Student specified by studentNumber arg cannot be found, returns -1.
        /// If Student is found, old password is valid and new password is valid, returns 0.
        /// if old password does not match the stored password, returns 1.
        /// </returns>
        public int ChangePassword(string studentNumber, string oldPassword, string newPassword)
        {
            try
            {
                Student student = GetStudentByStudentNumber(studentNumber);

                if (student == null) return -1;

                if (oldPassword != student.Password) return 1;

                bool success = false;
                bool status = false;

                _client.UpdateStudent(student.StudentNumber, student.FirstName, student.Surname, student.Email, newPassword, out success, out status);

                if (success) return 0;
            }
            catch (Exception ex)
            {
                //TODO add log4net and 
            }
            return -2;
        }

        public bool StudentAttendedClass(string studentNumber, int classID, decimal longitude, decimal latitiude)
        {
            bool result = false;
            bool status = false;

            try
            {
                GPS_Login login = new GPS_Login()
                {
                    LoginDateAndTime = DateTime.Now.Date,
                    Longitude = longitude,
                    Latitude = latitiude,
                };

                var loginStr = Serializers<GPS_Login>.Serialize(login);

                _client.StudentAttendedClass(studentNumber, classID, true, loginStr, out result, out status);
            }
            catch (Exception ex)
            {
                //TODO add log4net and 
            }
            return result;
        }
    }
}
