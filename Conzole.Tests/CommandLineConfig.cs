using System.Collections.Generic;

namespace Conzole
{
    public class CommandLineConfig
    {
        [OrderedParameter(0)]
        public int OrderedInt { get; set; }

        [OrderedParameter(1)]
        public bool OrderedBool { get; set; }
        
        [OrderedParameter(2)]
        public double OrderedDouble { get; set; }
        
        [OrderedParameter(3)]
        public string OrderedString { get; set; }

        [SwitchedParameter("int")]
        public int SwitchedInt { get; set; }

        [SwitchedParameter("bool")]
        public bool SwitchedBool { get; set; }
        
        [SwitchedParameter("double")]
        public double SwitchedDouble { get; set; }
        
        [SwitchedParameter("string")]
        public string SwitchedString { get; set; }
        
        [OrderedParameter(4)]
        public int[] OrderedIntArray { get; set; }
        
        [OrderedParameter(5)]
        public int[] OrderedBoolArray { get; set; }
        
        [OrderedParameter(6)]
        public double[] OrderedDoubleArray { get; set; }
        
        [OrderedParameter(7)]
        public string[] OrderedStringArray { get; set; }
        
        [SwitchedParameter("i")]
        public int[] SwitchedIntArray { get; set; }
        
        [SwitchedParameter("i")]
        public bool[] SwitchedBoolArray { get; set; }
        
        [SwitchedParameter("d")]
        public double[] SwitchedDoubleArray { get; set; }
        
        [SwitchedParameter("s")]
        public string[] SwitchedStringArray { get; set; }
    }
}