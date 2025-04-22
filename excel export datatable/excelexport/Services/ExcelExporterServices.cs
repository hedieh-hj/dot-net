using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using excelexport.Models;
using MD.PersianDateTime.Standard;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace excelexport.Services
{
    public class ExcelExporterServices(BarnStorageDBContext context)
    {
        private readonly BarnStorageDBContext _context = context;

        public async Task<Dictionary<string, string>> GetCaptionAsync()
        {
            try
            {
                var _result = await _context.TaxInvoiceFields.AsNoTracking()
                   .Select(x => new TaxInvoiceFieldModel
                   {
                       Name = x.Name,
                       Caption = x.Caption
                   })
                  .ToDictionaryAsync(x => x.Name, x => x.Caption);

                return new Dictionary<string, string>(_result, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetExcellResultAsync(InvoiceModel invoice)
        {
            try
            {                
                var dataTable = new DataTable("MyDataTable");
                dataTable.Columns.Add("شناسه مالیاتی", typeof(string));
                dataTable.Columns.Add("تاریخ صدور", typeof(string));
                dataTable.Columns.Add("شماره سریال", typeof(string));
                dataTable.Columns.Add("کد کالا", typeof(string));
                dataTable.Columns.Add("شرح کالا و خدمت", typeof(string));
                dataTable.Columns.Add("تعداد", typeof(long));
                dataTable.Columns.Add("روش پرداخت", typeof(long));
                dataTable.Columns.Add("تاریخ و زمان پرداخت", typeof(string));

                var _row_count = invoice.Body.Count;

                for (int i = 0; i < invoice.Body.Count; i++)
                {
                    var paymentItem = (invoice.Payments != null && invoice.Payments.Count > i) ? invoice.Payments[i] : null;

                    dataTable.Rows.Add(                        
                        invoice.Header.Taxid,
                        ToJalaali(DateTimeOffset.FromUnixTimeMilliseconds(invoice.Header.Indati2m ?? 0).DateTime),
                        invoice.Header.Inno,
                        invoice.Body[i].Sstid,
                        invoice.Body[i].Sstt,
                        invoice.Body[i].Am,
                        paymentItem?.Pmt,
                        ToJalaali(DateTimeOffset.FromUnixTimeMilliseconds(paymentItem.Pdt ?? 0).DateTime));
                }

                var workbook = new XLWorkbook();

                var worksheet = workbook.Worksheets.Add(dataTable, "invoice");

                worksheet.RightToLeft = true;

                var t = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                workbook.SaveAs($"{t}.xlsx");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in generating Excel", ex);
            }
        }

        public static string ToJalaali(DateTime date)
        {
            var persian_date = new PersianDateTime(date).ToString("yyyy/MM/dd");

            return persian_date;
        }
    }
}
