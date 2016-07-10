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
#elif __IOS__
        private iOS.LULUService.ClassServiceEndpoint _client;
#endif

        public ClassViewModel()
        {
#if __ANDROID__
            _client = new Droid.LULUService.ClassServiceEndpoint();
#elif __IOS__
            _client = new iOS.LULUService.ClassServiceEndpoint();
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
    }
}
