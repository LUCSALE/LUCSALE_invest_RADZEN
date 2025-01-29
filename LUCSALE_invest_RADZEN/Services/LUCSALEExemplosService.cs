using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using LUCSALEInvestRADZEN.Data;

namespace LUCSALEInvestRADZEN
{
    public partial class LUCSALE_ExemplosService
    {
        LUCSALE_ExemplosContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly LUCSALE_ExemplosContext context;
        private readonly NavigationManager navigationManager;

        public LUCSALE_ExemplosService(LUCSALE_ExemplosContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportProdsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/lucsale_exemplos/prods/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/lucsale_exemplos/prods/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProdsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/lucsale_exemplos/prods/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/lucsale_exemplos/prods/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProdsRead(ref IQueryable<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> items);

        public async Task<IQueryable<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod>> GetProds(Query query = null)
        {
            var items = Context.Prods.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProdsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProdGet(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);
        partial void OnGetProdById(ref IQueryable<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> items);


        public async Task<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> GetProdById(int id)
        {
            var items = Context.Prods
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetProdById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProdGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProdCreated(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);
        partial void OnAfterProdCreated(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);

        public async Task<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> CreateProd(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod prod)
        {
            OnProdCreated(prod);

            var existingItem = Context.Prods
                              .Where(i => i.Id == prod.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Prods.Add(prod);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(prod).State = EntityState.Detached;
                throw;
            }

            OnAfterProdCreated(prod);

            return prod;
        }

        public async Task<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> CancelProdChanges(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProdUpdated(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);
        partial void OnAfterProdUpdated(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);

        public async Task<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> UpdateProd(int id, LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod prod)
        {
            OnProdUpdated(prod);

            var itemToUpdate = Context.Prods
                              .Where(i => i.Id == prod.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(prod);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProdUpdated(prod);

            return prod;
        }

        partial void OnProdDeleted(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);
        partial void OnAfterProdDeleted(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod item);

        public async Task<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> DeleteProd(int id)
        {
            var itemToDelete = Context.Prods
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProdDeleted(itemToDelete);


            Context.Prods.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProdDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}