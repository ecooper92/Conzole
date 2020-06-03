using System;

namespace Conzole
{
    /// <summary>
    /// Indicates this property should get its value from a switched command line parameter.
    /// </summary>
    public class SwitchedParameterAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SwitchedParameterAttribute(string switchName)
        {
            Switch = switchName;
        }

        /// <summary>
        /// The string value of the switch that came before this value (does not include the starting character -, /, etc...).
        /// </summary>
        public string Switch { get; } = string.Empty;
    }
}