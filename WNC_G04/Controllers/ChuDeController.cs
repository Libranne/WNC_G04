using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WNC_G04.Models;

namespace WNC_G04.Controllers
{
    public class ChuDeController : Controller
    {
        private readonly DbG04RVContext _context;

        public ChuDeController(DbG04RVContext context)
        {
            _context = context;
        }

        // Câu 1: Hiển thị danh sách Chủ Đề
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChuDes.ToListAsync());
        }

        // Câu 3: Hiển thị form thêm mới Chủ Đề
        public IActionResult Create()
        {
            return View();
        }

        // Câu 3: Xử lý thêm mới Chủ Đề
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenChuDe,VerifyKey")] ChuDe chuDe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chuDe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chuDe); // Đảm bảo truyền model vào view
        }






        // Câu 4: Hiển thị thông tin Chủ Đề cần xóa
        [HttpDelete]
        [Route("ChuDe/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null)
            {
                return NotFound(new { message = "Không tìm thấy chủ đề với ID này." });
            }

            _context.ChuDes.Remove(chuDe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công!", id });
        }





        public async Task<IActionResult> Edit(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null)
            {
                return NotFound();
            }
            return View(chuDe); // Trả về View với đối tượng Chủ Đề để sửa
        }

        // Action POST: Xử lý lưu Chủ Đề sau khi sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaChuDe,TenChuDe,VerifyKey")] ChuDe chuDe)
        {
            if (id != chuDe.MaChuDe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuDe); // Cập nhật bản ghi
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuDeExists(chuDe.MaChuDe))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Sau khi lưu, chuyển về trang danh sách
            }
            return View(chuDe); // Nếu có lỗi, hiển thị lại form
        }

        private bool ChuDeExists(int id)
        {
            return _context.ChuDes.Any(e => e.MaChuDe == id);
        }




    }
}