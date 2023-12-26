using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataUnitOfWork
{
    public class GameRepository : IGenericRepository<Game>, IDisposable
    {
        private DataContex contex;

        public GameRepository(DataContex contex)
        {
            this.contex = contex;
        }

        public List<Game> GetAll()
        {
            return contex.Games.ToList();
        }

        public Game GetByName(string name)
        {
            return contex.Games.FirstOrDefault(game => game.Name == name);
        }

        public void Insert(Game entity)
        {
            contex.Games.Add(entity);
        }

        public void Delete(string name)
        {
            Game game = contex.Games.FirstOrDefault(game => game.Name == name);
            contex.Games.Remove(game);
        }

        public void Update(Game entity)
        {
            contex.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            contex.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    contex.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
