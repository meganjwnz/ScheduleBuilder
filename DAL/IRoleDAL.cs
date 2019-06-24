using ScheduleBuilder.Model;
using System.Collections.Generic;

namespace ScheduleBuilder.DAL
{
    public interface IRoleDAL
    {
        List<Role> GetRoles();

        string GetRoleByID(int id);

        int GetRoleIdByTitle(string roleTitle);
    }
}
