
using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCDbContext _dbContext;

        public GenericRepository(MVCDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
          await  _dbContext.AddAsync(item);
           
        }

        public void Delete(T item)
        {
            _dbContext.Remove(item);
          
        }

        //public async Task<IEnumerable<T>> GetAll()
        //{
        //    if (typeof(T) == typeof(Employee))
        //    {
        //        var employees = await _dbContext.Employees.Include(e => e.Departments).ToListAsync();
        //        return employees.Cast<T>(); // Use Cast<T>() to safely cast the list
        //    }
        //    return await _dbContext.Set<T>().ToListAsync(); // Use ToListAsync() in async methods
        //}

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Departments).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
           
        }
    }
}
