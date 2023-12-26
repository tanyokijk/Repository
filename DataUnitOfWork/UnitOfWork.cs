using Data;
using Models;

namespace DataUnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private DataContex contex = new DataContex();
        private GameRepository gameRepository;
        private StudioRepository studioRepository;

        public GameRepository GameRepository
        {
            get
            {

                if (this.gameRepository == null)
                {
                    this.gameRepository = new GameRepository(contex);
                }
                return gameRepository;
            }
        }

        public StudioRepository StudioRepository
        {
            get
            {

                if (this.studioRepository == null)
                {
                    this.studioRepository = new StudioRepository(contex);
                }

                return studioRepository;
            }
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
