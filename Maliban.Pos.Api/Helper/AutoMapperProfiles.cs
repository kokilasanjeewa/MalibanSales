using System.Linq;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using AutoMapper;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Service;
using Maliban.Pos.Api.DummyData;

namespace Maliban.Pos.Api.Helper
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<UserForRegisterDto, ApplicationUser>();
      CreateMap<ApplicationUser, UserForLogInDto>();
      CreateMap<ApplicationUser, UserForListDto>();
      CreateMap<ApplicationUser, UserForDetailedDto>();
      CreateMap<CreateProductDto, Product>();
      CreateMap<UpdateProductDto, Product>().ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.FromBase64String(src.RowVersion)));
      CreateMap<Product, UpdateProductDto>().ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));
      CreateMap<Product, ProductListDto>();
      CreateMap<MI_Employer_Employee, EmployeeDto>();
      CreateMap<CreatePriceDto, Price>();
      CreateMap<UpdatePriceDto, Price>();
      CreateMap<CreateBrandDto, Brand>();
      CreateMap<CreateSupplierDto, Supplier>();
      CreateMap<CreateVendorDto, Vendor>();
      CreateMap<CreatePhotoDto, Photo>();
      CreateMap<CreateCategoryDto, Category>();
      CreateMap<CreateSubCategoryDto, SubCategory>();
      CreateMap<CreateBranchLocationDto, BranchLocation>();
      CreateMap<CreateUnitOfMeasurementDto, UnitOfMeasurement>();
      CreateMap<UpdateSupplierDto, Supplier>();
      CreateMap<UpdateCustomerDto, MI_Employer_Employee>();
      CreateMap<UpdateVendorDto, Vendor>();
      CreateMap<UpdatePhotoDto, Photo>();
      CreateMap<UpdateCategoryDto, Category>();
      CreateMap<UpdateSubCategoryDto, SubCategory>();
      CreateMap<UpdateLocationDto, BranchLocation>();
      CreateMap<UpdateUnitOfMeasurementDto, UnitOfMeasurement>();
      CreateMap<Supplier, SuppliersDto>();
      CreateMap<Category, CategoriesDto>();
      CreateMap<Vendor, VendorsDto>();
      CreateMap<SubCategory, SubCategoriesDto>();
      CreateMap<Brand, BrandsDto>();
      CreateMap<UnitOfMeasurement, UomsDto>();
      CreateMap<Supplier, SupplierListDto>().ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityNameEn)).ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.DistrictNameEn)).ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Provinces.ProvinceNameEn)).ForMember(dest => dest.TitleName, opt => opt.MapFrom(src => src.Title.TitleName)).ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.ToString())).ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(p => p.Url).ToArray()));
      CreateMap<Brand, BrandListDto>();
      CreateMap<Vendor, VendorListDto>();
      CreateMap<UnitOfMeasurement, UnitOfMeasurementListDto>();
      CreateMap<BranchLocation, BranchLocationListDto>();
      CreateMap<GoodReceptNoteHeader, GRNListDto>();
      CreateMap<PurchaseReturnHeader, PrnListDto>();
      CreateMap<UpdateBrandDto, Brand>()
      .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
      .ForMember(dest => dest.BrandDescription, opt => opt.MapFrom(src => src.BrandDescription))
      .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));
      CreateMap<ProvinceData, Province>();
      CreateMap<DistrictData, District>();
      CreateMap<CitiesData, City>();
      CreateMap<Province, ProvinceListDto>();
      CreateMap<District, DistrictListDto>();
      CreateMap<MonthEndProcessHeader, ReportDate>();
      CreateMap<City, CityListDto>();
      CreateMap<CreateTitleDto, Title>();
      CreateMap<CreatePaymentMethodDto, PaymentMethod>();
      CreateMap<CreateTypeofGoodDto, TypeofGood>();
      CreateMap<Department, DepartmentListDto>();
      CreateMap<CreateDepartmentDto, Department>();
      CreateMap<Section, SectionListDto>();
      CreateMap<ReturnReason, ReturnReasonListDto>();
      CreateMap<CreateSectionDto, Section>();
      CreateMap<CreatePurchaseRequestHeaderDto, PurchaseRequestHeader>();
      CreateMap<CreatePurchaseReturnHeaderDto, PurchaseReturnHeader>();
      CreateMap<CreateSupplierProductDto, SupplierProduct>();
      CreateMap<DeleteSupplierProductDto, SupplierProduct>();
      CreateMap<CreatePurchaseOrderHeaderDto, PurchaseOrderHeader>();
      CreateMap<UpdatePurchaseOrderHeaderDto, PurchaseOrderHeader>();
      CreateMap<CreateGrnHeaderDto, GoodReceptNoteHeader>();
      CreateMap<CreateSupplierPriceDto, SupplierPrice>();
      CreateMap<CreatePaymentDto, Payment>();
      CreateMap<CreateSaleHeaderDto, SaleHeader>();
      CreateMap<CreateSaleReturnHeaderDto, CustomerReturnHeader>();
      CreateMap<CreateStockAdjustmentHeaderDto, StockAdjustmentHeader>();
      CreateMap<UpdateStockAdjustmentHeaderDto, StockAdjustmentHeader>();
      CreateMap<SupplierPrice, SupplierPriceListDto>();
      CreateMap<PurchaseOrderDetail, PurchaseOrderItemDetailsDto>();
      CreateMap<Report, ReportListDto>();
      CreateMap<CompanyMasterSettingDto, CalculationMaster>();
      CreateMap<CalculationMaster, CompanyMasterSettingListDto>();
      CreateMap<CreateCustomerMasterSettingDto, CustomerMasterSetting>();
      CreateMap<UpdateCompanyMasterSettingDto, CalculationMaster>();
      CreateMap<CustomerMasterSetting, CustomerMasterSettingListDto>();
      CreateMap<UpdateCustomerMasterSettingDto, CustomerMasterSetting>();
      CreateMap<CreateMealOrderHeaderDto, MealOrderHeader>().ForMember(dest => dest.MealType, opt => opt.MapFrom(src => (MealType)src.MealTypeId));
      CreateMap<CreateStockTransferHeaderDto, StockTransferHeader>();

        }
    }
}