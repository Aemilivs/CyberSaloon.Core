using System;
using System.Collections.Generic;
using System.Linq;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.Repo.Artists
{
    public class ArtistsRepository : IArtistsRepository
    {
        private readonly CyberSaloonDBContext _context;
        public ArtistsRepository(CyberSaloonDBContext context)
        {
            _context =
                context ?? 
                throw new ArgumentNullException(nameof(context));
        }

        public void Create(Artist entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Exists(Guid id) => _context.Artists.Any(e => e.Id == id);

        public Artist? Read(Guid id) => 
            _context
                .Artists
                .FirstOrDefault(e => e.Id == id);

        public IEnumerable<Artist> ReadAll(IEnumerable<Guid> ids) => 
            _context
                .Artists
                .Where(e => ids.Contains(e.Id));

        public IEnumerable<Artist> ReadAll() => _context.Artists;

        public Artist Update(Artist artist)
        {
            _context.Update(artist);
            _context.SaveChanges();
            return artist;
        }

        public void Delete(Guid id)
        {
            var artist = Read(id);
            _context.Remove(artist);
            _context.SaveChanges();
        }

        public Artist? ReadByUserId(Guid userId) =>
            _context
                .Artists
                .FirstOrDefault(e => e.UserId == userId);

        public Artist? ReadByAlias(string alias) =>
            _context
                .Artists
                .FirstOrDefault(it => 
                    String.Equals(
                            it.Alias.ToLower(), 
                            alias.ToLower()
                        )
                    );
        
        public bool Exists(string alias) => 
            _context
                .Artists
                .Any(it =>
                    String.Equals(
                        it.Alias.ToLower(),
                        alias.ToLower()
                    )
                );
    }
}