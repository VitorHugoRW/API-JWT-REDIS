namespace API_JWT.Application.Domain
{
    public class User
    {
        public string Name { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        protected User() { }
        public User(string username,string password,string name)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Name = name;
        }
    }
}
