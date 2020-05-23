namespace Conzole
{
    /// <summary>
    /// A command line parameter whose order in the sequence determines its function.
    /// </summary>
    public class OrderedParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderedParameter(int order, string value)
        {
            Order = order;
            Value = value;
        }

        /// <summary>
        /// The zero-based order in the sequence the parameter occured.
        /// </summary>
        public int Order { get; } = int.MinValue;

        /// <summary>
        /// The string value of the parameter.
        /// </summary>
        public string Value { get; } = string.Empty;
    }
}