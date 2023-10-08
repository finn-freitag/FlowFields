using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowFields
{
    public interface VectorBase
    {
        Vector GetVector();
        AngleVector GetAngleVector();
    }
}
