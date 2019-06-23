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

        int AddPerson(string lastName
            , string firstName
            , DateTime dateOfBirth
            , string ssn
            , string gender
            , string phone
            , string streetAddress
            , string zipcode
            , string username
            , string email);
        
    }
}
