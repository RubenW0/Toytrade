using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOs.Enums;

namespace BusinessLogicLayer.DTOs
{
    public class TradeRequestDTO
    {
        public int Id { get; set; }
        public TradeRequestStatus Status { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }

        public string RequesterUsername { get; set; }
        public string ReceiverUsername { get; set; }

        public List<ToyDTO> OfferedToys { get; set; } = new List<ToyDTO>();
        public List<ToyDTO> RequestedToys { get; set; } = new List<ToyDTO>();
    }

}