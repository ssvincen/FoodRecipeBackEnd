namespace Recipes.BO;

public class UserViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public DateTime? LastLogin { get; set; }
    public IEnumerable<RoleViewModel> Roles { get; set; }
}
