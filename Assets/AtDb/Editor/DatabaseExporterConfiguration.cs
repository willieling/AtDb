using TinyJSON;

namespace AtDb.Reader
{
    /// <summary>
    /// Class to specify how objects are serialized and compressed.
    /// 
    /// Modify function bodies as necessary to support your serializer and project needs.
    /// </summary>
    public class DatabaseExporterConfiguration
    {
        public string SerializationFunction(object model)
        {
            string json = JSON.Dump(model, Constants.ENCODE_OPTIONS);
            return json;
        }

        public string CompressionFunction(string data)
        {
            string base64 = EncoderUtilities.Base64Encode(data);
            return base64;
        }
    }
}