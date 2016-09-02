using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LULU.Model;
using LULU.Model.Common;

namespace LULU.Apps.ViewModels
{
    class ClassViewModel
    {
#if __ANDROID__
        private Droid.LULUService.ClassServiceEndpoint _client;
        private Droid.LULUService.ClassRoomServiceEndpoint _classroomClient;
        private Droid.LULUService.StudentServiceEndpoint _studentClient;
#elif __IOS__
        private iOS.LULUService.ClassServiceEndpoint _client;
        private iOS.LULUService.ClassRoomServiceEndpoint _classroomClient;
        private iOS.LULUService.StudentServiceEndpoint _studentClient;
#endif

        public ClassViewModel()
        {
#if __ANDROID__
            _client = new Droid.LULUService.ClassServiceEndpoint();
            _classroomClient = new Droid.LULUService.ClassRoomServiceEndpoint();
            _studentClient = new Droid.LULUService.StudentServiceEndpoint();
#elif __IOS__
            _client = new iOS.LULUService.ClassServiceEndpoint();
            _classroomClient = new iOS.LULUService.ClassRoomServiceEndpoint();
            _studentClient = new iOS.LULUService.StudentServiceEndpoint();
#endif
        }

        public List<Class> GetClassesAttendedInDateRange(string studentNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                var classes = _client.GetAttendedClassesByStudentNumberAndDateRange(studentNumber, startDate, true, endDate, true);
                return Serializers<Class>.DeserializeList(classes);
            }
            catch (Exception ex)
            {
                //TODO add logging
            }
            return null;
        }

        public List<Class> GetClassesMissedInDateRange(string studentNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                return Serializers<Class>.DeserializeList(_client.GetMissedClassesByStudentNumberAndDateRange(studentNumber, startDate, true, endDate, true));
            }
            catch (Exception ex)
            {
                //TODO add logging
            }
            return null;
        }

        public List<Class> GetClassesToday(string studentNumber)
        {

            try
            {
                var result = _client.GetClassesByStudentNumberAndDateRange(
                        studentNumber, DateTime.Now.Date, true, DateTime.Now.AddDays(1).AddSeconds(-1), true, false, true);

                var classes = Serializers<Class>.DeserializeList(result);

                return classes;
            }
            catch (Exception ex)
            {
                //TODO add logging
            }
            return null;
        }

        public ClassRoom GetClassRoomByClassID(int classID)
        {
            try
            {
                return Serializers<ClassRoom>.Deserialize(_classroomClient.GetClassRoomByClassID(classID, true));
            }
            catch (Exception ex)
            {
                //TODO add logging
            }
            return null;
        }

        public bool SignInToClass(string studentNumber, int classID, decimal longitude, decimal latitude)
        {
            var login = new GPS_Login()
            {
                GPS_LoginID = 0,
                Longitude = longitude,
                Latitude = latitude,
                LoginDateAndTime = DateTime.Now
            };
            bool result;
            bool specified;
            _studentClient.StudentAttendedClass(studentNumber, classID, true, Serializers<GPS_Login>.Serialize(login), out result, out specified);

            return result;
        }

        public ClassRoom GetClassRoom(int classroomID)
        {
            try
            {
                return Serializers<ClassRoom>.Deserialize(_classroomClient.GetClassRoomByID(classroomID, true));
            }
            catch (Exception ex)
            {
                //TODO add logging
            }
            return null;
        }
    }
}
