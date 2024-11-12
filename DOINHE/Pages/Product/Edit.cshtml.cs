using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOINHE.Db;
using DOINHE.Entitys;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace DOINHE.Pages.Product
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public EditModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DTO.ProductDTO Product { get; set; } = default!;
        public byte[] ImgDescriptionPages { get; set; }
        public byte[] ImgKeyPages { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = new DTO.ProductDTO
            {   Id = product.Id,
                ProductName = product.ProductName!,
                CategoryId = product.CategoryId!.Value,
                UserId = product.UserId,
                DateTimeStart = product.DateTimeStart!.Value,
                DateTimeEnd = product.DateTimeEnd!.Value,
                CreateDate = product.CreateDate,
                Price = product.Price!.Value,
                Address = product.Address!,
                Description = product.Description!,
                StatusIsBuy = product.StatusIsBuy,
                StatusIsApprove = product.StatusIsApprove, 
                ImgDescription = Static.ConvertToByte.ConvertByteToIFormFile(product.ImgDescription!, nameof(product.ImgDescription)),
                Key = product.Key!,
                ImgKey = Static.ConvertToByte.ConvertByteToIFormFile(product.ImgKey!, nameof(product.ImgKey))
            };
            ImgDescriptionPages = product.ImgDescription!;
                ImgKeyPages = product.ImgKey!;
           ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == Product.Id);
            if (!ModelState.IsValid)
            {
                var allKeys = ModelState
    .Where(ms => ms.Value.Errors.Count > 0)
    .Select(ms => ms.Key)
    .ToList();


                // Kiểm tra xem có bất kỳ khóa nào khác ngoài 2 trường cụ thể không
                bool hasOtherErrors = allKeys.Any(key => key != "Product.ImgDescription" && key != "Product.ImgKey");
                if (hasOtherErrors)
                {
                    return Page();
                }
               


            }



            product.Id = Product.Id;
            product.ProductName = Product.ProductName;
            product.CategoryId = Product.CategoryId;
            product.UserId = Product.UserId;
                product.DateTimeStart = Product.DateTimeStart;
            product.DateTimeEnd = Product.DateTimeEnd;
            product.CreateDate = Product.CreateDate;
            product.Price = Product.Price;
                product.Address = Product.Address;
            product.Description = Product.Description;
            product.StatusIsBuy = Product.StatusIsBuy;
                product.StatusIsApprove = false;


            product.Key = Product.Key;
           
            
            if (Product.ImgDescription == null) { }
            else product.ImgDescription = Static.ConvertToByte.ConvertIFormFileToByte(Product.ImgDescription!);

            if (Product.ImgKey == null) {  }
            else product.ImgKey = Static.ConvertToByte.ConvertIFormFileToByte(Product.ImgKey!);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
