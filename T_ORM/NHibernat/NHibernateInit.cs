using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernat
{
    public class ProductDao
    {
        private ISessionFactory sessionFactory;

        public ProductDao()
        {
            var cfg = new NHibernate.Cfg.Configuration().Configure("hibernate.cfg.xml");
            sessionFactory = cfg.BuildSessionFactory();
        }

        public object Save(Product entity)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var id = session.Save(entity);
                session.Flush();
                return id;
            }
        }

        public void Update(Product entity)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                session.Update(entity);
                session.Flush();
            }
        }

        public void Delete(Product entity)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                session.Delete(entity);
                session.Flush();
            }
        }

        public Product Get(object id)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Get<Product>(id);
            }
        }

        public Product Load(object id)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Load<Product>(id);
            }
        }

        public IList<Product> LoadAll()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.QueryOver<Product>().List();
            }
        }
    }
}
