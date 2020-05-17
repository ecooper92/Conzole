namespace Conzole
{
    /// <summary>
    /// A value that has an additional key property.
    /// </summary>
    public class KeyedValue<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public KeyedValue(string key, T value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// The key of the nested menu item.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The value associated with the key.
        /// </summary>
        public T Value { get; }
    }
}