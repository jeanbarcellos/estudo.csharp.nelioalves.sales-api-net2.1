using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentsController(SalesWebMvcContext context)
        {
            _context = context;
        }

        // GET: Departments
        // public async Task<List<Department>> Index()
        public async Task<IActionResult> Index()
        {
            var list = await _context.Department.ToListAsync();

            return Json(list);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id not provided" });
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound(new { message = "Id not found" });
            }

            return Ok(department);
        }

        // POST: Departments/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Add(department);
            await _context.SaveChangesAsync();

            // Um objeto Department é fornecido no corpo da resposta, juntamente com um cabeçalho de resposta Location contendo a URL do produto recém-criado.
            // return CreatedAtAction(nameof(Details), new { id = department.Id }, department);
            return Created(department.Id.ToString(), department);
        }

        // PUT: Departments/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(department);
        }

        // DELETE: Departments/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
