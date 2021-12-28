using System;
using System.Collections.Generic;
using System.Linq;
using CyberSaloon.Core.Repo.Common;
using Microsoft.EntityFrameworkCore;

namespace CyberSaloon.Core.Repo.Arts
{
    public class ArtsRepository : IArtsRepository
    {
        private readonly CyberSaloonDBContext _context;
        public ArtsRepository(CyberSaloonDBContext context)
        {
            _context =
                context ?? 
                throw new ArgumentNullException(nameof(context));
        }

        public void Create(Art entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Exists(Guid id) => _context.Arts.Any(e => e.Id == id);

        public Art? Read(Guid id) => 
            _context
                .Arts
                .Include(it => it.Application)
                .FirstOrDefault(it => it.Id == id);

        public IEnumerable<Art> ReadAll(IEnumerable<Guid> ids) => 
            _context
                .Arts
                .Include(it => it.Application)
                .Where(e => ids.Contains(e.Id));

        public IEnumerable<Art> ReadAll() => 
            _context
                .Arts
                .Include(it => it.Application);

        public Art Update(Art artist)
        {
            _context.Update(artist);
            _context.SaveChanges();
            return artist;
        }

        public void Delete(Guid id)
        {
            var entity = Read(id);
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}