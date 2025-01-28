using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using LUCSALEInvestRADZEN.Data;

namespace LUCSALEInvestRADZEN.Controllers
{
    public partial class ExportCadastroDBController : ExportController
    {
        private readonly CadastroDBContext context;
        private readonly CadastroDBService service;

        public ExportCadastroDBController(CadastroDBContext context, CadastroDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/CadastroDB/alunos/csv")]
        [HttpGet("/export/CadastroDB/alunos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAlunosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAlunos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CadastroDB/alunos/excel")]
        [HttpGet("/export/CadastroDB/alunos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAlunosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAlunos(), Request.Query, false), fileName);
        }
    }
}
