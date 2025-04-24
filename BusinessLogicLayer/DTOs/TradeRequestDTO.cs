using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class TradeRequestDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int UserIdRequester { get; set; }
        public int UserIdReceiver { get; set; }

        public List<int> OfferedToyIds { get; set; }
        public List<int> RequestedToyIds { get; set; }
    }
}