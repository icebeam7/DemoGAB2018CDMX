using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using DemoGAB2018CDMX.Servicios;
using DemoGAB2018CDMX.Modelos;

namespace DemoGAB2018CDMX.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaRating : ContentPage
	{
        Emocion emocion;

		public PaginaRating ()
		{
			InitializeComponent ();

            CargarEmocion();
        }

        async void CargarEmocion()
        {
            Loading(true);

            var bd = new ServicioBaseDatos();
            emocion = await bd.ObtenerEmocion();

            if (emocion == null)
            {
                emocion = new Emocion() { Id = 0, Nombre = "", Score = 0, Foto = "" };
                ToolbarItems.RemoveAt(1);
            }
            else
                btnAnalizar.IsEnabled = true;

            BindingContext = emocion;

            Loading(false);
        }

        MediaFile foto;

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            foto = await ServicioImagen.TomarFoto();

            if (foto != null)
            {
                imagen.Source = ImageSource.FromStream(foto.GetStream);
                btnAnalizar.IsEnabled = true;
            }
            else
                await DisplayAlert("Error", "No se pudo tomar la fotografía.", "OK");
        }

        async void btnAnalizar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Loading(true);
                emocion = await ServicioFace.ObtenerEmocion(foto);
                lblEmocion.Text = emocion.Resultado;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Loading(false);
            }
        }

        async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (emocion.Score > 0)
            {
                var bd = new ServicioBaseDatos();
                bool op = (emocion.Id > 0)
                    ? await bd.ActualizarEmocion(emocion)
                    : await bd.AgregarEmocion(emocion);

                if (op)
                {
                    await DisplayAlert("Éxito", "Operación realizada con éxito", "OK");
                    await Navigation.PopAsync(true);
                }
                else
                    await DisplayAlert("Error", "Rating no registrado", "OK");
            }
            else
                await DisplayAlert("Error", "Debes analizar una foto primero.", "OK");
        }

        async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            if (emocion.Id > 0)
            {
                var confirmar = await DisplayAlert("¿Eliminar Rating?", "¿Estás seguro de eliminar tu rating?", "Sí", "No");

                if (confirmar)
                {
                    var bd = new ServicioBaseDatos();
                    bool op = await bd.EliminarEmocion();

                    if (op)
                    {
                        await DisplayAlert("Éxito", "Rating eliminado con éxito", "OK");
                        await Navigation.PopAsync(true);
                    }
                    else
                        await DisplayAlert("Error", "Rating no eliminado", "OK");
                }
            }
            else
                await DisplayAlert("Error", "No hay rating por eliminar", "OK");
        }
    }
}