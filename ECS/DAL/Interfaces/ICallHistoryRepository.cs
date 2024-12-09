using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECS.DAL.Interfaces
{
    public interface ICallHistoryRepository
    {
        Task UpdateCallHistoryAsync(int callId, int status, string notes);
    }
}