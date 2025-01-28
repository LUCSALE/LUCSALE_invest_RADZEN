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
    public partial class CadastroDBService
    {
        CadastroDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly CadastroDBContext context;
        private readonly NavigationManager navigationManager;

        public CadastroDBService(CadastroDBContext context, NavigationManager navigationManager)
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


        public async Task ExportAlunosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cadastrodb/alunos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cadastrodb/alunos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAlunosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cadastrodb/alunos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cadastrodb/alunos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAlunosRead(ref IQueryable<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> items);

        public async Task<IQueryable<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno>> GetAlunos(Query query = null)
        {
            var items = Context.Alunos.AsQueryable();


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

            OnAlunosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAlunoGet(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);
        partial void OnGetAlunoById(ref IQueryable<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> items);


        public async Task<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> GetAlunoById(int id)
        {
            var items = Context.Alunos
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAlunoById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAlunoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAlunoCreated(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);
        partial void OnAfterAlunoCreated(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);

        public async Task<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> CreateAluno(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno aluno)
        {
            OnAlunoCreated(aluno);

            var existingItem = Context.Alunos
                              .Where(i => i.Id == aluno.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Alunos.Add(aluno);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aluno).State = EntityState.Detached;
                throw;
            }

            OnAfterAlunoCreated(aluno);

            return aluno;
        }

        public async Task<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> CancelAlunoChanges(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAlunoUpdated(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);
        partial void OnAfterAlunoUpdated(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);

        public async Task<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> UpdateAluno(int id, LUCSALEInvestRADZEN.Models.CadastroDB.Aluno aluno)
        {
            OnAlunoUpdated(aluno);

            var itemToUpdate = Context.Alunos
                              .Where(i => i.Id == aluno.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aluno);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAlunoUpdated(aluno);

            return aluno;
        }

        partial void OnAlunoDeleted(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);
        partial void OnAfterAlunoDeleted(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno item);

        public async Task<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> DeleteAluno(int id)
        {
            var itemToDelete = Context.Alunos
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAlunoDeleted(itemToDelete);


            Context.Alunos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAlunoDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}