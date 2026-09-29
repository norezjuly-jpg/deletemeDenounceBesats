using Microsoft.AspNetCore.Mvc;
using DenounceBeasts.API.Models.Entities;

namespace DenounceBeasts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorsController : ControllerBase
    {
        private static readonly List<Sector> _sectors = new()
        {
            new Sector { Id = 1, Name = "Ensanche Ozama", MunicipalityId = 1, IsActive = true },
            new Sector { Id = 2, Name = "Piantini", MunicipalityId = 2, IsActive = true }
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(_sectors);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var sector = _sectors.FirstOrDefault(s => s.Id == id);
            return sector == null ? NotFound("Sector no encontrado.") : Ok(sector);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Sector sector)
        {
            if (string.IsNullOrWhiteSpace(sector.Name) || sector.MunicipalityId <= 0)
                return BadRequest("Nombre válido y MunicipalityId son requeridos.");

            sector.Id = _sectors.Any() ? _sectors.Max(s => s.Id) + 1 : 1;
            _sectors.Add(sector);
            return CreatedAtAction(nameof(GetById), new { id = sector.Id }, sector);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Sector updated)
        {
            var existing = _sectors.FirstOrDefault(s => s.Id == id);
            if (existing == null) return NotFound("Sector no encontrado.");

            if (string.IsNullOrWhiteSpace(updated.Name) || updated.MunicipalityId <= 0)
                return BadRequest("Datos del sector inválidos.");

            existing.Name = updated.Name;
            existing.MunicipalityId = updated.MunicipalityId;
            existing.IsActive = updated.IsActive;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _sectors.FirstOrDefault(s => s.Id == id);
            if (existing == null) return NotFound("Sector no encontrado.");

            _sectors.Remove(existing);
            return NoContent();
        }
    }
}
