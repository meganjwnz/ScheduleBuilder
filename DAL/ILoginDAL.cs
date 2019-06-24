using System.Data;

namespace ScheduleBuilder.DAL
{
    public interface ILoginDAL
    {
        DataTable GetLogin(string username, string password);
    }
}
