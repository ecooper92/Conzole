using System.Collections.Generic;

namespace Conzole
{
    /// <summary>
    /// A command line parameter that came after a switch value, the switch determines the function of the parameter and the parameter can have multiple values if multiple switches were used.
    /// </summary>
    public class SwitchedParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SwitchedParameter(string switchName, IEnumerable<string> values)
        {
            Switch = switchName;
            Values = values;
        }

        /// <summary>
        /// The string value of the switch that came before this value (does not include the starting character -, /, etc...).
        /// </summary>
        public string Switch { get; } = string.Empty;

        /// <summary>
        /// The string values of the parameter.
        /// </summary>
        public IEnumerable<string> Values { get; }
    }
}