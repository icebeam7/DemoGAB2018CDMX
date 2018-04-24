namespace DemoGAB2018CDMX.Helpers
{
    public static class Constantes
    {
        public static readonly string SpeechServiceApiKey = "tu-llave-speech";
        public static readonly string FetchTokenURL = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        public static readonly string SpeechServiceURL = "https://speech.platform.bing.com/speech/recognition/dictation/cognitiveservices/v1?language=es-MX&format=simple";
        public static readonly string TextToSpeechServiceURL = "https://speech.platform.bing.com/synthesize";
        public static readonly string AudioContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

        public static readonly string TextServiceApiKey = "tu-llave-text";
        public static readonly string TextServiceURL = "https://api.microsofttranslator.com/v2/http.svc/translate?from=es&to=en&text=";

        public static readonly string SsmlXmlContentType = "application/ssml+xml";
        public static readonly string AudioFile = "audio.wav";
        public static readonly string Locale = "en-US";
        public static readonly string Gender = "Female";
        public static readonly string VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
        public static readonly string OutputFormat = "riff-16khz-16bit-mono-pcm";
        public static readonly string AppId = "07D3234E49CE426DAA29772419F436CA";
        public static readonly string ClientId = "1ECFAE91408841A480F00935DC390960";
        public static readonly string ApplicationName = "DemoGAB2018CDMX.Android";
        public static readonly string Version = "1.0";

        public static readonly string FaceApiKey = "tu-llave-face";
        public static readonly string FaceApiURL = "https://aqui-va-el-endpoint.api.cognitive.microsoft.com/face/v1.0/";

        public static readonly string NombreBD = "BaseDatosTareas.db";
    }
}
