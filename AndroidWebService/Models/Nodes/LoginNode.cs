namespace AndroidWebService.Models.Utils.Nodes
{
    public class LoginNode
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginNode()
        {
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }
    }
}
