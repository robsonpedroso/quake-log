using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLog.Domain.Contracts.Services
{
    public interface IGameServices
    {
        List<DTO.Games> GetAll();
        List<DTO.Games> GetWithoutWorld(int? idGame = null);
    }
}
