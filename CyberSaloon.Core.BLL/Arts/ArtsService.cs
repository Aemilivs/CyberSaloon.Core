using System;
using System.Collections.Generic;
using CyberSaloon.Core.Repo.Arts;
using CyberSaloon.Core.Repo.Common;
using Microsoft.Extensions.Logging;

namespace CyberSaloon.Core.BLL.Arts
{
    public class ArtsService : IArtsService
    {
        private readonly IArtsRepository _repository;
        private readonly ILogger<ArtsService> _logger;
        public ArtsService(
                IArtsRepository repository,
                ILoggerFactory factory
            )
        {
            _repository =
                repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _logger =
                factory?.CreateLogger<ArtsService>() ??
                throw new ArgumentNullException(nameof(factory));
        }

        public void Create(Art entity)
        {
            using(_logger.BeginScope("Create"))
            {
                _logger.LogInformation("Creating art");
                _repository.Create(entity);
            }
        }

        public Art? Read(Guid id)
        {
            using(_logger.BeginScope("Read"))
            {
                _logger.LogInformation("Reading art");
                return _repository.Read(id);
            }
        }

        public IEnumerable<Art> ReadAll()
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading arts");
                return _repository.ReadAll();
            }
        }

        public IEnumerable<Art> ReadAll(IEnumerable<Guid> ids)
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading arts");
                return _repository.ReadAll(ids);
            }
        }

        public void Update(Art entity)
        {
            using(_logger.BeginScope("Update"))
            {
                _logger.LogInformation("Updating art");
                _repository.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            using(_logger.BeginScope("Delete"))
            {
                _logger.LogInformation("Deleting art");
                _repository.Delete(id);
            }
        }
    }
}