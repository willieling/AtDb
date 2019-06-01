using TinyJSON;

namespace AtDb.Reader
{
    public class DatabaseExporterConfiguration
    {
        public string serializationFunction(object model)
        {
            string json = JSON.Dump(model, Constants.ENCODE_OPTIONS);
            return json;
        }

        public string compressionFunction(string data)
        {
            string base64 = EncoderUtilities.Base64Encode(data);
            return base64;
        }
    }
}