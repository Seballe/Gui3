using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobsController(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EfJob>>> GetJobs()
        {
            var modelStr = User.Claims.First(a => a.Type == "ModelId").Value;
            long modelId;
            if (!long.TryParse(modelStr, out modelId))
                return Unauthorized("ModelId missing");
            if (modelId < 0)
            {
                // Is manager
                return await _context.Jobs.ToListAsync().ConfigureAwait(false);
            }
            else
            {
                return await _context.JobModels
                    .Where(jm => jm.EfModelId == modelId)
                    .Include(r => r.Job)
                    .Select(s => s.Job)
                    .ToListAsync().ConfigureAwait(false);
            }
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(long id)
        {
            var job = await _context.Jobs.Where(j => j.EfJobId == id)
                .Include(j => j.JobModels)
                .ThenInclude(jm => jm.Model)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (job == null)
            {
                return NotFound();
            }

            var jobDto = _mapper.Map<Job>(job);

            foreach (var jobModel in job.JobModels)
            {
                var modelDto = _mapper.Map<Model>(jobModel.Model);
                jobDto.Models.Add(modelDto);
            }

            return jobDto;
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutJob(long id, NewJob newJob)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id).ConfigureAwait(false);
                job.Comments = newJob.Comments;
                job.Customer = newJob.Customer;
                job.Days = newJob.Days;
                job.Location = newJob.Location;
                job.StartDate = newJob.StartDate;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        /// <summary>
        /// Create a new job
        /// </summary>
        /// <param name="newJob"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Job>> PostJob(NewJob newJob)
        {
            var job = _mapper.Map<EfJob>(newJob);
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            var jobDto = _mapper.Map<Job>(job);
            return CreatedAtAction("GetJob", new { id = job.EfJobId }, jobDto);
        }

        // POST: api/Jobs
        /// <summary>
        /// Add model to job.
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="modelId">modelId</param>
        /// <returns></returns>
        [HttpPost("{jobId}/model/{modelId}")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<EfJob>> AddModelToJob(long jobId, long modelId)
        {
            var job = await _context.Jobs.FindAsync(jobId).ConfigureAwait(false);
            if (job == null)
            {
                ModelState.AddModelError("jobId", "jobId not found");
                return BadRequest(ModelState);
            }
            var model = await _context.Models.FindAsync(modelId).ConfigureAwait(false);
            if (model == null)
            {
                ModelState.AddModelError("modelId", "modelId not found");
                return BadRequest(ModelState);
            }
            _context.Entry(job)
                .Collection(j => j.JobModels)
                .Load();
            job.JobModels.Add(new EfJobModel()
            {
                Job = job,
                Model = model
            });

            await _context.SaveChangesAsync();

            var jobDto = _mapper.Map<Job>(job);
            foreach (var jobModel in job.JobModels)
            {
                var modelDto = _mapper.Map<Model>(jobModel.Model);
                jobDto.Models.Add(modelDto);
            }


            return Created(job.EfJobId.ToString(), jobDto);
        }

        // DELETE: api/Jobs/1/model/3.
        /// <summary>
        /// Removes the model from the job.
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="modelId">ModelId</param>
        /// <returns></returns>
        [HttpDelete("{jobId}/model/{modelId}")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<EfJob>> RemoveModelFromJob(long jobId, long modelId)
        {
            var jobModel = await _context.JobModels.Where(
                jm => jm.EfJobId == jobId && jm.EfModelId == modelId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            if (jobModel == null)
            {
                ModelState.AddModelError("jobId", "jobId or modelId not found");
                return BadRequest(ModelState);
            }

            _context.JobModels.Remove(jobModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                ModelState.AddModelError("jobId", "jobId or modelId not found");
                return BadRequest(ModelState);
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.EfJobId == id);
        }
    }
}
