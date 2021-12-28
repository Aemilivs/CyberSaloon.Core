using System;
using System.Collections.Generic;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Common;
using Microsoft.Extensions.Logging;

namespace CyberSaloon.Core.BLL.Applicants
{
    public class ApplicantsService : IApplicantsService
    {
        private readonly IApplicantsRepository _repository;
        private readonly ILogger<ApplicantsService> _logger;
        public ApplicantsService(
                IApplicantsRepository repository,
                ILoggerFactory factory
            )
        {
            _repository =
                repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _logger =
                factory?.CreateLogger<ApplicantsService>() ??
                throw new ArgumentNullException(nameof(factory));
        }

        public void Create(Applicant entity)
        {
            using(_logger.BeginScope("Create"))
            {
                _logger.LogInformation("Creating Applicant");
                _repository.Create(entity);
            }
        }

        public Applicant? Read(Guid id)
        {
            using(_logger.BeginScope("Read"))
            {
                _logger.LogInformation("Reading Applicant");
                return _repository.Read(id);
            }
        }
        
        public IEnumerable<Applicant> ReadAll()
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading Applicants");
                return _repository.ReadAll();
            }
        }

        public IEnumerable<Applicant> ReadAll(IEnumerable<Guid> ids)
        {
            using(_logger.BeginScope("ReadAll"))
            {
                _logger.LogInformation("Reading Applicants");
                return _repository.ReadAll(ids);
            }
        }

        public void Update(Applicant entity)
        {
            using(_logger.BeginScope("Update"))
            {
                _logger.LogInformation("Updating Applicant");
                _repository.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            using(_logger.BeginScope("Delete"))
            {
                _logger.LogInformation("Deleting Applicant");
                _repository.Delete(id);
            }
        }

        public Applicant? ReadByUserId(Guid userId)
        {
            _logger.LogInformation("Reading Applicant by UserId");
            return _repository.ReadByUserId(userId);
        }

        public Applicant? ReadByAlias(string alias)
        {
            _logger.LogInformation("Reading Applicant by Alias");
            return _repository.ReadByAlias(alias);
        }
    }
}