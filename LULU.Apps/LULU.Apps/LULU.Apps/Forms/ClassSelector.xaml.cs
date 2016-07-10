using Xamarin.Forms;
using Xamarin.Forms.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassSelector : ContentPage
    {
        private Map map;
        public ClassSelector()
        {
            map = new Map(MapSpan.FromCenterAndRadius(new Position(54.583551, -5.9335411), Distance.FromMiles(0.5)));

            Content = new StackLayout
            {
                Children = { map }
            };
        }
    }
}
