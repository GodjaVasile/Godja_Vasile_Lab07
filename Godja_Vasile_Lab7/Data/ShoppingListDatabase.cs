using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Godja_Vasile_Lab7.Models;
using Godja_Vasile_Lab7.Models;

namespace Godja_Vasile_Lab7.Data
{
    public class ShoppingListDatabase
    {
        //Aceasta clasa va contine cod pentru crearea, citirea, srierea si stergerea datelor
        //Utilizam API-r SQLite asincrone pentru a pune operatiile pe baza de date in thread-uri de background
        //Constructorul a estei clase ia ca si argument calea catre fisierul de tip baza de date 
        // => aceasta cale este furnizata de clasa App in pasul urmator

        readonly SQLiteAsyncConnection _database;
        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();

        }

        public Task<List<Shop>> GetShopsAsync()
        {
            return _database.Table<Shop>().ToListAsync();
        }
        public Task<int> SaveShopAsync(Shop shop)
        {
            if (shop.ID != 0)
            {
                return _database.UpdateAsync(shop);
            }
            else
            {
                return _database.InsertAsync(shop);
            }
        }
        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }
        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product);
        }
        public Task<List<Product>> GetProductsAsync()

        {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }
        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
            .Where(i => i.ID == id)
           .FirstOrDefaultAsync();
        }
        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);

            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }
        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<int> DeleteListProductAsync(ListProduct listp)
        {
            return _database.DeleteAsync(listp);
        }

        public Task<List<ListProduct>> GetListProducts()
        {
            return _database.QueryAsync<ListProduct>("select * from ListProduct");
        }


        public Task<int> SaveListProductAsync(ListProduct listp)
        {
            if (listp.ID != 0)
            {
                return _database.UpdateAsync(listp);
            }
            else
            {
                return _database.InsertAsync(listp);
            }
        }
        public Task<List<Product>> GetListProductsAsync(int shoplistid)
        {
            return _database.QueryAsync<Product>(
            "select P.ID, P.Description from Product P"
            + " inner join ListProduct LP"
            + " on P.ID = LP.ProductID where LP.ShopListID = ?",
            shoplistid);
        }

    }

}