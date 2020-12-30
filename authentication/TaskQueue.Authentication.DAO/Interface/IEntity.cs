using System;
using System.ComponentModel.DataAnnotations;

namespace TaskQueue.Authentication.DAO.Interface
{
    public interface IEntity<T> where T : struct, IComparable
    {
        [Key]
        T Id { get; set; }
    }
}