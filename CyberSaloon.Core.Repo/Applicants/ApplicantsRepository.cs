using System;
using System.Collections.Generic;
using System.Linq;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Common;
using Microsoft.EntityFrameworkCore;

namespace CyberSaloon.Core.BLL.Applicants
{
    public class ApplicantsRepository : IApplicantsRepository
    {
        private readonly CyberSaloonDBContext _context;
        public ApplicantsRepository(CyberSaloonDBContext context)
        {
            _context =
                context ?? 
                throw new ArgumentNullException(nameof(context));
        }

        public void Create(Applicant entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Exists(Guid id) => _context.Applicants.Any(e => e.Id == id);

        public Applicant? Read(Guid id) => 
            _context
                .Applicants
                .Include(it => it.Applications)
                .FirstOrDefault(e => e.Id == id);

        public IEnumerable<Applicant> ReadAll(IEnumerable<Guid> ids) => 
            _context
                .Applicants
                .Include(it => it.Applications)
                .Where(e => ids.Contains(e.Id));

        public IEnumerable<Applicant> ReadAll() => 
            _context
                .Applicants
                .Include(it => it.Applications);

        public Applicant Update(Applicant applicant)
        {
            _context.Update(applicant);
            _context.SaveChanges();
            return applicant;
        }

        public void Delete(Guid id)
        {
            var applicant = Read(id);
            _context.Remove(applicant);
            _context.SaveChanges();
        }

        public Applicant? ReadByUserId(Guid userId) =>
            _context
                .Applicants
                .FirstOrDefault(e => e.UserId == userId);

        public Applicant? ReadByAlias(string alias) =>
            _context
                .Applicants
                .FirstOrDefault(it =>
                    String.Equals(
                        it.Alias.ToLower(),
                        alias.ToLower()
                    )
                );

        public bool Exists(string alias) => 
            _context
                .Applicants
                .Any(it =>
                    String.Equals(
                        it.Alias.ToLower(),
                        alias.ToLower()
                    )
                );
    }
}