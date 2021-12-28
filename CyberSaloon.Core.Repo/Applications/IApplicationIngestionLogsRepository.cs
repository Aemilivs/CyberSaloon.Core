using System;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.BLL.Applications
{
    public interface IApplicationIngestionLogsRepository : IRepository<ApplicationIngestLog>, IDisposable
    {
         
    }
}