using System.Collections.Generic;

namespace Conzole
{
    /// <summary>
    /// The collection of ordered and switched parameters read from the command line at application start.
    /// </summary>
    public class CommandLineParameters
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandLineParameters(IEnumerable<OrderedParameter> orderedParameters, IEnumerable<SwitchedParameter> switchedParameters)
        {
            OrderedParameters = orderedParameters;
            SwitchedParameters = switchedParameters;
        }

        /// <summary>
        /// The parameters that occured without switches and are order dependent.
        /// </summary>
        public IEnumerable<OrderedParameter> OrderedParameters { get; }

        /// <summary>
        /// The parameters that occured with switches, this parameters can have multiple values.
        /// </summary>
        public IEnumerable<SwitchedParameter> SwitchedParameters { get; }
    }
}