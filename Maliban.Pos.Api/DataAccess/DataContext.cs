using System;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.DataAccess
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>,
                               ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //disable lazy loading
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseLazyLoadingProxies(); // Notice this change}
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.ApplicationRole).WithMany(r => r.ApplicationUserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.ApplicationUser).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });
            modelBuilder.Entity<ApplicationUser>(a =>
            {
                a.HasKey(q => q.Id);
                a.Property(b => b.RegDate).HasDefaultValueSql("CONVERT(date, GETDATE())");
                a.HasAlternateKey(c => c.Uid);
                //a.HasOne(s => s.BranchLocation).WithMany(u => u.ApplicationUsers).HasForeignKey(f => f.BranchLocationId);
                a.HasMany(d => d.SaleHeader).WithOne(u => u.ApplicationUser).IsRequired().OnDelete(DeleteBehavior.Restrict);
                a.HasOne(c => c.BranchLocation).WithMany(c => c.ApplicationUsers).HasForeignKey(c => c.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
            });
            modelBuilder.Entity<RolePermission>(a =>
      {
          a.HasOne(c => c.MainMenu).WithMany(c => c.RolePermissions).HasForeignKey(c => c.MainMenuId).IsRequired().OnDelete(DeleteBehavior.Restrict);
          //  a.HasOne(c => c.Submenu).WithMany(c => c.RolePermissions).HasForeignKey(c => c.SubmenuId).IsRequired().OnDelete(DeleteBehavior.Restrict);
          a.HasOne(c => c.SubmenuItem).WithMany(c => c.RolePermissions).HasForeignKey(c => c.SubmenuItemId).IsRequired().OnDelete(DeleteBehavior.Restrict);

      });
            modelBuilder.Entity<MainMenu>(b =>
        {
            b.HasMany(c => c.RolePermissions).WithOne(e => e.MainMenu);
        });
            /*                   modelBuilder.Entity<SubmenuItem>(b =>
                    {
                        b.HasMany(c => c.RolePermissions).WithOne(e => e.SubmenuItem);
                    }); */
            modelBuilder.Entity<SaleHeader>(a =>
               {
                   //a.HasOne(s => s.BranchLocation).WithMany(u => u.ApplicationUsers).HasForeignKey(f => f.BranchLocationId);
                   a.HasMany(d => d.Lines).WithOne(u => u.SaleHeader).IsRequired().OnDelete(DeleteBehavior.Restrict);
               });

            modelBuilder.Entity<SaleDetail>(a =>
           {
               a.Property(x => x.CreatedBy).IsRequired(false);
               a.HasOne(p => p.TransactionDetail).WithMany(cod => cod.SaleDetails).HasForeignKey(s => s.TransactionDetailId).IsRequired().OnDelete(DeleteBehavior.Restrict);
               a.Property(x => x.ModifiedBy).IsRequired(false);
               a.Property(a => a.Quantity).HasColumnType("decimal(18, 2)");
               a.Property(a => a.ReturnQuantity).HasColumnType("decimal(18, 2)");


           });
            // one to many relationship with StoreLoation and Product Table
            modelBuilder.Entity<BranchLocation>(b =>
            {
                b.HasMany(c => c.Store).WithOne(e => e.BranchLocation);
                b.HasMany(c => c.ApplicationUsers).WithOne(e => e.BranchLocation);
                b.HasMany(p => p.Suppliers).WithOne(cod => cod.BranchLocation).HasForeignKey(s => s.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Product>(b =>
            {
                b.HasMany(c => c.Prices).WithOne(e => e.Product).IsRequired().OnDelete(DeleteBehavior.Restrict);
                b.HasAlternateKey(c => c.ProductUid);
                b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
                b.HasQueryFilter(s => s.Active == true);
            });
            modelBuilder.Entity<Price>(p =>
                {
                    p.HasOne(c => c.Product).WithMany(c => c.Prices).IsRequired().OnDelete(DeleteBehavior.Restrict);
                    p.HasAlternateKey(c => c.PriceUid);
                    p.Property(b => b.UnitPrice).IsConcurrencyToken();
                    p.Property<byte[]>(b => b.RowVersion).IsRowVersion();
                    p.Property(a => a.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
                });
            modelBuilder.Entity<ProductCategory>(pc =>
            {
                pc.HasKey(p => new { p.ProductId, p.CategoryId });
                pc.HasOne(c => c.Product).WithMany(c => c.ProductCategory).HasForeignKey(c => c.ProductId);
                pc.HasOne(c => c.Category).WithMany(c => c.ProductCategory).HasForeignKey(c => c.CategoryId);
            });
            modelBuilder.Entity<Brand>(b =>
            {
                b.HasAlternateKey(c => c.BrandUid);
            });
            modelBuilder.Entity<CustomerOrderDetail>(c =>
            {
                c.HasKey(a => a.CustomerOrderDetailId);
                c.HasOne(p => p.CustomerOrderHeader).WithMany(cod => cod.CustomerOrderDetails).HasForeignKey(s => s.CustomerOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                c.HasOne(p => p.Price).WithMany(cod => cod.CustomerOrderDetails).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Supplier>(c =>
         {
             c.HasKey(a => a.SupplierId);
             c.HasOne(p => p.BranchLocation).WithMany(cod => cod.Suppliers).HasForeignKey(s => s.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
             c.HasQueryFilter(s => s.Active == true);
         });
            modelBuilder.Entity<CustomerReturnDetail>(c =>
           {
               c.HasKey(a => a.CustomerReturnDetailId);
               c.HasOne(p => p.CustomerReturnHeader).WithMany(cod => cod.Lines).HasForeignKey(s => s.CustomerReturnHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
               c.HasOne(p => p.CustomerReturnHeader).WithMany(cod => cod.Lines).HasForeignKey(s => s.CustomerReturnHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
               c.HasOne(p => p.Price).WithMany(cod => cod.CustomerReturnDetails).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
               c.Property(a => a.CurrentQuantity).HasColumnType("decimal(18, 2)");
               c.Property(a => a.BalanceQuantity).HasColumnType("decimal(18, 2)");
               c.Property(a => a.ReturnQuantity).HasColumnType("decimal(18, 2)");
               c.Property(a => a.SaleQuantity).HasColumnType("decimal(18, 2)");
           });
            modelBuilder.Entity<StockIssueDetail>(c =>
           {
               c.HasKey(a => a.StockIssueDetailId);
               c.HasOne(p => p.StockIssueHeader).WithMany(cod => cod.StockIssueDetail).HasForeignKey(s => s.StockIssueHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
               c.HasOne(p => p.GoodReceptNoteDetail).WithMany(cod => cod.StockIssueDetails).HasForeignKey(s => s.GoodReceptNoteDetailId).IsRequired().OnDelete(DeleteBehavior.Restrict);
           });
            modelBuilder.Entity<StockDetail>(c =>
        {
            c.HasKey(a => a.StockDetailId);
            c.HasOne(p => p.Price).WithMany(cod => cod.StockDetails).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            c.HasOne(p => p.Bin).WithMany(cod => cod.StockDetails).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            c.HasOne(p => p.GoodReceptNoteDetail).WithMany(cod => cod.StockDetails).HasForeignKey(s => s.GoodReceptNoteDetailId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

            modelBuilder.Entity<PurchaseReturnHeader>(c =>
          {
              c.HasKey(a => a.PurchaseReturnHeaderId);
              c.HasOne(p => p.GoodReceptNoteHeader).WithMany(e => e.PurchaseReturnHeaders).HasForeignKey(p => p.GoodReceptNoteHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;

              /*       c.HasOne(p => p.PurchaseOrderHeader).WithMany(cod => cod.PurchaseReturnHeaders).HasForeignKey(s => s.PurchaseOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict); */

          });
            modelBuilder.Entity<StockMovementHeader>(c =>
         {
             c.HasKey(a => a.StockMovementHeaderId);
             c.HasOne(p => p.Store).WithMany(cod => cod.StockMovementHeaders).HasForeignKey(s => s.StoreId).IsRequired().OnDelete(DeleteBehavior.Restrict);

         });
            modelBuilder.Entity<StockDetail>(c =>
       {
           c.HasKey(a => a.StockDetailId);
           c.HasOne(p => p.Store).WithMany(cod => cod.StockDetails).HasForeignKey(s => s.StoreId).IsRequired().OnDelete(DeleteBehavior.Restrict);

       });
            modelBuilder.Entity<StockIssueHeader>(c =>
       {
           c.HasKey(a => a.StockIssueHeaderId);
           c.HasOne(p => p.StoreArea).WithMany(cod => cod.StockIssueHeaders).HasForeignKey(s => s.StoreAreaId).IsRequired().OnDelete(DeleteBehavior.Restrict);
           c.HasOne(p => p.Store).WithMany(cod => cod.StockIssueHeaders).HasForeignKey(s => s.StoreId).IsRequired().OnDelete(DeleteBehavior.Restrict);

       });
            modelBuilder.Entity<Bin>(c =>
      {
          c.HasKey(a => a.BinId);
          c.HasOne(p => p.StoreArea).WithMany(cod => cod.Bins).HasForeignKey(s => s.StoreAreaId).IsRequired().OnDelete(DeleteBehavior.Restrict);
          c.HasOne(p => p.Store).WithMany(cod => cod.Bins).HasForeignKey(s => s.StoreId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      });
            modelBuilder.Entity<ShopStock>(c =>
        {
            c.HasKey(a => a.ShopStockId);
            c.HasOne(p => p.Price).WithMany(cod => cod.ShopStocks).HasForeignKey(s => s.ShopStockId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            c.HasQueryFilter(s => s.Active == true);
            c.Property(a => a.UnitsInStock).HasColumnType("decimal(18, 2)");
            c.Property(a => a.SaleQuantity).HasColumnType("decimal(18, 2)");

        });
            modelBuilder.Entity<PurchaseReturnDetail>(c =>
        {
            c.HasKey(a => a.PurchaseReturnDetailId);
            /*   c.HasOne(p => p.PurchaseOrderHeader).WithMany(cod => cod.PurchaseReturnDetails).HasForeignKey(s => s.PurchaseOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict); */
            c.HasOne(p => p.PurchaseReturnHeader).WithMany(cod => cod.Lines).HasForeignKey(s => s.PurchaseReturnHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            c.Property(a => a.FreeQuantity).HasColumnType("decimal(18, 2)");
            c.Property(a => a.ReturnQuantity).HasColumnType("decimal(18, 2)");

        });
            modelBuilder.Entity<PurchaseOrderDetail>(c =>
      {
          c.HasKey(a => a.PurchaseOrderDetailId);
          c.HasOne(p => p.PurchaseOrderHeaders).WithMany(pd => pd.PurchaseOrderDetails).HasForeignKey(p => p.PurchaseOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
          c.Property(a => a.ApproveQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.BonusQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.CurrentQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.FreeQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.GRNQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.Quantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.ReturnQuantity).HasColumnType("decimal(18, 2)");
      });
            modelBuilder.Entity<GoodReceptNoteDetail>(c =>
      {
          c.HasKey(a => a.GoodReceptNoteDetailId);
          c.HasOne(p => p.SupplierPrice).WithMany(pd => pd.GoodReceptNoteDetails).HasForeignKey(p => p.SupplierPriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
          c.Property(a => a.BalanceItemQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.FreeItemQuantity).HasColumnType("decimal(18, 2)");
          c.Property(a => a.ItemQuantity).HasColumnType("decimal(18, 2)");

      });
            modelBuilder.Entity<GoodReceptNoteHeader>(c =>
      {
          c.HasKey(a => a.GoodReceptNoteHeaderId);
          c.HasOne(p => p.BranchLocation).WithMany(pd => pd.GoodReceptNoteHeaders).HasForeignKey(p => p.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      });
            modelBuilder.Entity<StockTransferDetail>(c =>
            {
                c.HasKey(a => a.StockTransferDetailId);
                c.HasOne(p => p.StockTransferHeader).WithMany(pd => pd.Lines).HasForeignKey(p => p.StockTransferHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                c.Property(a => a.dQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dIssueQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dNetQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dTransferQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.ApproveQuantity).HasColumnType("decimal(18, 2)");

            });
            modelBuilder.Entity<StockTransferHeader>(c =>
            {
                c.HasKey(a => a.StockTransferHeaderId).Metadata.IsPrimaryKey();
                c.HasOne(p => p.BranchLocation).WithMany(pd => pd.StockTransferHeaders).HasForeignKey(p => p.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                c.HasOne(p => p.BranchLocation).WithMany(pd => pd.StockTransferHeaders).HasForeignKey(p => p.TransferBranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                //c.HasOne(p => p.Supplier).WithMany(pd => pd.StockTransferHeaders).HasForeignKey(p => p.SupplierId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                // c.HasOne(p => p.Supplier).WithMany(pd => pd.StockTransferHeaders).HasForeignKey(p => p.TransferSupplierId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<SupplierPrice>(c =>
      {
          c.HasKey(a => a.SupplierPriceId);
          c.HasOne(p => p.Product).WithMany(pd => pd.SupplierPrices).HasForeignKey(p => p.ProductId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      });
            modelBuilder.Entity<PurchaseRequestDetail>(c =>
           {
               c.HasKey(a => a.PurchaseRequestDetailId);
               c.HasOne(p => p.PurchaseOrderHeader).WithMany(pd => pd.PurchaseRequestDetails).HasForeignKey(p => p.PurchaseOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
           });
            modelBuilder.Entity<PurchaseOrderHeader>(c =>
      {
          c.HasKey(a => a.PurchaseOrderHeaderId);
          c.HasMany(p => p.PurchaseRequestDetails).WithOne(pd => pd.PurchaseOrderHeader).HasForeignKey(p => p.PurchaseOrderHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);

      });
            modelBuilder.Entity<StoreArea>(c =>
        {
            c.HasKey(a => a.StoreAreaId);
            c.HasOne(p => p.BranchLocation).WithMany(cod => cod.StoreAreas).HasForeignKey(s => s.BranchLocationId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });
            modelBuilder.Entity<Stock>(c =>
                {
                    c.HasKey(a => a.StockId);
                    c.HasOne(p => p.Price).WithMany(cod => cod.Stocks).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                    c.HasOne(p => p.Store).WithMany(cod => cod.Stocks).HasForeignKey(s => s.StockId).IsRequired().OnDelete(DeleteBehavior.Restrict);


                });
            modelBuilder.Entity<StockAdjustmentDetail>(c =>
              {
                  c.HasKey(a => a.StockAdjustmentDetailId);
                  c.HasOne(p => p.StockAdjustmentHeader).WithMany(pd => pd.Lines).HasForeignKey(p => p.StockAdjustmentHeaderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                  c.HasOne(p => p.Price).WithMany(pd => pd.StockAdjustmentDetails).HasForeignKey(p => p.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                  c.Property(p => p.CreatedBy).IsRequired(false);
                  c.Property(a => a.AdjustmentQuantity).HasColumnType("decimal(18, 2)");
                  c.Property(a => a.CurrentQuantity).HasColumnType("decimal(18, 2)");
                  c.Property(a => a.ApproveQuantity).HasColumnType("decimal(18, 2)");
                  c.Property(a => a.PreviousQuantity).HasColumnType("decimal(18, 2)");

              });
            #region
            /*           modelBuilder.Entity<StockTransferDetail>(c =>
                        {
                            c.HasKey(a => a.StockTransferDetailId);
                            c.HasOne(p => p.Price).WithMany(std => std.StockTransferDetails).HasForeignKey(p => p.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);

                        });*/
            #endregion
            modelBuilder.Entity<TransactionDetail>(c =>
            {
                c.HasKey(a => a.TransactionDetailId);
                c.HasOne(p => p.Price).WithMany(cod => cod.TransactionDetails).HasForeignKey(s => s.PriceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                c.Property(p => p.dNetValue).HasComputedColumnSql("[dNetQuantity]*[dCostPrice]");
                c.Property(p => p.dValue).HasComputedColumnSql("[dQuantity]*[dCostPrice]");
                c.Property(a => a.dQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dIssueQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dNetQuantity).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dIN).HasColumnType("decimal(18, 2)");
                c.Property(a => a.dOUT).HasColumnType("decimal(18, 2)");
                // c.Property(p => p.dNetValue).HasComputedColumnSql("Store.GetOrderTotal([ID])");

            });
            modelBuilder.Entity<SupplierProduct>(sp =>
            {
                sp.HasKey(bc => new { bc.ProductId, bc.SupplierId });
            });
            modelBuilder.Entity<SupplierProduct>()
                 .HasOne(bc => bc.Product)
                 .WithMany(b => b.SupplierProducts)
                 .HasForeignKey(bc => bc.ProductId);
            modelBuilder.Entity<SupplierProduct>()
                .HasOne(bc => bc.Supplier)
                .WithMany(c => c.SupplierProducts)
                .HasForeignKey(bc => bc.SupplierId);
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(SaleReturnTotalAmount), new[] { typeof(Int64) }))
          .HasName("SaleReturnUpdateSaleHeaderTotalAmount");
        }

        public decimal SaleReturnTotalAmount(Int64 id) => throw new NotSupportedException();
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerMasterSetting> CustomerMasterSettings { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<StoreArea> StoreAreas { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<BranchLocation> BranchLocations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }

        //public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseRequestHeader> PurchaseRequestHeaders { get; set; }
        public DbSet<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }
        public DbSet<SaleHeader> SaleHeaders { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<GoodReceptNoteHeader> GoodReceptNoteHeaders { get; set; }
        public DbSet<GoodReceptNoteDetail> GoodReceptNoteDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierPrice> SupplierPrices { get; set; }
        public DbSet<MI_Employer_Employee> MI_Employer_Employees { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        public DbSet<ShopStock> ShopStocks { get; set; }
        public DbSet<StockIssueHeader> StockIssueHeaders { get; set; }
        public DbSet<StockIssueDetail> StockIssueDetails { get; set; }
        public DbSet<StockAdjustmentHeader> StockAdjustmentHeaders { get; set; }
        public DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public DbSet<PurchaseReturnHeader> PurchaseReturnHeaders { get; set; }
        public DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public DbSet<CustomerReturnHeader> CustomerReturnHeaders { get; set; }
        public DbSet<CustomerReturnDetail> CustomerReturnDetails { get; set; }
        public DbSet<CustomerOrderHeader> CustomerOrderHeaders { get; set; }
        public DbSet<CustomerOrderDetail> CustomerOrderDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<RequestForQuotationHeader> RequestForQuotationHeaders { get; set; }
        public DbSet<RequestForQuotationDetail> RequestForQuotationDetails { get; set; }
        public DbSet<StockMovementHeader> StockMovementHeaders { get; set; }
        public DbSet<StockMovementDetail> StockMovementDetails { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<AdjustmentType> AdjustmentTypes { get; set; }
        public DbSet<TypeofGood> TypeofGoods { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<CalculationMaster> CalculationMasters { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReturnReason> ReturnReasons { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<Submenu> Submenus { get; set; }
        public DbSet<SubmenuItem> SubmenuItems { get; set; }
        public DbSet<MonthEndProcessDetail> MonthEndProcessDetails { get; set; }
        public DbSet<MonthEndProcessHeader> MonthEndProcessHeaders { get; set; }
        public DbSet<MI_Employer_Map> MI_Employer_Maps { get; set; }
        public DbSet<UserBranchLocation> UserBranchLocations { get; set; }

        //  public DbSet<MealOrderHeader> MealOrderHeaders { get; set; }
        // public DbSet<MealOrderDetail> MealOrderDetails { get; set; }
        public DbSet<MealOrder_Header> MealOrder_Headers { get; set; }

        public DbSet<StockTransferHeader> StockTransferHeaders { get; set; }
        public DbSet<StockTransferDetail> StockTransferDetails { get; set; }




    }
}