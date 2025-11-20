using System.Collections.Generic;
using Common;

namespace Abstraction
{
    public interface IEntityContext<T> where T : Person
    {
        List<T> GetAll();
        void SaveAll(List<T> data);
        void Add(T entity);
    }
}