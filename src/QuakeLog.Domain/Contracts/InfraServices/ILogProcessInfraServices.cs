using System.Collections.Generic;

namespace QuakeLog.Domain.Contracts.InfraServices
{
    public interface ILogProcessInfraServices
    {
        List<DTO.Games> Load();
    }
}
