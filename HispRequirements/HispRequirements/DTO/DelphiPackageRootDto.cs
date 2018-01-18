using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispRequirements.DTO
{
    public class DelphiPackageRootDto : BaseRegDto
    {
        public List<DelphiPackageDto> Packages { get; set; }
    }
}
