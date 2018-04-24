using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.MediaManager;
using DemoGAB2018CDMX.Servicios;
using DemoGAB2018CDMX.Modelos;

namespace DemoGAB2018CDMX.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaDetalleTarea : ContentPage
	{
        Tarea tarea;
        bool estaGrabando = false;

        public PaginaDetalleTarea(Tarea tarea)
        {
            InitializeComponent();

            this.tarea = tarea;

            if (tarea.Id <= 0 && ToolbarItems.Count > 1)
                ToolbarItems.RemoveAt(1);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            BindingContext = tarea;
            Loading(false);
        }

        async void btnTraducirTarea_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTarea.Text))
            {
                try
                {
                    Loading(true);

                    var texto = await ServicioText.TraducirTexto(txtTarea.Text);
                    txtTask.Text = texto;

                    Loading(false);
                }
                catch (Exception ex)
                {
                }
            }
            else
                await DisplayAlert("Error", "The task can't be an empty string", "OK");
        }

        async void btnListenTask_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTask.Text))
            {
                try
                {
                    Loading(true);

                    var servicioAudioRecorder = DependencyService.Get<IServicioAudioRecorder>();

                    var datos = await ServicioSpeech.ConvertirTexto_Voz(txtTask.Text);
                    var archivo = servicioAudioRecorder.WriteAudioDataToFile(datos);
                    await CrossMediaManager.Current.Play(archivo);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Loading(false);
                }
            }
            else
                await DisplayAlert("Error", "The task can't be an empty string", "OK");
        }

        async void btnDictarTarea_Clicked(object sender, EventArgs e)
        {
            try
            {
                var servicioAudioRecorder = DependencyService.Get<IServicioAudioRecorder>();

                if (!estaGrabando)
                {
                    servicioAudioRecorder.IniciarGrabacion();
                    btnDictarTarea.Text = "Finalizar";
                    Loading(true);
                }
                else
                {
                    servicioAudioRecorder.DetenerGrabacion();
                }

                estaGrabando = !estaGrabando;

                if (!estaGrabando)
                {
                    var texto = await ServicioSpeech.ReconocerVozTexto();
                    txtTarea.Text = texto;
                }
            }
            catch (Exception ex)
            {
                txtTarea.Text = "Error durante la grabación";
            }
            finally
            {
                if (!estaGrabando)
                {
                    btnDictarTarea.Text = "Dictar Tarea";
                    Loading(false);
                }
            }
        }

        async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            var bd = new ServicioBaseDatos();
            bool op = (tarea.Id > 0)
                ? await bd.ActualizarTarea(tarea)
                : await bd.AgregarTarea(tarea);

            if (op)
            {
                await DisplayAlert("Éxito", "Operación realizada con éxito", "OK");
                await Navigation.PopAsync(true);
            }
            else
                await DisplayAlert("Error", "Tarea no registrada", "OK");
        }

        async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            if (tarea.Id > 0)
            {
                var confirmar = await DisplayAlert("¿Eliminar Tarea?", "¿Estás seguro de eliminar esta tarea?", "Sí", "No");

                if (confirmar)
                {

                    var bd = new ServicioBaseDatos();
                    bool op = await bd.EliminarTarea(tarea.Id);

                    if (op)
                    {
                        await DisplayAlert("Éxito", "Tarea eliminada con éxito", "OK");
                        await Navigation.PopAsync(true);
                    }
                    else
                        await DisplayAlert("Error", "Tarea no eliminada", "OK");
                }
            }
            else
                await DisplayAlert("Error", "Selecciona una tarea para eliminar", "OK");
        }
    }
}