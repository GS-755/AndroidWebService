namespace AndroidWebService.Models.Nodes
{
    public class TransactionNode
    {
        public string UserName { get; set; }
        public string TransactionId { get; set; }
        
        public TransactionNode() 
        {
            this.UserName = string.Empty;
            this.TransactionId = string.Empty;
        }
        public TransactionNode(string TransactionId)
        {
            this.UserName = string.Empty;
            this.TransactionId = TransactionId;
        }
    }
}
