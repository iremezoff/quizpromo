using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGSK.K3.Infrastructure.EF.K3Conventions
{
    class DateTimeOffsetConvention : Convention
    {
        public DateTimeOffsetConvention()
        {
            Properties<DateTimeOffset>().Configure(c => c.HasColumnType("datetimeoffset"));
        }
    }
}
