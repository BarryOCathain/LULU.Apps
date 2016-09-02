using LULU.Apps.ViewModels;
using LULU.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassesView : ContentPage
    {
        private DatePicker startDate, endDate;
        private ListView classList;
        private Button submitButton;
        private string student;
        private ClassViewModel viewModel;

        public ClassesView(string studentNumber, bool isShowingMissedClasses = false)
        {
            student = studentNumber;
            viewModel = new ClassViewModel();

            if (isShowingMissedClasses)
            {
                Title = "Missed Classes";
            }
            else
            {
                Title = "Attended Classes";
            }

            Content = LoadControls();
            classList.ItemsSource = GetClasses(student, isShowingMissedClasses);
        }

        private List<Class> GetClasses(string user, bool isShowingMissedClasses)
        {
            List<Class> classes = new List<Class>();

            try
            {
                if (isShowingMissedClasses)
                    classes = viewModel.GetClassesMissedInDateRange(student, startDate.Date, endDate.Date.AddDays(1));
                else
                    classes = viewModel.GetClassesAttendedInDateRange(student, startDate.Date, endDate.Date.AddDays(1));

                return classes.OrderBy(c => c.ClassDate).OrderBy(c => c.StartTime).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        private StackLayout LoadControls()
        {
            startDate = new DatePicker()
            {
                Date = DateTime.Now.AddDays(-7)
            };

            endDate = new DatePicker()
            {
                Date = DateTime.Now
            };

            classList = new ListView()
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Label className = new Label();
                    className.SetBinding(Label.TextProperty, "Name");
                };

                return new ViewCell
                {
                    wiew = new StackLayout
                    {
                        
                    }
                }
            };

            submitButton = new Button()
            {
                Text = "Get Classes",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };

            return new StackLayout()
            {
                Children = { startDate, endDate, classList, submitButton }
            };
        }
    }
}
