using System;
using System.Collections.Generic;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Common;
using Microsoft.Extensions.Logging;

namespace CyberSaloon.Core.BLL.Applications
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly IApplicationsRepository _repository;
        private readonly ILogger<ApplicationsService> _logger;
        public ApplicationsService(
                IApplicationsRepository repository,
                ILoggerFactory factory
            )
        {
            _repository =
                repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _logger =
                factory?.CreateLogger<ApplicationsService>() ??
                throw new ArgumentNullException(nameof(factory));
        }

        public void Create(Application entity)
        {
            using(_logger.BeginScope("Create"))
            {
                _logger.LogInformation("Creating application");
                _repository.Create(entity);
            }
        }

        public Application? Read(Guid id)
        {
            using(_logger.BeginScope("Read"))
            {
                _logger.LogInformation("Reading application");
                return _repository.Read(id);
            }
        }

        public IEnumerable<Application> ReadAll()
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading applications");
                return _repository.ReadAll();
            }
        }

        public IEnumerable<Application> ReadAll(IEnumerable<Guid> ids)
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading applications");
                return _repository.ReadAll(ids);
            }
        }

        public void Update(Application entity)
        {
            using(_logger.BeginScope("Update"))
            {
                _logger.LogInformation("Updating application");
                _repository.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            using(_logger.BeginScope("Delete"))
            {
                _logger.LogInformation("Deleting application");
                _repository.Delete(id);
            }
        }

        public void Apply(Application application, Applicant applicant)
        {
            using(_logger.BeginScope("Apply"))
            {
                _logger.LogInformation("Applying applicant to application");
                _repository.Apply(application, applicant);
            }
        }

        public void Defy(Application application, Applicant applicant)
        {
            using(_logger.BeginScope("Defy"))
            {
                _logger.LogInformation("Defying applicant from application");
                _repository.Defy(application, applicant);
            }
        }
    }
}