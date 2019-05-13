using Data.Extension;
using Domain.Model;
using Domain.Model.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public abstract class Repository<T> where T : AppBaseEntity
    {
        enum TrackOperation { Insert, Update }
        #region Properties
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }
        protected DbSet<T> DbSet
        {
            get { return DbContext.Set<T>(); }
        }

        protected IQueryable<T> DbQueryable
        {
            get { return DbSet; }
        }

        protected NotificationDispatcherEntities DbContext
        {
            //get { return dataContext ?? (dataContext = DbFactory.Init()); }
            get { return DbFactory.Init(); }
        }
        #endregion

        protected Repository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        public virtual T Add(T entity)
        {
            if (EntityIdGeneration(entity))
            {
                GenerateDefaultValueForId(entity);
            }
            SetTrackingInformations(entity, TrackOperation.Insert);
            return DbSet.Add(entity);
        }

        public virtual IEnumerable<T> Add(IEnumerable<T> entities)
        {
            var result = new List<T>();

            foreach (var entity in entities)
            {
                result.Add(Add(entity));
            }
            return result;
        }

        public virtual T Update(T entity)
        {
            SetTrackingInformations(entity, TrackOperation.Update);
            var result = DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return result;
        }
        public virtual IEnumerable<T> Update(IEnumerable<T> entities)
        {
            var result = new List<T>();
            foreach (var entity in entities)
            {
                result.Add(Update(entity));
            }
            return result;
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = DbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                DbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbQueryable.Where(where).ToList();
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return DbQueryable.Where(where).FirstOrDefaultAsync<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return DbQueryable.Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where)
        {
            return DbQueryable.AsNoTracking().Where(where).ToList();
        }

        public virtual T GetAsNoTracking(Expression<Func<T, bool>> where)
        {
            return DbQueryable.AsNoTracking().Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetAllAsNoTracking()
        {
            return DbSet.AsNoTracking().ToList();
        }


        private bool EntityIdGeneration(T entity)
        {
            var attr = typeof(T).GetAttributeFromClass((AppIdGenerationAttribute ta) => ta);

            return (attr != null);
        }

        private void GenerateDefaultValueForId(T entity)
        {
            var keyField = typeof(T).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).FirstOrDefault();

            if (keyField == null)
                throw new Exception("Transmodel key generation: KeyAttribute not defined");

            var currValue = entity.GetType().GetProperty(keyField.Name).GetValue(entity);

            if ((!(currValue is int)) && (((int)currValue) != 0))
                return; //key valorizzata, non serve default value

            var dbGenAttr = entity.GetAttributeFromProperty<DatabaseGeneratedAttribute>(keyField.Name);

            if ((dbGenAttr != null) && (dbGenAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None))
            {
                var tableAttribute = typeof(T).GetAttributeFromClass((TableAttribute ta) => ta);
                if (tableAttribute != null)
                {
                    var newValue = GetNextId(tableAttribute.Name);
                    entity.GetType().GetProperty(keyField.Name).SetValue(entity, newValue, null);
                }
            }
        }

        private void SetTrackingInformations(T entity, TrackOperation trackOperation)
        {
            if (trackOperation == TrackOperation.Insert)
            {
                entity.InsertUser = Thread.CurrentPrincipal.Identity.Name;
                entity.InsertDatetime = DateTime.Now;
                entity.InsertGuid = Guid.NewGuid();
                entity.UpdateUser = entity.InsertUser;
                entity.UpdateDatetime = entity.InsertDatetime;
                entity.UpdateGuid = entity.InsertGuid;
            }

            if (trackOperation == TrackOperation.Update)
            {
                entity.UpdateUser = Thread.CurrentPrincipal.Identity.Name;
                entity.UpdateDatetime = DateTime.Now;
                entity.UpdateGuid = Guid.NewGuid();
            }
        }

        protected int GetNextId(string tableName)
        {
            return GetNextId(tableName, 1);
        }

        protected int GetNextId(string tableName, int rangeSize)
        {
            try
            {
                //TODO: vautare il valore di timeout. E' stato modificato questo perchè la GetSequence è la prima cosa che viene chiamata
                DbContext.Database.CommandTimeout = 180;
                var rawQuery = DbContext.Database.SqlQuery<int>(String.Format("declare @idn int; EXEC dbo.GetSequence @tableName = '{0}', @rangeSize = {1}, @value = @idn output; select @idn", tableName, rangeSize));
                var task = rawQuery.SingleAsync();
                int nextVal = task.Result;

                return nextVal;
            }
            catch (Exception e)
            {
                // throw new Exception(String.Format(BackendResources.ErrSequenceTable, tableName));
            }
        }
    }
}
