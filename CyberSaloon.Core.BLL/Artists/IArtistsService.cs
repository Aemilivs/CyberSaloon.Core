using System;
using CyberSaloon.Core.BLL.Common;
using CyberSaloon.Core.Repo.Artists;

namespace CyberSaloon.Core.BLL.Artists
{
    public interface IArtistsService : IService<Artist>
    {
        Artist? ReadByUserId(Guid userId);
        Artist? ReadByAlias(string alias);
    }
}