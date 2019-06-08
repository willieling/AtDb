namespace AtDb.Reader.Container
{
    public class ModelDataContainer
    {
        public readonly object data;
        public readonly TableMetadata metadata;

        public ModelDataContainer(object data, TableMetadata metadata)
        {
            this.data = data;
            this.metadata = metadata;
        }
    }
}