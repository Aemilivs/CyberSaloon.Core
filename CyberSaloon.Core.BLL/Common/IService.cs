using System;
using System.Collections.Generic;
using CyberSaloon.Core.Repo.Common;

namespace CyberSaloon.Core.BLL.Common
{
    public interface IService<T>
    {
        void Create(T entity);
        
        T? Read(Guid id);
        IEnumerable<T> ReadAll(IEnumerable<Guid> ids);
        IEnumerable<T> ReadAll();

        void Update(T entity);

        void Delete(Guid id);
    }
}