using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace LUCSALEInvestRADZEN.Components.Pages
{
    public partial class Alunos
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public CadastroDBService CadastroDBService { get; set; }

        protected IEnumerable<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> alunos;

        protected RadzenDataGrid<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> grid0;
        protected bool isEdit = true;
        protected override async Task OnInitializedAsync()
        {
            alunos = await CadastroDBService.GetAlunos();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            aluno = new LUCSALEInvestRADZEN.Models.CadastroDB.Aluno();
        }

        protected async Task EditRow(LUCSALEInvestRADZEN.Models.CadastroDB.Aluno args)
        {
            isEdit = true;
            aluno = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, LUCSALEInvestRADZEN.Models.CadastroDB.Aluno aluno)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CadastroDBService.DeleteAluno(aluno.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Aluno"
                });
            }
        }
        protected bool errorVisible;
        protected LUCSALEInvestRADZEN.Models.CadastroDB.Aluno aluno;

        protected async Task FormSubmit()
        {
            try
            {
                var result = isEdit ? await CadastroDBService.UpdateAluno(aluno.Id, aluno) : await CadastroDBService.CreateAluno(aluno);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}