using System;
using System.Collections.Generic;

namespace TaskQueue.DAO.Interface
{
    public interface IBaseDAO<TEntity, UId>
        where UId : struct, IComparable
        where TEntity : class, IEntity<UId>, new()
    {
        UId Create(TEntity item);

        IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> items);

        TEntity Read(UId id);

        IEnumerable<TEntity> ReadMultiple(IEnumerable<UId> Ids);

        int Update(TEntity item);

        int UpdateMultiple(IEnumerable<TEntity> items);

        int Delete(UId itemId);

        int DeleteMultiple(IEnumerable<UId> itemsId);

        int Count();
    }
}