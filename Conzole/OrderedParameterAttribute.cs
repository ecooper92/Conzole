using System;

namespace Conzole
{
    /// <summary>
    /// Indicates this property should get its value from an ordered command line parameter.
    /// </summary>
    public class OrderedParameterAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderedParameterAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderedParameterAttribute(int order, int count)
        {
            Order = order;
            Count = count;
        }

        /// <summary>
        /// The zero-based order in the sequence the parameter occured.
        /// </summary>
        public int Order { get; } = int.MinValue;

        /// <summary>
        /// The number of expected values for this property, if set to int.MaxValue all the remaining ordered values after this parameter will be read.
        /// </summary>
        public int Count { get; } = 1;
    }
}