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
    public partial class EditProd
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
        public LUCSALE_ExemplosService LUCSALE_ExemplosService { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            prod = await LUCSALE_ExemplosService.GetProdById(Id);
        }
        protected bool errorVisible;
        protected LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod prod;

        protected async Task FormSubmit()
        {
            try
            {
                await LUCSALE_ExemplosService.UpdateProd(Id, prod);
                DialogService.Close(prod);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}