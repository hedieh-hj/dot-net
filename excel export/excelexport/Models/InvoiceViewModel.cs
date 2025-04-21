using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excelexport.Models
{
    public class InvoiceModel
    {
        public InvoiceHeaderModel? Header { get; set; }

        public List<InvoiceItemModel>? Body { get; set; }

        public List<InvoicePaymentModel>? Payments { get; set; }

        public List<InvoiceExtension>? Extension { get; set; }
    }
    
    public record InvoiceHeaderModel
    {
        #region 'TaxLib invoice header properties'
        public long? Indati2m { get; set; }

        public long? Indatim { get; set; }

        public int? Inty { get; set; }

        public int? Ft { get; set; }

        public string? Inno { get; set; }

        public string? Irtaxid { get; set; }

        public string? Scln { get; set; }

        public int? Setm { get; set; }

        public string? Tins { get; set; }

        public long? Cap { get; set; }

        public string? Bid { get; set; }

        public long? Insp { get; set; }

        public long? Tvop { get; set; }

        public string? Bpc { get; set; }

        public long? Tax17 { get; set; }

        public string? Taxid { get; set; }

        public int? Inp { get; set; }

        public string? Scc { get; set; }

        public int? Ins { get; set; }

        public string? Billid { get; set; }

        public long? Tprdis { get; set; }

        public long? Tdis { get; set; }

        public long? Tadis { get; set; }

        public long? Tvam { get; set; }

        public long? Todam { get; set; }

        public long? Tbill { get; set; }

        public int? Tob { get; set; }

        public string? Tinb { get; set; }

        public string? Sbc { get; set; }

        public string? Bbc { get; set; }

        public string? Bpn { get; set; }

        public string? Crn { get; set; }

        public string? Cdcn { get; set; }

        public int? Cdcd { get; set; }

        public decimal? Tonw { get; set; }

        public long? Torv { get; set; }

        public decimal? Tocv { get; set; }

        public string? Tinc { get; set; }

        public string? Lno { get; set; }

        public string? Lrno { get; set; }

        public string? Ocu { get; set; }

        public string? Oci { get; set; }

        public string? Dco { get; set; }

        public string? Dci { get; set; }

        public string? Tid { get; set; }

        public string? Rid { get; set; }

        public byte? Lt { get; set; }

        public string? Cno { get; set; }

        public string? Did { get; set; }

        public List<ShippingGoodHeaderModel>? Sg { get; set; }

        public string? Asn { get; set; }

        public int? Asd { get; set; }
        #endregion

        #region 'Extra invoice header properties for control'           
        public string? PersonID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }
        
        public string? AsdTs { get; set; }
        
        public string? CdcdTs { get; set; }
        #endregion
    }

    public class InvoiceItemModel
    {
        #region 'TaxLib invoice body properties'
        public string? Sstid { get; set; }

        public string? Sstt { get; set; }

        public string? Mu { get; set; }

        public decimal? Am { get; set; }

        public decimal? Fee { get; set; }

        public decimal? Cfee { get; set; }

        public string? Cut { get; set; }

        public long? Exr { get; set; }

        public long? Prdis { get; set; }

        public long? Dis { get; set; }

        public long? Adis { get; set; }

        public decimal? Vra { get; set; }

        public long? Vam { get; set; }

        public string? Odt { get; set; }

        public decimal? Odr { get; set; }

        public long? Odam { get; set; }

        public string? Olt { get; set; }

        public decimal? Olr { get; set; }

        public long? Olam { get; set; }

        public long? Consfee { get; set; }

        public long? Spro { get; set; }

        public long? Bros { get; set; }

        public long? Tcpbs { get; set; }

        public long? Cop { get; set; }

        public string? Bsrn { get; set; }

        public long? Vop { get; set; }

        public long? Tsstam { get; set; }

        public decimal? Nw { get; set; }

        public long? Ssrv { get; set; }

        public decimal? Sscv { get; set; }

        public decimal? Cui { get; set; }

        //public string? Hs { get; set; }

        public decimal? Cpr { get; set; }

        public long? Sovat { get; set; }
        #endregion

        #region 'Extra invoice body properties for control'
        public long RowId { get; set; }

        public string? ItemID { get; set; }

        public int? UnitId { get; set; }

        public string? UnitTitle { get; set; }
        #endregion
    }

    public record InvoicePaymentModel
    {
        public long RowId { get; set; }

        public string? PaymentDate { get; set; }

        public string? Iinn { get; init; }

        public string? Acn { get; init; }

        public string? Trmn { get; init; }

        public string? Trn { get; init; }

        public string? Pcn { get; init; }

        public string? Pid { get; init; }

        public long? Pdt { get; init; }

        public int? Pmt { get; init; }

        public long? Pv { get; init; }
    }

    public class ShippingGoodHeaderModel
    {
        public string? Sgid { get; set; }

        public string? Sgt { get; set; }
    } 

    public record InvoiceExtension;
}
