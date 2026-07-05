using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace carWorldDbFirstProject.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext dbcontext;
        public CarsController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index(string search)
        {
            var cars = dbcontext.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                cars = cars.Where(x =>
                    x.Marka.Contains(search) ||
                    x.Model.Contains(search));
            }

            ViewBag.Search = search;

            return View(cars.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cars cars)
        {

            dbcontext.Cars.Add(cars);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            var result = dbcontext.Cars.Find(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(Cars cars)
        {
            dbcontext.Update(cars);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var result = dbcontext.Cars.Find(id);

            if (result != null)
            {
                dbcontext.Cars.Remove(result);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult ExportToPdf()
        {
            var cars = dbcontext.Cars.ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Araç Listesi Raporu")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Marka").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Model").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Fiyat").Bold();
                            });

                            foreach (var item in cars)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Marka);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Model);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Fiyat.ToString());
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Arac_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            var cars = dbcontext.Cars.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Araç Listesi");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Marka";
                worksheet.Cells[1, 3].Value = "Model";
                worksheet.Cells[1, 4].Value = "Fiyat";
                worksheet.Cells[1, 5].Value = "Resim URL";

                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;

                foreach (var item in cars)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.Marka;
                    worksheet.Cells[rowNumber, 3].Value = item.Model;
                    worksheet.Cells[rowNumber, 4].Value = item.Fiyat;
                    worksheet.Cells[rowNumber, 5].Value = item.ImageUrl;

                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Arac_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}

