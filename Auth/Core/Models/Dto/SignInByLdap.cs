namespace Core.Models.Dto
{
    public class SignInByLdap
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}