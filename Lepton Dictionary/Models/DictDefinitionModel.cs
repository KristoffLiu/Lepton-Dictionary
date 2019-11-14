using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepton_Dictionary.Models
{
    public class DictDefinitionModel
    {
        public string WordName { get; set; }
        public List<AiCiBaE2C_Exchange> Exchanges { get; set; }
        public string Pronounciation_British { get; set; }
        public string Pronounciation_American { get; set; }
        public string Pronounciation_British_MP3 { get; set; }
        public string Pronounciation_Amercian_MP3 { get; set; }

        public List<AiCiBaE2C_Part> Parts { get; set; }
    }

    //public class Exchange
    //{
    //    public string Formation { get; set; }
    //    public string Word { get; set; }
    //}
    
    //public class Symbol
    //{

    //}


}
