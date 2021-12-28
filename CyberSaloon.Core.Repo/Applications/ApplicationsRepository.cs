using System;
using System.Collections.Generic;
using System.Linq;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Common;
using Microsoft.EntityFrameworkCore;

namespace CyberSaloon.Core.Repo.Applications
{
    public class ApplicationsRepository : IApplicationsRepository
    {
        private readonly CyberSaloonDBContext _context;
        public ApplicationsRepository(CyberSaloonDBContext context)
        {
            _context =
                context ?? 
                throw new ArgumentNullException(nameof(context));
        }

        public void Create(Application entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Exists(Guid id) => _context.Applications.Any(e => e.Id == id);

        public Application? Read(Guid id) => 
            _context
                .Applications
                .FirstOrDefault(e => e.Id == id);

        public IEnumerable<Application> ReadAll(IEnumerable<Guid> ids) => 
            _context
                .Applications
                .Where(e => ids.Contains(e.Id));

        public IEnumerable<Application> ReadAll() => 
            _context
                .Set<Application>()
                .ToList();

        public Application Update(Application application)
        {
            _context.Update(application);
            _context.SaveChanges();
            return application;
        }

        public void Delete(Guid id)
        {
            var application = Read(id);
            _context.Remove(application);
            _context.SaveChanges();
        }

        public void Apply(Application application, Applicant applicant)
        {
            application
                .Supporters
                .Add(applicant);
            
            _context.Update(application);
            _context.SaveChanges();
        }

        public void Defy(Application application, Applicant applicant)
        {
            application
                .Supporters
                .Remove(applicant);

            _context.Update(application);
            _context.SaveChanges();
        }
    }
}