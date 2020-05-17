using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExpensesController(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Expenses
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<EfExpense>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync().ConfigureAwait(false);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EfExpense>> GetExpense(long id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpGet("model/{modelId}")]
        public async Task<ActionResult<IEnumerable<EfExpense>>> GetExpenseForModel(long modelId)
        {
            var expense = await _context.Expenses
                .Where(e => e.ModelId == modelId)
                .ToListAsync().ConfigureAwait(false);

            return expense;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(long id, EfExpense expense)
        {
            if (expense is null)
                return BadRequest("expense is null.");
            if (id != expense.EfExpenseId)
            {
                return BadRequest("Id mismatch.");
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return BadRequest("ExpenseId not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Expenses
        /// <summary>
        /// Create a new expense for a model.
        /// </summary>
        /// <param name="newExpense"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EfExpense>> PostExpense(NewExpense newExpense)
        {
            if (newExpense is null)
            {
                return BadRequest("Data is missing");
            }

            var expense = _mapper.Map<EfExpense>(newExpense);
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction("GetExpense", new { id = expense.EfExpenseId }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EfExpense>> DeleteExpense(long id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return expense;
        }

        private bool ExpenseExists(long id)
        {
            return _context.Expenses.Any(e => e.EfExpenseId == id);
        }
    }
}
