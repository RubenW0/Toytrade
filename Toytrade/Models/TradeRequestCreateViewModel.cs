using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class TradeRequestCreateViewModel
    {
        public int ReceiverId { get; set; }

        public List<int> OfferedToyIds { get; set; } = new List<int>();
        public List<int> RequestedToyIds { get; set; } = new List<int>();

        public List<ToyViewModel> MyToys { get; set; } = new List<ToyViewModel>();

        public List<ToyViewModel> ReceiverToys { get; set; } = new List<ToyViewModel>();

        public List<UserViewModel> AllUsers { get; set; } = new List<UserViewModel>();
    }
}