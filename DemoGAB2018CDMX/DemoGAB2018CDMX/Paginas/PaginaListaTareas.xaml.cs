using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DemoGAB2018CDMX.Servicios;
using DemoGAB2018CDMX.Modelos;

namespace DemoGAB2018CDMX.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaListaTareas : ContentPage
	{
        public PaginaListaTareas ()
		{
			InitializeComponent ();
		}

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        protected async override void OnAppearing()
        {
            Loading(true);

            base.OnAppearing();
            var bd = new ServicioBaseDatos();

            lsvTareas.ItemsSource = await bd.ObtenerTareas();

            Loading(false);
        }

        private async void lsvTareas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = (Tarea)e.SelectedItem;
                await Navigation.PushAsync(new PaginaDetalleTarea(item));
            }
            catch (Exception ex)
            {
            }
        }

        private async void btnAgregar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaDetalleTarea(new Tarea()));
        }

        private async void btnRating_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaRating());
        }
    }
}