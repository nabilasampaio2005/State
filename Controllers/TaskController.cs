using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using State.Data;
using State.Models;
using State.State.ENUM;

namespace State.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TasksController : Controller
	{
		private readonly StateDBContext _context;
		public TasksController(StateDBContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<ActionResult<TasksModel>> PostTask(TasksModel tasksmodel)
		{
			tasksmodel.State = StateTask.Created;
			_context.TasksModel.Add(tasksmodel);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTask", new { id = tasksmodel.Id }, tasksmodel);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TasksModel>> GetTask(int id)
		{
			var tasks = await _context.TasksModel.FindAsync(id);

			if (tasks == null)
			{
				return NotFound();
			}

			return tasks;
		}

		[HttpPut("{id}/start")]
		public async Task<IActionResult> StartTask(int id)
		{
			var tasks = await _context.TasksModel.FindAsync(id);

			if (tasks == null)
			{
				return NotFound();
			}

			// só altera se a tarefa estiver criada
			if (tasks.State == StateTask.Created)
			{
				tasks.State = StateTask.In_Progress;
				_context.Entry(tasks).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			else
			{
				return BadRequest("A tarefa não pode ser iniciada, tente novamente!");
			}

			return NoContent();
		}

		[HttpPut("{id}/complete")]
		public async Task<IActionResult> CompleteTask(int id)
		{
			var tasks = await _context.TasksModel.FindAsync(id);

			if (tasks == null)
			{
				return NotFound();
			}

			// só altera se a tarefa estiver em progresso
			if (tasks.State == StateTask.In_Progress)
			{
				tasks.State = StateTask.Completed;
				_context.Entry(tasks).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			else
			{
				return BadRequest("A tarefa não pode ser completada, tente novamente!");
			}

			return NoContent();
		}

		[HttpPut("{id}/cancel")]
		public async Task<IActionResult> CancelTask(int id)
		{
			var tasks = await _context.TasksModel.FindAsync(id);
			if (tasks == null)
			{
				return NotFound();
			}
			// Cancela a tarefa criada ou em progresso
			if (tasks.State == StateTask.Created || tasks.State == StateTask.In_Progress)
			{
				tasks.State = StateTask.Canceled;
				_context.Entry(tasks).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			else
			{
				return BadRequest("A tarefa não pode ser cancelada, tente novamente!");
			}

			return NoContent();
		}
	}
}