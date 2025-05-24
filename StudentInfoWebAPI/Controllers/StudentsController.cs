using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInfoWebAPI.Data;
using StudentInfoWebAPI.Models;
using System.Text.Json;

namespace StudentInfoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string jsonFilepath = "Data/Students.json";
        private StudentDbContext _studentDbContext;

        public StudentsController(StudentDbContext studentDbContext) 
        {
           _studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
           // var jsonString = File.ReadAllText(jsonFilepath);
            //var students = JsonSerializer.Deserialize(jsonString);

            if (_studentDbContext.Students == null)
            {
                return NotFound();
            }
            return await _studentDbContext.Students.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id) 
        {
           if(_studentDbContext.Students == null)
           {
                return NotFound();
           }

            var student = await _studentDbContext.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }

            return student;

        
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student) 
        {
           _studentDbContext.Students.Add(student);
            await _studentDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudents), new {id= student.Id}, student);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            if (_studentDbContext.Students == null)
            {
                return NotFound();
            }

            var student = await _studentDbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _studentDbContext.Students.Remove(student);
            await _studentDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
