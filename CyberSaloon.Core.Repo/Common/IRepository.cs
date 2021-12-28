using System;
using System.Collections.Generic;

namespace CyberSaloon.Core.Repo.Common
{
    public interface IRepository<T>
    {
        void Create(T entity);
        T? Read(Guid id);
        IEnumerable<T> ReadAll(IEnumerable<Guid> ids);
        IEnumerable<T> ReadAll();
        bool Exists(Guid id);
        T Update(T entity);
        void Delete(Guid id);    
    }
}