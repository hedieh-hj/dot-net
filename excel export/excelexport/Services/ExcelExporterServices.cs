using ClosedXML.Excel;
using excelexport.Models;
using MD.PersianDateTime.Standard;
using Microsoft.EntityFrameworkCore;
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

        public async Task<MemoryStream> GetExcellResultAsync(InvoiceModel invoice)
        {
            try
            {
                var captions = await GetCaptionAsync();

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("invoice");
                worksheet.RightToLeft = true;

                int currentRow = 1;

                var headerProps = typeof(InvoiceHeaderModel).GetProperties();
                var bodyProps = typeof(InvoiceItemModel).GetProperties();
                var paymentProps = typeof(InvoicePaymentModel).GetProperties();

                var orderedKeys = captions.Keys.ToList();

                int col = 1;

                foreach (var key in orderedKeys)
                {
                    var prop = headerProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                    if (prop != null)
                    {
                        var cell = worksheet.Cell(currentRow, col++);
                        cell.Value = captions[key];
                        cell.Style.Fill.BackgroundColor = XLColor.Green;
                    }
                }

                foreach (var key in orderedKeys)
                {
                    var prop = bodyProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                    if (prop != null)
                    {
                        var cell = worksheet.Cell(currentRow, col++);
                        cell.Value = captions[key];
                        cell.Style.Fill.BackgroundColor = XLColor.Orange;
                    }
                }

                foreach (var key in orderedKeys)
                {
                    var prop = paymentProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                    if (prop != null)
                    {
                        var cell = worksheet.Cell(currentRow, col++);
                        cell.Value = captions[key];
                        cell.Style.Fill.BackgroundColor = XLColor.Blue;
                    }
                }

                //set width to cells 
                //for (int i = 1; i <= col - 1; i++)
                //{
                //    worksheet.Column(i).AdjustToContents();
                //    worksheet.Column(i).Width = Math.Max(worksheet.Column(i).Width, 20); // عرض حداقلی 20 برای ستون‌ها
                //}

                currentRow++;

                for (int i = 0; i < invoice.Body.Count; i++)
                {
                    var item = invoice.Body[i];
                    var payment = invoice.Payments.ElementAtOrDefault(i);

                    col = 1;

                    foreach (var key in orderedKeys)
                    {
                        var prop = headerProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                        if (prop != null && captions.ContainsKey(key))
                        {
                            var value = prop.GetValue(invoice.Header);

                            if (prop.Name.Equals("Indati2m", StringComparison.OrdinalIgnoreCase))
                            {
                                if (value is long timestamp)
                                {
                                    var date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
                                    var cell = worksheet.Cell(currentRow, col++);
                                    cell.Value = ToJalaali(date);
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else
                                {
                                    var cell = worksheet.Cell(currentRow, col++);
                                    cell.Value = value?.ToString() ?? "";
                                    cell.Style.NumberFormat.Format = "@";
                                }
                            }
                            else
                            {
                                var cell = worksheet.Cell(currentRow, col++);
                                cell.Value = value?.ToString() ?? "";
                                var typeName = (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType).Name;

                                if (typeName.ToLower() == "string")
                                {
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else
                                {
                                    cell.Style.NumberFormat.Format = "0";
                                }
                            }
                        }
                    }

                    foreach (var key in orderedKeys)
                    {
                        var prop = bodyProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                        if (prop != null && captions.ContainsKey(key))
                        {
                            var value = prop.GetValue(item);
                            var cell = worksheet.Cell(currentRow, col++);
                            cell.Value = value?.ToString() ?? "";
                            var typeName = (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType).Name;

                            if (typeName.ToLower() == "string")
                            {
                                cell.Style.NumberFormat.Format = "@";
                            }
                            else
                            {
                                cell.Style.NumberFormat.Format = "0";
                            }
                        }
                    }

                    foreach (var key in orderedKeys)
                    {
                        var prop = paymentProps.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                        if (prop != null && captions.ContainsKey(key))
                        {
                            var value = prop.GetValue(payment);

                            if (prop.Name.Equals("Pdt", StringComparison.OrdinalIgnoreCase))
                            {
                                if (value is long timestamp)
                                {
                                    var date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
                                    var cell = worksheet.Cell(currentRow, col++);
                                    cell.Value = ToJalaali(date);
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else
                                {
                                    var cell = worksheet.Cell(currentRow, col++);
                                    cell.Value = value?.ToString() ?? "";
                                    cell.Style.NumberFormat.Format = "@";
                                }
                            }
                            else
                            {
                                var cell = worksheet.Cell(currentRow, col++);
                                cell.Value = value?.ToString() ?? "";
                                var typeName = (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType).Name;

                                if (typeName.ToLower() == "string")
                                {
                                    cell.Style.NumberFormat.Format = "@";
                                }
                                else
                                {
                                    cell.Style.NumberFormat.Format = "0";
                                }
                            }
                        }
                    }

                    currentRow++;
                }

                worksheet.Columns.Width = Math.Max(worksheet.Column(i).Width, 20);

                var t = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                
                //save excel file that generated
                workbook.SaveAs($"{t}.xlsx");

                var stream = new MemoryStream();

                workbook.SaveAs(stream);

                stream.Seek(0, SeekOrigin.Begin);
                
                //return excel file to client
                return stream;
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
