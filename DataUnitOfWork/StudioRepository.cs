using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUnitOfWork
{
    public class StudioRepository : IGenericRepository<Studio>, IDisposable
    {
        private DataContex contex;

        public StudioRepository(DataContex contex)
        {
            this.contex = contex;
        }

        public List<Studio> GetAll()
        {
            return contex.Studios.ToList();
        }

        public Studio GetByName(string name)
        {
            return contex.Studios.FirstOrDefault(s => s.Name == name);
        }

        public void Insert(Studio entity)
        {
            contex.Studios.Add(entity);
        }

        public void Delete(string name)
        {
            Studio studio = contex.Studios.FirstOrDefault(s => s.Name == name);
            contex.Studios.Remove(studio);
        }

        public void Update(Studio entity)
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
