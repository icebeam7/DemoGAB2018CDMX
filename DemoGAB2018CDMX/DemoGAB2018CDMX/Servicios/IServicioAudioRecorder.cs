namespace DemoGAB2018CDMX.Servicios
{
    public interface IServicioAudioRecorder
    {
        void IniciarGrabacion();
        void DetenerGrabacion();
        string WriteAudioDataToFile(byte[] datos);
    }
}