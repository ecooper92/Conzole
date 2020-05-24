using System.Collections.Generic;

namespace Conzole
{
    public class CommandLineConfig
    {
        [OrderedParameter(0)]
        public int Param1 { get; set; }
        
        [OrderedParameter(1)]
        public double Param2 { get; set; }
        
        [OrderedParameter(2)]
        public string Param3 { get; set; }

        [SwitchedParameter("a")]
        public int Param4 { get; set; }
        
        [SwitchedParameter("b")]
        public double Param5 { get; set; }
        
        [SwitchedParameter("c")]
        public string Param6 { get; set; }
        
        [OrderedParameter(3)]
        public int[] Params1 { get; set; }
        
        [OrderedParameter(4)]
        public double[] Params2 { get; set; }
        
        [OrderedParameter(5)]
        public string[] Params3 { get; set; }
        
        [SwitchedParameter("d")]
        public int[] Params4 { get; set; }
        
        [SwitchedParameter("e")]
        public double[] Params5 { get; set; }
        
        [SwitchedParameter("f")]
        public string[] Params6 { get; set; }
    }
}