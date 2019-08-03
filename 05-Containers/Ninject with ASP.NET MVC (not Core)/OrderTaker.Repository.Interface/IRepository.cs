using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.Interface
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetItems();

        IEnumerable<T> GetItems(int filterKey);

        IEnumerable<T> SearchByName(string searchString);

        T GetItem(int key);

        void AddItem(T newItem);

        void UpdateItem(int key, T updatedItem);

        void DeleteItem(int key);
    }
}
