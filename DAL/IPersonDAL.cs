using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleBuilder.DAL
{
    public interface IPersonDAL
    {
        List<Person> GetDesiredPersons(string whereClause);
    }
}
