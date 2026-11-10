namespace Application.Auth
{
    public interface ICurrentUser
    {
        public int UserId { get;}
        public string Email { get;}
        public string Name { get;}
        public string PhoneNumber { get;}
        public int RoleId { get;}
        public string RoleName { get;}
        public int TenantId { get;}
    }
}
