using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using LUCSALEInvestRADZEN.Data;

namespace LUCSALEInvestRADZEN.Controllers
{
    public partial class ExportLUCSALE_ExemplosController : ExportController
    {
        private readonly LUCSALE_ExemplosContext context;
        private readonly LUCSALE_ExemplosService service;

        public ExportLUCSALE_ExemplosController(LUCSALE_ExemplosContext context, LUCSALE_ExemplosService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/LUCSALE_Exemplos/prods/csv")]
        [HttpGet("/export/LUCSALE_Exemplos/prods/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProdsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProds(), Request.Query, false), fileName);
        }

        [HttpGet("/export/LUCSALE_Exemplos/prods/excel")]
        [HttpGet("/export/LUCSALE_Exemplos/prods/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProdsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProds(), Request.Query, false), fileName);
        }
    }
}
