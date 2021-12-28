using System;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.Repo.Artists
{
    public interface IArtistsRepository : IRepository<Artist>
    {
        Artist? ReadByUserId(Guid userId);
        Artist? ReadByAlias(string alias);
        bool Exists(string alias);
    }
}