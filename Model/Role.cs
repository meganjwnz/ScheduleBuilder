namespace ScheduleBuilder.Model
{
    /// <summary>
    /// The Role Model Class references the role a user will have, thus allowing them
    /// specific permissions to parts of the site
    /// </summary>
    public class Role
    {
        //Id
        public int Id { get; set; }

        //Title
        public string RoleTitle { get; set; }

        //Description 
        public string RoleDescription { get; set; }
    }
}