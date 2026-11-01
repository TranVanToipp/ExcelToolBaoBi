using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
    {
        public abstract List<T> GetAll();

        public abstract T? GetById(int id);

        public abstract bool Insert(T entity);

        public abstract bool Update(T entity);

        public abstract bool Delete(int id);
    }
}