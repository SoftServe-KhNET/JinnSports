using JinnSports.BLL.Dtos.ClientLog;

namespace JinnSports.BLL.Interfaces
{
    public interface ILoggerService
    {
        void SaveLog(LogDto logs);
    }
}
