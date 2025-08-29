using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class Repository<T>: IRepository<T> where T : class 
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
            //asnotracking melhora a performance em consultas que não precisam de rastreamento
        }

        public T? GetId(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            //Atualização parcial identifica apenas o que teve de alterações na Entidade
            //_context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //Atualiza todos os campos
            _context.Set<T>().Update(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }
    }
}
