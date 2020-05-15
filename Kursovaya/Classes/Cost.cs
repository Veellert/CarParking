using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Cost
    {
        public int CostForCommon { get; set; }
        public int CostForStudent { get; set; }
        public int CostForPensioner { get; set; }
        public int CostForDisability { get; set; }
    }
}
