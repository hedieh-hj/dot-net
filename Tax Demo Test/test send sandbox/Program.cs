using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;

class Program
{
    private static string _uid;
    private static string _referenceNumber;

    private static readonly string TaxId = "TaxId";
    private static readonly string Privatekey = "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIB..."; 

    static async Task Main()
    {
        if (!await InitializeAsync())
        {
            Console.WriteLine("❌ Initialization failed.");
            return;
        }

        var token = TaxApiService.Instance.TaxApis.GetToken();
        var random = new Random();
        long randomSerial = random.Next(999_999_999);

        var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var taxIdGenerated = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(TaxId, randomSerial, DateTime.Now);

        var invoice = new InvoiceDto
        {
            Body = new List<InvoiceBodyDto> { CreateInvoiceBody() },
            Header = CreateInvoiceHeader(now, taxIdGenerated, randomSerial),
            Payments = new List<PaymentDto> { CreatePayment() }
        };

        await SendInvoiceAsync(invoice);
    }

    private static async Task<bool> InitializeAsync()
    {
        try
        {
            TaxApiService.Instance.Init(
                TaxId,
                new SignatoryConfig(Privatekey, null),
                new NormalProperties(ClientType.SELF_TSP),
                baseUrl: "https://sandboxrc.tax.gov.ir/req/api", //"https://tp.tax.gov.ir/req/api",
                contentSignatoryConfig: new SignatoryConfig(Privatekey, null));

            await TaxApiService.Instance.TaxApis.GetServerInformationAsync();
            await TaxApiService.Instance.TaxApis.RequestTokenAsync();

            Console.WriteLine("✅ Initialization successful.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ TaxApiException: {ex.Message}");
            return false;
        }
    }

    private static async Task SendInvoiceAsync(InvoiceDto invoiceData)
    {
        var requestBody = new List<InvoiceDto> { invoiceData };

        try
        {
            var response = await TaxApiService.Instance.TaxApis.SendInvoicesAsync(requestBody, null);

            if (response?.Body?.Result != null && response.Body.Result.Any())
            {
                var result = response.Body.Result.First();
                _uid = result.Uid;
                _referenceNumber = result.ReferenceNumber;

                Console.WriteLine($"✅ Invoice sent successfully. UID: {_uid}, Reference: {_referenceNumber}");
            }
            else
            {
                Console.WriteLine("❌ Failed to send invoice. Errors:");
                if (response?.Body?.Errors != null)
                {
                    foreach (var error in response.Body.Errors)
                        Console.WriteLine($"- {error.ErrorCode}: {error.Detail}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception during SendInvoiceAsync: {ex.Message}");
        }
    }

    private static PaymentDto CreatePayment()
    {
        return new PaymentDto
        {
            Iinn = "1131244211",
            Acn = "2131244212",
            Trmn = "3131244213",
            Trn = "4131244214"
        };
    }

    private static InvoiceBodyDto CreateInvoiceBody()
    {
        return new InvoiceBodyDto
        {
            Sstid = "1",
            Sstt = "شیر کم چرب پاستوریزه",
            Mu = "006584",
            Am = 2,
            Fee = 500_000,
            Prdis = 500_000,
            Dis = 0,
            Adis = 500_000,
            Vra = 0,
            Vam = 0,
            Tsstam = 1_000_000
        };
    }

    private static InvoiceHeaderDto CreateInvoiceHeader(long timestamp, string taxId, long serial)
    {
        return new InvoiceHeaderDto
        {
            Inty = 1,
            Inp = 1,
            Inno = serial.ToString(),
            Ins = 1,
            Tins = "1",
            Tprdis = 1_000_000,
            Tadis = 1_000,
            Tdis = 0,
            Tvam = 0,
            Todam = 0,
            Tbill = 1_000_000,
            Setm = 1,
            Cap = 1_000_000,
            Insp = 1_000_000,
            Tvop = 0,
            Tax17 = 0,
            Indatim = timestamp,
            Indati2m = timestamp,
            Taxid = taxId
        };
    }
}
