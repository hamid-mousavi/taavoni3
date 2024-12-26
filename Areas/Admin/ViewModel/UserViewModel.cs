public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }

    public string DisplayName
    {
        get
        {
            return $"{UserName} - {Name}";
        }
    }
}
