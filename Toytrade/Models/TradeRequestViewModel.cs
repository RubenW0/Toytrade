using Org.BouncyCastle.Asn1.Mozilla;
using PresentationLayer.Models;


namespace PresentationLayer.Models
{
    public class TradeRequestViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string RequesterUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Username { get; set; } 

        public List<ToyViewModel> OfferedToys { get; set; }
        public List<ToyViewModel> RequestedToys { get; set; }
  
    }
}