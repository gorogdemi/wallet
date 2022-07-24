using Wallet.Contracts.Requests;

namespace Wallet.UI.Models
{
    public class CategoryDialog
    {
        public long? Id { get; set; }

        public CategoryRequest Request { get; set; }

        public bool ShowDialog { get; set; }
    }
}