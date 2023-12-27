using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _context;
        private readonly ILog logger;
        private IProductRepository _product;
        private IPriceRepository _price;
        private IStoreRepository _store;
        private IRackRepository _rack;
        private IBinRepository _bin;
        private IStoreAreaRepository _storearea;
        private IBrandRepository _brand;
        private IVendorRepository _vendor;
        private IDepartmentRepository _department;
        private ISectionRepository _section;
        private ISupplierRepository _supplier;
        private IUserRepository _user;
        private IBranchRepository _branch;
        private ICategoryRepository _category;
        private ISubCategoryRepository _subcategory;
        private IUnitOfMeasurementRepository _unitOfmeasurement;
        private IPhotoRepository _photo;
        private ISortHelper<Supplier> _supplierSortHelper;
        private IHttpContextAccessor _httpContextAccessor;
        private IEmployeeRepository _employee;
        private ISupplierProduct _supplierproduct;
        private IShopStockRepository _shopstock;
        private IPurchaseRequestHeaderRepository _purchaserequestHeader;
        private IPurchaseRequestDetailRepository _purchaseRequestDetail;
        private IPurchaseReturnHeaderRepository _purchasereturnHeader;
        private IPurchaseReturnDetailRepository _purchaseReturnDetail;
        private IPurchaseOrderHeaderRepository _purchaseOrderHeader;
        private IStockAdjustmentHeaderRepository _stockadjustmentHeader;
        private IStockAdjustmentDetailRepository _stockadjustmentDetail;
        private IPurchaseOrderDetailRepository _purchaseOrderDetail;
        private IGoodReceiptDetailRepository _goodreceiptDetail;
        private ITransactionDetailRepository _transactionDetail;
        private IGoodReceiptHeaderRepository _goodreceiptHeader;
        private ISupplierPriceRepository _supplierprice;
        private ICompanyMasterSettingRepository _companyMasterSetting;
        private ICustomerMasterSettingRepository _customerMasterSetting;
        private IReportRepository _report;
        private ISortHelper<PurchaseOrderHeader> _purchaseOrderSortHelper;
        private ISortHelper<ShopStock> _stockSortHelper;
        private ISortHelper<PurchaseRequestHeader> _purchaserequestHeaderSortHelper;
        private ISortHelper<PurchaseRequestDetail> _purchaserequestDetailSortHelper;
        private ISortHelper<PurchaseReturnHeader> _purchasereturnHeaderSortHelper;
        private ISortHelper<PurchaseReturnDetail> _purchasereturnDetailSortHelper;
        private ISortHelper<PurchaseOrderDetail> _purchaseorderDetailSortHelper;
        private ISortHelper<GoodReceptNoteDetail> _goodreceiptDetailSortHelper;
        private ISortHelper<GoodReceptNoteHeader> _goodreceiptSortHelper;
        private ISortHelper<SupplierPrice> _supplierpriceSortHelper;
        private ISortHelper<ApplicationUser> _userSortHelper;
        private UserManager<ApplicationUser> _userManager;
        public RepositoryWrapper(DataContext ctx, UserManager<ApplicationUser> userManager, ISortHelper<Supplier> supplierSortHelper, ISortHelper<ApplicationUser> userSortHelper, ISortHelper<ShopStock> stockSortHelper, ISortHelper<PurchaseRequestHeader> purchaserequestHeaderSortHelper, ISortHelper<PurchaseRequestDetail> purchaserequestDetailSortHelper, ISortHelper<PurchaseReturnHeader> purchasereturnHeaderSortHelper, ISortHelper<PurchaseReturnDetail> purchasereturnDetailSortHelper, ISortHelper<PurchaseOrderHeader> purchaseOrderSortHelper, ISortHelper<PurchaseOrderDetail> purchaseorderDetailSortHelper, ISortHelper<GoodReceptNoteDetail> goodreceiptDetailSortHelper, ISortHelper<GoodReceptNoteHeader> goodreceiptSortHelper, ISortHelper<SupplierPrice> supplierpriceSortHelper, IHttpContextAccessor httpContextAccessor, ILog logger)
        {
            _context = ctx;
            _userManager = userManager;
            _supplierSortHelper = supplierSortHelper;
            _userSortHelper = userSortHelper;
            _stockSortHelper = stockSortHelper;
            _purchaserequestHeaderSortHelper = purchaserequestHeaderSortHelper;
            _purchaserequestDetailSortHelper = purchaserequestDetailSortHelper;
            _purchasereturnHeaderSortHelper = purchasereturnHeaderSortHelper;
            _purchasereturnDetailSortHelper = purchasereturnDetailSortHelper;
            _purchaseOrderSortHelper = purchaseOrderSortHelper;
            _purchaseorderDetailSortHelper = purchaseorderDetailSortHelper;
            _goodreceiptDetailSortHelper = goodreceiptDetailSortHelper;
            _goodreceiptSortHelper = goodreceiptSortHelper;
            _supplierpriceSortHelper = supplierpriceSortHelper;
            _httpContextAccessor = httpContextAccessor;
            this.logger = logger;

        }
        public IReportRepository Report
        {
            get
            {
                if (_report == null)
                {
                    _report = new EFReportRepository(_context, _httpContextAccessor);
                }

                return _report;
            }
        }
        public ICompanyMasterSettingRepository CompanyMasterSetting
        {
            get
            {
                if (_companyMasterSetting == null)
                {
                    _companyMasterSetting = new EFCompanyMasterSettingRepository(_context);
                }

                return _companyMasterSetting;
            }
        }
        public ICustomerMasterSettingRepository CustomerMasterSetting
        {
            get
            {
                if (_customerMasterSetting == null)
                {
                    _customerMasterSetting = new EFCustomerMasterSettingRepository(_context);
                }

                return _customerMasterSetting;
            }
        }
        public ISupplierPriceRepository SupplierPrice
        {
            get
            {
                if (_supplierprice == null)
                {
                    _supplierprice = new EFSupplierPriceRepository(_context, _supplierpriceSortHelper, _httpContextAccessor);
                }

                return _supplierprice;
            }
        }
        public IGoodReceiptHeaderRepository GoodReceiptHeader
        {
            get
            {
                if (_goodreceiptHeader == null)
                {
                    _goodreceiptHeader = new EFGoodReceiptHeaderRepository(_context, _goodreceiptSortHelper);
                }

                return _goodreceiptHeader;
            }
        }
        public IGoodReceiptDetailRepository GoodReceiptDetail
        {
            get
            {
                if (_goodreceiptDetail == null)
                {
                    _goodreceiptDetail = new EFGoodReceiptDetailRepository(_context, _goodreceiptDetailSortHelper);
                }

                return _goodreceiptDetail;
            }
        }
        public ITransactionDetailRepository TransactionDetail
        {
            get
            {
                if (_transactionDetail == null)
                {
                    _transactionDetail = new EFTransactionDetailRepository(_context);
                }

                return _transactionDetail;
            }
        }
        public IStockAdjustmentDetailRepository StockAdjustmentDetail
        {
            get
            {
                if (_stockadjustmentDetail == null)
                {
                    _stockadjustmentDetail = new EFStockAdjustmentDetailRepository(_context);
                }

                return _stockadjustmentDetail;
            }
        }
        public IStockAdjustmentHeaderRepository StockAdjustmentHeader
        {
            get
            {
                if (_stockadjustmentHeader == null)
                {
                    _stockadjustmentHeader = new EFStockAdjustmentHeaderRepository(_context);
                }

                return _stockadjustmentHeader;
            }
        }
        public IPurchaseOrderDetailRepository PurchaseOrderDetail
        {
            get
            {
                if (_purchaseOrderDetail == null)
                {
                    _purchaseOrderDetail = new EFPurchaseOrderDetailRepository(_context, _purchaseorderDetailSortHelper);
                }

                return _purchaseOrderDetail;
            }
        }
        public IPurchaseOrderHeaderRepository PurchaseOrderHeader
        {
            get
            {
                if (_purchaseOrderHeader == null)
                {
                    _purchaseOrderHeader = new EFPurchaseOrderHeaderRepository(_context, _purchaseOrderSortHelper);
                }

                return _purchaseOrderHeader;
            }
        }
        public IPurchaseReturnDetailRepository PurchaseReturnDetail
        {
            get
            {
                if (_purchaseReturnDetail == null)
                {
                    _purchaseReturnDetail = new EFPurchaseReturnDetailRepository(_context, _purchasereturnDetailSortHelper);
                }

                return _purchaseReturnDetail;
            }
        }
        public IPurchaseReturnHeaderRepository PurchaseReturnHeader
        {
            get
            {
                if (_purchasereturnHeader == null)
                {
                    _purchasereturnHeader = new EFPurchaseReturnHeaderRepository(_context, _purchasereturnHeaderSortHelper);
                }

                return _purchasereturnHeader;
            }
        }
        public IPurchaseRequestDetailRepository PurchaseRequestDetail
        {
            get
            {
                if (_purchaseRequestDetail == null)
                {
                    _purchaseRequestDetail = new EFPurchaseRequestDetailRepository(_context, _purchaserequestDetailSortHelper);
                }

                return _purchaseRequestDetail;
            }
        }
        public IPurchaseRequestHeaderRepository PurchaseRequestHeader
        {
            get
            {
                if (_purchaserequestHeader == null)
                {
                    _purchaserequestHeader = new EFPurchaseRequestHeaderRepository(_context, _purchaserequestHeaderSortHelper);
                }

                return _purchaserequestHeader;
            }
        }

        public IShopStockRepository ShopStock
        {
            get
            {
                if (_shopstock == null)
                {
                    _shopstock = new EFShopStockRepository(_context, _stockSortHelper, logger);
                }

                return _shopstock;
            }
        }

        public ISupplierProduct SupplierProduct
        {
            get
            {
                if (_supplierproduct == null)
                {
                    _supplierproduct = new EFSupplierProduct(_context);
                }

                return _supplierproduct;
            }
        }
        public IDepartmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new EFDepartmentRepository(_context);
                }

                return _department;
            }
        }
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EFEmployeeRepository(_context);
                }

                return _employee;
            }
        }
        public ISectionRepository Section
        {
            get
            {
                if (_section == null)
                {
                    _section = new EFSectionRepository(_context);
                }

                return _section;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new EFProductRepository(_context);
                }

                return _product;
            }
        }
        public IPriceRepository Price
        {
            get
            {
                if (_price == null)
                {
                    _price = new EFPriceRepository(_context);
                }

                return _price;
            }
        }
        public IBrandRepository Brand
        {
            get
            {
                if (_brand == null)
                {
                    _brand = new EFBrandRepository(_context);
                }

                return _brand;
            }
        }
        public ISupplierRepository Supplier
        {
            get
            {
                if (_supplier == null)
                {
                    _supplier = new EFSupplierRepository(_context, _supplierSortHelper, _httpContextAccessor);
                }

                return _supplier;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new EFUserRepository(_context, _userSortHelper, _httpContextAccessor, _userManager);
                }

                return _user;
            }
        }
        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new EFCategoryRepository(_context);
                }

                return _category;
            }
        }
        public ISubCategoryRepository SubCategory
        {
            get
            {
                if (_subcategory == null)
                {
                    _subcategory = new EFSubCategoryRepository(_context);
                }

                return _subcategory;
            }
        }
        public IPhotoRepository Photo
        {
            get
            {
                if (_photo == null)
                {
                    _photo = new EFPhotoRepository(_context);
                }

                return _photo;
            }
        }
        public IBranchRepository BranchLocation
        {
            get
            {
                if (_branch == null)
                {
                    _branch = new EFBranchRepository(_context);
                }

                return _branch;
            }
        }
        public IVendorRepository Vendor
        {
            get
            {
                if (_vendor == null)
                {
                    _vendor = new EFVendorRepository(_context);
                }

                return _vendor;
            }
        }
        public IUnitOfMeasurementRepository UnitOfMeasurement
        {
            get
            {
                if (_unitOfmeasurement == null)
                {
                    _unitOfmeasurement = new EFUnitOfMeasurementRepository(_context);
                }

                return _unitOfmeasurement;
            }
        }
        public IStoreRepository Store
        {
            get
            {
                if (_store == null)
                {
                    _store = new EFStoreRepository(_context);
                }

                return _store;
            }
        }
        public IStoreAreaRepository StoreArea
        {
            get
            {
                if (_storearea == null)
                {
                    _storearea = new EFStoreAreaRepository(_context);
                }

                return _storearea;
            }
        }
        public IRackRepository Rack
        {
            get
            {
                if (_rack == null)
                {
                    _rack = new EFRackRepository(_context);
                }

                return _rack;
            }
        }
        public IBinRepository Bin
        {
            get
            {
                if (_bin == null)
                {
                    _bin = new EFBinRepository(_context);
                }

                return _bin;
            }
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> VerifyBarCode(string barcode)
        {
            if (await _context.Products.AnyAsync(c => c.Barcode==barcode && c.IsDeleted == false))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> VerifyEmpCode(string empcode)
        {
            if (await _context.MI_Employer_Employees.AnyAsync(c => c.EPFNo == empcode && c.Deleted == 0))
            {
                return true;
            }
            return false;
        }
     

    }
}