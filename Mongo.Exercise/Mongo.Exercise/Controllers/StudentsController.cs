using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.Exercise.Models;
using Mongo.Exercise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Exercise.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class StudentsController : ControllerBase
	{
		private readonly StudentService _studentService;

		public StudentsController(StudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Student>>> Get()
		{
			var students = await _studentService.GetStudentsAsync();
			return Ok(students);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Student>> Get(string id)
		{
			var student = await _studentService.GetStudentAsync(id);
			if (student == null)
				return NotFound();

			return Ok(student);
		}

		[HttpPost]
		public async Task<ActionResult<Student>> Post(Student student)
		{
			await _studentService.CreateStudentAsync(student);
			return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, Student updatedStudent)
		{
			var isUpdated = await _studentService.UpdateStudentAsync(id, updatedStudent);
			if (!isUpdated)
				return NotFound();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var isDeleted = await _studentService.DeleteStudentAsync(id);
			if (!isDeleted)
				return NotFound();

			return NoContent();
		}
	}
}


