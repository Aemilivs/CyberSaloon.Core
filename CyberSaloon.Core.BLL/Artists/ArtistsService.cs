using System;
using System.Collections.Generic;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.Repo.Common;
using Microsoft.Extensions.Logging;

namespace CyberSaloon.Core.BLL.Artists
{
    public class ArtistsService : IArtistsService
    {
        private readonly IArtistsRepository _repository;
        private readonly ILogger<ArtistsService> _logger;
        public ArtistsService(
                IArtistsRepository repository,
                ILoggerFactory factory
            )
        {
            _repository =
                repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _logger =
                factory?.CreateLogger<ArtistsService>() ??
                throw new ArgumentNullException(nameof(factory));
        }

        public void Create(Artist entity)
        {
            using(_logger.BeginScope("Create"))
            {
                _logger.LogInformation("Creating artist");
                _repository.Create(entity);
            }
        }

        public Artist? Read(Guid id)
        {
            using(_logger.BeginScope("Read"))
            {
                _logger.LogInformation("Reading artist");
                return _repository.Read(id);
            }
        }

        public IEnumerable<Artist> ReadAll()
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading artists");
                return _repository.ReadAll();
            }
        }

        public IEnumerable<Artist> ReadAll(IEnumerable<Guid> ids)
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading artists");
                return _repository.ReadAll(ids);
            }
        }

        public void Update(Artist entity)
        {
            using(_logger.BeginScope("Update"))
            {
                _logger.LogInformation("Updating artist");
                _repository.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            using(_logger.BeginScope("Delete"))
            {
                _logger.LogInformation("Deleting artist");
                _repository.Delete(id);
            }
        }

        public Artist? ReadByUserId(Guid userId)
        {
            _logger.LogInformation("Reading Applicant by UserId");
            return _repository.ReadByUserId(userId);
        }

        public Artist? ReadByAlias(string alias)
        {
            _logger.LogInformation("Reading Artist by Alias");
            return _repository.ReadByAlias(alias);
        }
    }
}