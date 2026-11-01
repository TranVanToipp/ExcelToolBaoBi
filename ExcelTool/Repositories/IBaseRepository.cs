using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public interface IBaseRepository<T>
    {
        List<T> GetAll();

        T? GetById(int id);

        bool Insert(T entity);

        bool Update(T entity);

        bool Delete(int id);
    }
}