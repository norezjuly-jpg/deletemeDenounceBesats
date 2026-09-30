using Microsoft.AspNetCore.Mvc;
using DenounceBeasts.API.Models.Entities;

namespace DenounceBeasts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalitiesController : ControllerBase
    {
        private static readonly List<Municipality> _municipalities = new()
        {
            new Municipality { Id = 1, Name = "Santo Domingo Este", PostalCode = "11501", IsActive = true },
            new Municipality { Id = 2, Name = "Distrito Nacional", PostalCode = "10101", IsActive = true }
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(_municipalities);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _municipalities.FirstOrDefault(m => m.Id == id);
            return item == null ? NotFound("Municipio no encontrado.") : Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Municipality municipality)
        {
            if (string.IsNullOrWhiteSpace(municipality.Name))
                return BadRequest("El nombre es obligatorio.");

            municipality.Id = _municipalities.Any() ? _municipalities.Max(m => m.Id) + 1 : 1;
            _municipalities.Add(municipality);
            return CreatedAtAction(nameof(GetById), new { id = municipality.Id }, municipality);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Municipality updated)
        {
            var existing = _municipalities.FirstOrDefault(m => m.Id == id);
            if (existing == null) return NotFound("Municipio no encontrado.");

            if (string.IsNullOrWhiteSpace(updated.Name))
                return BadRequest("El nombre no puede estar vacío.");

            existing.Name = updated.Name;
            existing.PostalCode = updated.PostalCode;
            existing.IsActive = updated.IsActive;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _municipalities.FirstOrDefault(m => m.Id == id);
            if (existing == null) return NotFound("Municipio no encontrado.");

            _municipalities.Remove(existing);
            return NoContent();
        }
    }
}
