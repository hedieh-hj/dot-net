namespace excelexport.Models
{
    public class ExcelImportFieldTemplateModel
    {
        public int Id { get; set; }

        public required int PartType { get; set; }

        public required string Name { get; set; }

        public required string Caption { get; set; }

        public bool Visible { get; set; }
    }

    public class TaxInvoiceFieldsModel
    {
        public int Id { get; set; }

        public required string ExcelFieldName { get; set; }

        public required string Name { get; set; }

        public required string Caption { get; set; }

        public int Position { get; set; }

        public int Order { get; set; }

        public bool Visible { get; set; }

        public string? Type { get; set; }

        public string? RequireInType { get; set; }

        public string? RequireInPattern { get; set; }
    }

    public class TaxInvoiceFieldModel
    {
        public required string Name { get; set; }

        public required string Caption { get; set; }
    }
}
