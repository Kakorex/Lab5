using System;
using Common;
using Abstraction;

namespace DAL
{
    public class EntityContext<T> : IEntityContext<T> where T : Person
    {
        private readonly IDataProvider<T> _dataProvider;
        private readonly string _filePath;

        public EntityContext(IDataProvider<T> dataProvider, string filePath)
        {
            _dataProvider = dataProvider;
            _filePath = filePath;
        }

        public List<T> GetAll()
        {
            return _dataProvider.Load(_filePath);
        }

        public void SaveAll(List<T> data)
        {
            _dataProvider.Save(_filePath, data);
        }

        public void Add(T entity)
        {
            List<T> entities = GetAll();
            entities.Add(entity);
            SaveAll(entities);
        }
    }
}
