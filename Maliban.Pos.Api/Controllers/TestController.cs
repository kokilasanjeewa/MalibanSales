using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]

    public class TestController : BaseController
    {
        #region SaleConstructor
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IOperationGroupRepository _op;
        private Sale cart;

        // private IDistributedCache cache;

        [Obsolete]
        public TestController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, Sale cartService, IDistributedCache cache,IOperationGroupRepository op) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            cart = cartService;
            _op = op;
            //  this.cache = cache;
        }
        #endregion
        /*   
          [HttpPost("savetosession")]

          public async Task<IActionResult> SaveToSession([FromForm] CreateSaleDetailDto createSaleItemDetailsDto)
          {
            if (!ModelState.IsValid)
            {
              return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
              var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
              product.Price = product.Price.Where(p => p.Active == true && p.IsDeleted == false).ToList<Price>();
              cart.AddItem(product, createSaleItemDetailsDto.quantity);
              HttpContext.Session.SetJson<Sale>("Test", cart);
            }
            else
            {
              return NotFound("Barcode doesn't exists.");
            }
            return Content($"save to session");
          }
          */
          [HttpGet]
          [Route("fetch-from-session")]
          public IActionResult FetchFromSession()
          {
           // Sale cart = HttpContext.Session.GetJson<Sale>("Test") ?? new Sale();
            _op.InsertIntoEmployee();
            return Ok();
          } 
        [HttpGet]
        public IActionResult Get()
        {
          // first method
            var client = new HttpClient();
           string[] svm = {"DG-23-000266","MG-23-001354"};
          // String[] svm = new string[2] {"DG-23-000266", "PM-23-000016"}; 
            var jsonObject = JsonConvert.SerializeObject(svm);
            client.DefaultRequestHeaders.Add("X-SampleAppApiKey", "SampleAppX123:YesAppKeyIsPersist");
          //  HttpResponseMessage response = client.GetAsync("http://203.115.20.74:5008/api/GetAEPOs?fromDate=2022-07-05&toDate=2022-07-08").Result;
        //  HttpResponseMessage response = client.GetAsync("http://203.115.20.74:5008/api/GetAEPOs").Result;
      //  HttpResponseMessage response = client.GetAsync("http://203.115.20.74:5008/api/GetAEPOs/PurchaseOrder/GetOrderAsync?purchaseOrderNumber=MG-23-001354").Result;
              //  HttpResponseMessage response = client.GetAsync("http://203.115.20.74:5008/api/GetAEPOs/PurchaseOrder/GetOrderAsync?purchaseOrderNumber="+ svm[0]).Result;

           HttpResponseMessage response = client.PostAsync("https://localhost:44307/api/GetAEPOs/PostOrdersIsSendToSupplieAsync",new StringContent(jsonObject.ToString(), System.Text.Encoding.UTF8, "application/json")).Result;
                 var result =response.Content.ReadAsStringAsync().Result;
               //     HttpResponseMessage response = client.PostAsync("https://localhost:44307/api/GetAEPOs",new StringContent(jsonObject.ToString(), System.Text.Encoding.UTF8, "application/json")).Result;

           // secound method
           //  var httpResponseMessage = new HttpResponseMessage {};  
           /*  var httpRequestMessage = new HttpRequestMessage
            // {
            //     // post then change Post
                  Method = HttpMethod.Post, 
                  RequestUri = new Uri("https://localhost:44307/api/GetAEPOs/PostOrdersIsSendToSupplieAsync"),
                 Headers = {
                    { HttpRequestHeader.Authorization.ToString(), "Bearer xxxxxxxxxxxxxxxxxxx" },
                    { HttpRequestHeader.Accept.ToString(), "application/xml" },
                    { "X-SampleAppApiKey", "SampleAppX123:YesAppKeyIsPersist" }
                  },
                  Content = new StringContent(JsonConvert.SerializeObject(svm)) 

          /*       Method = HttpMethod.Get, 
                RequestUri = new Uri("http://203.115.20.74:5008/api/GetAEPOs"),
                Headers = {
                            { HttpRequestHeader.Authorization.ToString(), "Bearer xxxxxxxxxxxxxxxxxxx" },
                            { HttpRequestHeader.Accept.ToString(), "application/xml" },
                            { "X-SampleAppApiKey", "SampleAppX123:YesAppKeyIsPersist" }
                          }, */
           // };*/
//             var formdata = new MultipartFormDataContent();
// formdata.Add(new StringContent("value1"), "key1");
// formdata.Add(new StringContent("value2"), "key2");
          //  client.SendAsync(httpRequestMessage);
          //  var result =response.Content.ReadAsStringAsync().Result;
            return Ok(result);
        }
    }
}
