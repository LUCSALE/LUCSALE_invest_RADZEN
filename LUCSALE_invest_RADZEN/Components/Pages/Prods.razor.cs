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
    public partial class Prods
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

        protected IEnumerable<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> prods;

        protected RadzenDataGrid<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> grid0;
        protected override async Task OnInitializedAsync()
        {
            prods = await LUCSALE_ExemplosService.GetProds();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddProd>("Add Prod", null);
            await grid0.Reload();
        }

        protected async Task EditRow(LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod args)
        {
            await DialogService.OpenAsync<EditProd>("Edit Prod", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod prod)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await LUCSALE_ExemplosService.DeleteProd(prod.Id);

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
                    Detail = $"Unable to delete Prod"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await LUCSALE_ExemplosService.ExportProdsToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Prods");
            }

            if (args == null || args.Value == "xlsx")
            {
                await LUCSALE_ExemplosService.ExportProdsToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Prods");
            }
        }
    }
}