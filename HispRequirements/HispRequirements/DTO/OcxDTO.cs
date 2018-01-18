using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispRequirements.DTO
{
    public class OcxDto
    {
        public string Classe { get; set; }
        public string Arquivo { get; set; }
        public List<string> Modulos { get; set; }
    }
}
