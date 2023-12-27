using System.Net.Http;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using Maliban.Pos.Api.Models;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Maliban.Pos.Api.Helper
{
  public class PrintJob1
  {
    private PrintDocument PrintDocument;
    private Graphics graphics;
    //private Order order { set; get; }
    //private Shop shop { set; get; }
    //private int InitialHeight = 360;
    //Order order, Shop shop
    private Sale cart;
    private SaleHeader saleheader;
    private MI_Employer_Employee employee;
    private BranchLocation branchLocation;
     public PrintJob1(Sale cart, SaleHeader saleheader, MI_Employer_Employee employee, BranchLocation branchLocation)
    {
      //this.order = order;
      //this.shop = shop;
      this.cart = cart;
      this.saleheader = saleheader;
      this.employee = employee;
      this.branchLocation = branchLocation;
      AdjustHeight();
    }
    private string GetDefaultPrinter()
    {
      PrinterSettings settings = new PrinterSettings();
      foreach (string printer in PrinterSettings.InstalledPrinters)
      {
        settings.PrinterName = printer;
        if (settings.IsDefaultPrinter)
          return printer;
      }
      return string.Empty;
    }
    private void AdjustHeight()
    {
      //var capacity = 5 * order.ItemTransactions.Capacity;
      //InitialHeight += capacity;

      //capacity = 5 * order.DealTransactions.Capacity;
      //InitialHeight += capacity;
    }
    //public void Print(string printername)
    /*   public void GetAllPrinterList()
      {
        ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
        objScope.Connect();

        SelectQuery selectQuery = new SelectQuery();
        selectQuery.QueryString = "Select * from win32_Printer";
        ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
        ManagementObjectCollection MOC = MOS.Get();
        foreach (ManagementObject mo in MOC)
        {
          // lstPrinterList.Items.Add(mo["Name"].ToString());
        }
      } */
    /*   private List<string> GetPrinters()
    {
      List<string> printerNames = new List<string>();

      System.Management.ObjectQuery oquery =
          new System.Management.ObjectQuery("SELECT * FROM Win32_Printer");

      System.Management.ManagementObjectSearcher mosearcher =
          new System.Management.ManagementObjectSearcher(oquery);

      System.Management.ManagementObjectCollection moc = mosearcher.Get();

      foreach (ManagementObject mo in moc)
      {
        System.Management.PropertyDataCollection pdc = mo.Properties;
        foreach (System.Management.PropertyData pd in pdc)
        {
          if ((bool)mo["Network"])
          {
            printerNames.Add(Convert.ToString(mo[pd.Name]));
          }
        }
      }
      return printerNames;
    } */

    public void Print(string ipaddress)
    {
      PrintingPermission perm = new PrintingPermission(System.Security.Permissions.PermissionState.Unrestricted);
      perm.Level = PrintingPermissionLevel.AllPrinting;
      PrintDocument = new PrintDocument();
      //PrintDocument.PrinterSettings.PrinterName = printername;
      PrintDocument.PrinterSettings.PrinterName = $@"{ipaddress}";
      if(PrintDocument.PrinterSettings.IsValid)
      {
      //PrintDocument.PrinterSettings.PrinterName = $"{ipaddress}";
      PrintDocument.PrintPage += new PrintPageEventHandler(FormatPage);
      //PrintDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);
      PrintDocument.Print();
      }
    }
    void DrawAtStart(string text, int Offset)
    {
      int startX = 10;
      int startY = 5;
      Font minifont = new Font("Arial", 8);

      graphics.DrawString(text, minifont,
               new SolidBrush(Color.Black), startX + 5, startY + Offset);
    }
    void InsertItem(string key, string value, int Offset)
    {
      Font minifont = new Font("Arial", 7);
      int startX = 10;
      int startY = 5;

      graphics.DrawString(key, minifont,
                   new SolidBrush(Color.Black), startX + 5, startY + Offset);

      graphics.DrawString(value, minifont,
               new SolidBrush(Color.Black), startX + 160, startY + Offset);
    }
    void InsertHeaderStyleItem(string key, string value, int Offset)
    {
      int startX = 10;
      int startY = 5;
      Font itemfont = new Font("Arial", 8, FontStyle.Regular);

      graphics.DrawString(key, itemfont,
                   new SolidBrush(Color.Black), startX + 5, startY + Offset);

      graphics.DrawString(value, itemfont,
               new SolidBrush(Color.Gray), startX + 160, startY + Offset);
    }
    void DrawLine(string text, Font font, int Offset, int xOffset)
    {
      int startX = 10;
      int startY = 5;
      graphics.DrawString(text, font,
               new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
    }
    void DrawSimpleString(string text, Font font, int Offset, int xOffset)
    {
      int startX = 10;
      int startY = 5;
      graphics.DrawString(text, font,
               new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
    }
    private void FormatPage(object sender, PrintPageEventArgs e)
    {
      graphics = e.Graphics;
      Font minifont = new Font("Arial", 5);
      Font itemfont = new Font("Arial", 6);
      Font smallfont = new Font("Arial", 8);
      Font mediumfont = new Font("Arial", 10);
      Font largefont = new Font("Arial", 12);
      int Offset = 10;
      int smallinc = 10, mediuminc = 14, largeinc = 12;
      int startX = 5;
      int startY = 5;
      //Image image = Resources.logo;
      //e.Graphics.DrawImage(image, startX + 50, startY + Offset, 100, 30);

      graphics.DrawString($"{branchLocation?.BranchName}", mediumfont,
            new SolidBrush(Color.Black), startX + 45, startY + Offset);

      Offset = Offset + largeinc + 10;
      graphics.DrawString($"{branchLocation?.Address}", smallfont,
            new SolidBrush(Color.Black), startX + 15, startY + Offset);

      Offset = Offset + smallinc + 10;
      graphics.DrawString("Email : general@maliban.com", smallfont, new SolidBrush(Color.Black), startX + 30, startY + Offset);
      Offset = Offset + smallinc + 10;
      graphics.DrawString($"Phone  {branchLocation?.ContactNumbers}", smallfont,
      new SolidBrush(Color.Black), startX + 55, startY + Offset);

      Offset = Offset + smallinc + 10;

      String underLine = "";
      DrawLine(underLine, largefont, Offset, 0);

      Offset = Offset + mediuminc;
      DrawAtStart("Date: " + $"{saleheader.CreateDate}", Offset);
      Offset = Offset + mediuminc;
      DrawAtStart("Bill#: " + $"{saleheader.SaleHeaderId}", Offset);
      if (!String.Equals("", "N/A"))
      {
        Offset = Offset + mediuminc;
        DrawAtStart("Epf Number: " + $"{employee.Number}", Offset);
      }

      if (!String.Equals("", "N/A"))
      {
        Offset = Offset + mediuminc;
        DrawAtStart("Customer Name : " + $"{employee.FullName}", Offset);
      }
      Offset = Offset + mediuminc;
      DrawAtStart("Casher Name: " + $"{saleheader.CreatedBy}", Offset);
      Offset = Offset + mediuminc;
      Offset = Offset + largeinc;
      //Printe Table Headings
      e.Graphics.DrawString("Item Name", smallfont, Brushes.Black, 14, Offset, new StringFormat());
      e.Graphics.DrawString("Qty", smallfont, Brushes.Black, 150, Offset, new StringFormat());
      e.Graphics.DrawString("Price", smallfont, Brushes.Black, 190, Offset, new StringFormat());
      e.Graphics.DrawString("Total", smallfont, Brushes.Black, 220, Offset, new StringFormat());
      Offset = Offset + largeinc;
      //Printe Table Rows
      foreach (var item in cart.Lines)
      {
        SizeF qtyWidth = e.Graphics.MeasureString(item.Quantity.ToString(), itemfont);
        SizeF priceWidth = e.Graphics.MeasureString(item.Price.SalePrice.ToString(), itemfont);
        SizeF totalWidth = e.Graphics.MeasureString(item.Total.ToString(), itemfont);

        e.Graphics.DrawString(item.Product.ProductName.Length > 18 ? item.Product.ProductName.Substring(0, 18) : item.Product.ProductName.ToString(), itemfont, Brushes.Black, 14, Offset, new StringFormat());
        e.Graphics.DrawString(item.Quantity.ToString(), itemfont, Brushes.Black, 110 + (50 - qtyWidth.Width), Offset, new StringFormat());
        e.Graphics.DrawString(item.Price.SalePrice.ToString(), itemfont, Brushes.Black, 160 + (50 - priceWidth.Width), Offset, new StringFormat());
        e.Graphics.DrawString(item.Total.ToString(), itemfont, Brushes.Black, 200 + (50 - totalWidth.Width), Offset, new StringFormat());
        Offset += mediuminc;
      }
      underLine = "";
      DrawLine(underLine, largefont, Offset, 30);

      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Gross Amount: ", $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.SalePrice)}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Net Amount: ", $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.SalePrice)}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Number of Items/Qty Sold: ", $"{cart.Lines.Count}" + "/" + $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity))}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("This Bill Credit: ", $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.SalePrice)}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Your Credit Limit: ", $"{employee.CreditLimit}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Previous Credit: ", $"{employee.PreviousCreditBalance}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Total Credit: ", $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.SalePrice) + employee.PreviousCreditBalance}", Offset);
      Offset = Offset + largeinc;
      InsertHeaderStyleItem("Your Savings: ", $"{cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.MaximumRetailPrice) - cart.Lines.Sum(s => (s.Quantity - s.ReturnQuantity) * s.Price.SalePrice)}", Offset);
      Offset = Offset + largeinc;
      underLine = "-------------------------------------";
      DrawLine(underLine, largefont, Offset, 0);

      Offset = Offset + mediuminc;
      String greetings = "Thank You Come Again!!!";
      DrawSimpleString(greetings, mediumfont, Offset, 28);
      Offset = Offset + mediuminc;
      underLine = "-------------------------------------";
      DrawLine(underLine, largefont, Offset, 0);
      /* Offset += (2 * mediuminc);
        string tip = "TIP: -----------------------------";
        InsertItem(tip, "", Offset);
        Offset = Offset + largeinc;
        string DrawnBy = "Meganos Softwares: 0312-0459491 - OR - 0321-6228321";
        DrawSimpleString(DrawnBy, minifont, Offset, 15); */
    }
  }
}
//https://www.codemodes.com/print-receipt-on-thermal-printer/
//https://github.com/Robert-McGinley/TableParser

