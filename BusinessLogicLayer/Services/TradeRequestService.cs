using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class TradeRequestService
    {
        private readonly ITradeRequestRepository _tradeRequestRepository;

        public TradeRequestService(ITradeRequestRepository tradeRequestRepository)
        {
            _tradeRequestRepository = tradeRequestRepository;
        }







    }
}
