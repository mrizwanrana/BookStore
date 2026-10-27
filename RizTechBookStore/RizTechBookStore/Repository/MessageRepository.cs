using Microsoft.Extensions.Options;
using RizTechBookStore.Models;

namespace RizTechBookStore.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NewBookAlertConfig _newBookAlertConfig;
        public MessageRepository(IOptionsMonitor<NewBookAlertConfig> newBookAlertCongig)
        {
            _newBookAlertConfig = newBookAlertCongig.CurrentValue;
        }
        public string GetName()
        {
            return _newBookAlertConfig.NewBookName;
        }
    }
}