/*   private void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
      System.Drawing.Font headingFont = new System.Drawing.Font("Calibri", 13, System.Drawing.FontStyle.Bold);
      System.Drawing.Font boldFont = new System.Drawing.Font("Calibri", 11, System.Drawing.FontStyle.Bold);
      System.Drawing.Font normalFont = new System.Drawing.Font("Calibri", 7, System.Drawing.FontStyle.Regular);

      float topMargin = ev.MarginBounds.Top;
      float leftMargin = ev.MarginBounds.Left;

      //Create DataTable for Sales Details
      DataTable dt = new DataTable();
      dt.Columns.Add("Product");
      dt.Columns.Add("Price");
      dt.Columns.Add("Qty");
      dt.Columns.Add("Total");

      //Add Sales Details into DataTable
      DataRow dr = dt.NewRow();
      dr[0] = "SUNSILK THIK SND LONG  ";
      dr[1] = "2";
      dr[2] = "100.00";
      dr[3] = "200.00";
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr[0] = "SOAP MISUMI 4 PACK 360G ";
      dr[1] = "10";
      dr[2] = "50.00";
      dr[3] = "500.00";
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr[0] = "SCHOOL WATER BOTTLE";
      dr[1] = "10";
      dr[2] = "150.00";
      dr[3] = "1500.00";
      dt.Rows.Add(dr);

      string receipt_no = "10002";
      string receipt_date = "29-10-2021";
      string net_total = "2200.00";
      string line = "--------------------------------------------------------------------------------";
      float height = 30;


      //Print Company Name
      ev.Graphics.DrawString("SAMPLE BILL", headingFont, Brushes.Black, 10, height, new StringFormat());
      height += 30;
      //Print Company Address
      ev.Graphics.DrawString("4 th Cross Street, City - Pincode", normalFont, Brushes.Black, 20, height, new StringFormat());
      height += 40;

      //Print Receipt No
      ev.Graphics.DrawString("Receipt No : " + receipt_no, boldFont, Brushes.Black, 10, height, new StringFormat());
      //Print Receipt Date
      ev.Graphics.DrawString("Date : " + receipt_date, boldFont, Brushes.Black, 50, height, new StringFormat());
      height += 40;

      //Print Line
      ev.Graphics.DrawString(line, normalFont, Brushes.Black, 10, height, new StringFormat());
      height += 20;

      //Printe Table Headings
      ev.Graphics.DrawString("Description", normalFont, Brushes.Black, 10, height, new StringFormat());
      ev.Graphics.DrawString("Qty", normalFont, Brushes.Black, 100, height, new StringFormat());
      ev.Graphics.DrawString("Price", normalFont, Brushes.Black, 140, height, new StringFormat());
      ev.Graphics.DrawString("Total", normalFont, Brushes.Black, 170, height, new StringFormat());
      height += 10;

      //Print Line
      ev.Graphics.DrawString(line, normalFont, Brushes.Black, 10, height, new StringFormat());
      height += 10;

      //Printe Table Rows
      for (int i = 0; i < dt.Rows.Count; i++)
      {
        SizeF qtyWidth = ev.Graphics.MeasureString(dt.Rows[i][1].ToString(), normalFont);
        SizeF priceWidth = ev.Graphics.MeasureString(dt.Rows[i][2].ToString(), normalFont);
        SizeF totalWidth = ev.Graphics.MeasureString(dt.Rows[i][3].ToString(), normalFont);

        ev.Graphics.DrawString(dt.Rows[i][0].ToString(), normalFont, Brushes.Black, 10, height, new StringFormat());
        ev.Graphics.DrawString(dt.Rows[i][1].ToString(), normalFont, Brushes.Black, 150 + (50 - qtyWidth.Width), height, new StringFormat());
        ev.Graphics.DrawString(dt.Rows[i][2].ToString(), normalFont, Brushes.Black, 140 + (50 - priceWidth.Width), height, new StringFormat());
        ev.Graphics.DrawString(dt.Rows[i][3].ToString(), normalFont, Brushes.Black, 170 + (50 - totalWidth.Width), height, new StringFormat());
        height += 15;
      }
      //Print Line
      ev.Graphics.DrawString(line, normalFont, Brushes.Black, 10, height, new StringFormat());
      height += 20;

      //Print Net Total
      ev.Graphics.DrawString("Total", normalFont, Brushes.Black, 220, height, new StringFormat());

      SizeF netWidth = ev.Graphics.MeasureString(net_total, normalFont);
      ev.Graphics.DrawString(net_total, normalFont, Brushes.Black, 320 + (50 - netWidth.Width), height, new StringFormat());
      height += 20;

      //Print Line
      ev.Graphics.DrawString(line, normalFont, Brushes.Black, 10, height, new StringFormat());
      height += 40;

      ev.Graphics.DrawString("!!! THANK YOU !!!", headingFont, Brushes.Black, 130, height, new StringFormat());

      ev.HasMorePages = false;
    } */