using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskQueue.Authentication.DAO.Interface;

namespace TaskQueue.Authentication.DAO.DAO
{
    internal abstract class BaseDAO<TEntity, UId> : IBaseDAO<TEntity, UId>
        where UId : struct, IComparable
        where TEntity : class, IEntity<UId>, new()
    {
        protected ApplicationDBContext _db;

        protected DbSet<TEntity> _table;

        public BaseDAO(ApplicationDBContext dbContext)
        {
            _db = dbContext;
            _table = _db.Set<TEntity>();
        }

        public virtual UId Create(TEntity item)
        {
            _table.Add(item);
            _db.SaveChanges();
            return item.Id;
        }

        public virtual IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> items)
        {
            _table.AddRange(items);
            _db.SaveChanges();
            return items;
        }

        public virtual TEntity Read(UId id)
        {
            return _table.Find(id);
        }

        public virtual int Update(TEntity item)
        {
            if (_table.Local.Contains(item))
            {
                _db.Entry(item).State = EntityState.Detached;
            }

            _db.Update(item);
            return _db.SaveChanges();
        }

        public virtual int UpdateMultiple(IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                if (_table.Local.Contains(item))
                {
                    _db.Entry(item).State = EntityState.Detached;
                }
            }

            _db.UpdateRange(items);
            return _db.SaveChanges();
        }

        public virtual int Delete(UId itemId)
        {
            TEntity tracked = _table.Local.SingleOrDefault(i => i.Id.CompareTo(itemId) == 0);

            _table.Remove(tracked ?? new TEntity { Id = itemId });
            return _db.SaveChanges();
        }

        public virtual int DeleteMultiple(IEnumerable<UId> itemsId)
        {
            List<TEntity> tracked = new List<TEntity>();
            foreach (UId id in itemsId)
            {
                TEntity item = _table.Local.SingleOrDefault(i => i.Id.CompareTo(id) == 0);
                tracked.Add(item ?? new TEntity { Id = id });
            }

            _table.RemoveRange(tracked);
            return _db.SaveChanges();
        }

        public virtual int Count()
        {
            return _table.Count();
        }

        public virtual IEnumerable<TEntity> ReadMultiple(IEnumerable<UId> Ids)
        {
            return _table.Where(e => Ids.Contains(e.Id)).ToList();
        }
    }
}