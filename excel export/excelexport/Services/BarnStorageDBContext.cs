using DocumentFormat.OpenXml.InkML;
using excelexport.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excelexport.Services
{
    public class BarnStorageDBContext : DbContext
    {
        public BarnStorageDBContext(DbContextOptions<BarnStorageDBContext> options) : base(options) { }

        public DbSet<TaxInvoiceFieldsModel> TaxInvoiceFields { get; set; }

        public DbSet<ExcelImportFieldTemplateModel> ExcelImportFieldTemplates { get; set; }

    }
}
    