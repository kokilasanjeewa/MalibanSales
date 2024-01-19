using System.Threading.Tasks;

namespace Maliban.Pos.Api.Repository
{
  public interface IRepositoryWrapper
  {
    IBrandRepository Brand { get; }
    ISupplierRepository Supplier { get; }
    IUserRepository User { get; }
    ICategoryRepository Category { get; }
    ISubCategoryRepository SubCategory { get; }
    IPhotoRepository Photo { get; }
    IBranchRepository BranchLocation { get; }
    IVendorRepository Vendor { get; }
    IUnitOfMeasurementRepository UnitOfMeasurement { get; }
    IProductRepository Product { get; }
    IDepartmentRepository Department { get; }
    ISectionRepository Section { get; }
    IPriceRepository Price { get; }
    IStoreRepository Store { get; }
    IStockTransferRepository StockTransfer { get; }
    IStoreAreaRepository StoreArea { get; }
    IRackRepository Rack { get; }
    IBinRepository Bin { get; }
    IEmployeeRepository Employee { get; }
    ISupplierProduct SupplierProduct { get; }
    IShopStockRepository ShopStock { get; }
    IPurchaseRequestHeaderRepository PurchaseRequestHeader { get; }
    IPurchaseRequestDetailRepository PurchaseRequestDetail { get; }
    IPurchaseReturnHeaderRepository PurchaseReturnHeader { get; }
    IStockAdjustmentDetailRepository StockAdjustmentDetail { get; }
    IStockAdjustmentHeaderRepository StockAdjustmentHeader { get; }
    IPurchaseReturnDetailRepository PurchaseReturnDetail { get; }
    IPurchaseOrderHeaderRepository PurchaseOrderHeader { get; }
    IPurchaseOrderDetailRepository PurchaseOrderDetail { get; }
    IGoodReceiptDetailRepository GoodReceiptDetail { get; }
    ITransactionDetailRepository TransactionDetail { get; }
    IGoodReceiptHeaderRepository GoodReceiptHeader { get; }
    ISupplierPriceRepository SupplierPrice { get; }
    ICompanyMasterSettingRepository CompanyMasterSetting { get; }
    ICustomerMasterSettingRepository CustomerMasterSetting { get; }
    IReportRepository Report { get; }
    Task<bool> SaveAllAsync();
    Task<bool> VerifyBarCode(string barcode);
    Task<bool> VerifyEmpCode(string empcode);

    }
}