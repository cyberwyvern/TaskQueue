using System;
using System.ComponentModel.DataAnnotations;

namespace TaskQueue.DAO.Interface
{
    public interface IEntity<T> where T : struct, IComparable
    {
        [Key]
        T Id { get; set; }
    }
}