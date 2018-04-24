using Xamarin.Forms;

namespace DemoGAB2018CDMX
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
			MainPage = new NavigationPage(new Paginas.PaginaListaTareas());
		}
	}
}
