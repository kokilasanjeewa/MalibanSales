using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Maliban.Pos.Api.Service;
using System.IO;

namespace Maliban.Pos.Api.DummyData
{
  public class SeedData
  {
    public static async Task Initialize(UserManager<ApplicationUser> userManager, DataContext context,
                             RoleManager<ApplicationRole> roleManager, IMapper _map)
    {
      context.Database.EnsureCreated();

      string role1 = "Administrator";
      string desc1 = "This is the administrator role";

      string role2 = "Manager";
      string desc2 = "This is the managers role";

      string role3 = "User";
      string desc3 = "This is the user role";

      string password = "kK.12345678";

      if (await roleManager.FindByNameAsync(role1) == null)
      {
        await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
      }
      if (await roleManager.FindByNameAsync(role2) == null)
      {
        await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
      }
      if (await roleManager.FindByNameAsync(role3) == null)
      {
        await roleManager.CreateAsync(new ApplicationRole(role3, desc3, DateTime.Now));
      }
      if (await userManager.FindByNameAsync("kokila.sanjeewa@gmail.com") == null)
      {

        var branch = new BranchLocation
        {
          BranchCode = "AD",
          BranchName = "Admin Only",
          Address = "Admin Only",
          ContactNumbers = "Admin Only",
          CreateDate = DateTime.Now,
          CreatedBy = "Administrator"

        };
        var createBranch = _map.Map<BranchLocation>(branch);
        await context.BranchLocations.AddAsync(createBranch);
        await context.SaveChangesAsync();
        var user = new ApplicationUser
        {
          UserName = "kokila.sanjeewa@gmail.com",
          Email = "kokila.sanjeewa@gmail.com",
          FirstName = "Kokila",
          MiddleName = "Sanjeewa",
          LastName = "Perera Wejayarathne ",
          SurName = "Attapattu Mudalige",
          PhoneNumber = "0728993455",
          NationId = "780081155V",
          BranchLocationId = createBranch.BranchLocationId,
          Password = password
        };

        var result = await userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          await userManager.AddPasswordAsync(user, password);
          await userManager.AddToRoleAsync(user, role1);
          await userManager.AddToRoleAsync(user, role2);
        }
      }
      if (await userManager.FindByNameAsync("kokila.sanjeewa@maliban.com") == null)
      {
        var user = new ApplicationUser
        {
          UserName = "kokila.sanjeewa@maliban.com",
          Email = "kokila.sanjeewa@maliban.com",
          FirstName = "Kokila",
          MiddleName = "Sanjeewa",
          LastName = "Perera Wejayarathne ",
          SurName = "Attapattu Mudalige",
          PhoneNumber = "0728993455",
          NationId = "780081155V",
          BranchLocationId = 1,
          Password = password
        };

        var result = await userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          await userManager.AddPasswordAsync(user, password);
          await userManager.AddToRoleAsync(user, role3);
        }
      }
    }
    public static async Task InitializeDb(DataContext context, IConfiguration _config, IMapper _map)
    {
      context.Database.EnsureCreated();
      if (!context.Countries.Any(c => c.ISO == "LK"))
      {
        var country = new Country
        {
          ISO = "LK",
          ISO3 = "LKA",
          NiceName = "Sri Lanka",
          CountryName = "SRI LANKA",
          CountryCode = "144",
          PhoneCode = "94",
          CreateDate = DateTime.Now,
          CreatedBy = "Administrator",
          IsDeleted = false,
          Active = true
        };
        await context.Countries.AddAsync(country);
      }
      var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"data\\cities.json"}");
      var JSON = System.IO.File.ReadAllText(folderDetails);
      var root = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(JSON);
      var CitiesData = _map.Map<IEnumerable<City>>(root.CitiesData);
      await context.Cities.AddRangeAsync(CitiesData);
      await context.SaveChangesAsync();
    }

  }
  public class CitiesData
  {
    public int DistrictCode { get; set; }
    public string CityNameEn { get; set; }
    public string CityNameSl { get; set; }
    public string CityNameTm { get; set; }
    public string PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CreatedBy { get; set; }
  }
  public class DistrictData
  {
    public string CreatedBy { get; set; }
    public string ProvinceCode { get; set; }
    public string DistrictCode { get; set; }
    public string DistrictNameEn { get; set; }
    public string DistrictNameSl { get; set; }
    public string DistrictNameTm { get; set; }
  }
  public class ProvinceData
  {

    public string CreatedBy { get; set; }
    public int ProvinceCode { get; set; }
    public string ProvinceNameEn { get; set; }
    public string ProvinceNameSl { get; set; }
    public string ProvinceNameTm { get; set; }
  }
  public class RootObject
  {
    public List<CitiesData> CitiesData { get; set; }
    public List<ProvinceData> ProvinceData { get; set; }
    public List<DistrictData> DistrictData { get; set; }

  }
}
/*    var provinces = _config.GetSection("ProvinceData").Get<ProvinceData[]>();
          var provincesData = _map.Map<IEnumerable<Province>>(provinces);
          await context.Provinces.AddRangeAsync(provincesData);
          var districts = _config.GetSection("DistrictData").Get<DistrictData[]>();
          var districtsData = _map.Map<IEnumerable<District>>(districts);
          await context.Districts.AddRangeAsync(districtsData);
          var Cities = _config.GetSection("CitiesData").Get<CitiesData[]>();
          var CitiesData = _map.Map<IEnumerable<City>>(Cities);
          await context.Cities.AddRangeAsync(CitiesData); 
    if (!context.Countries.Any(c => c.ISO == "LK"))
    {
      var country = new Country
      {
        ISO = "LK",
        ISO3 = "LKA",
        NiceName = "Sri Lanka",
        CountryName = "SRI LANKA",
        CountryCode = "144",
        PhoneCode = "94",
        CreateDate = DateTime.Now,
        CreatedBy = "Administrator",
        IsDeleted = false,
        Active = true
      };
      await context.Countries.AddAsync(country);
    }
    await context.SaveChangesAsync(); */