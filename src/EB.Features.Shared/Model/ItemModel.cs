namespace EB.Features.Shared.Model
{
    public class ItemModel
    {
        public ItemModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }
    }
}
