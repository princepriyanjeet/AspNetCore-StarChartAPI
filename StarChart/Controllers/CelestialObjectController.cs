using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}",Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = this._context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            var satellites = this._context.CelestialObjects.Where(s => s.OrbitedObjectId == id).ToList();
            celestialObject.Satellites = satellites;
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = this._context.CelestialObjects.Where(c=>c.Name==name).ToList();
            if (celestialObjects == null || celestialObjects.Count==0)
            {
                return NotFound();
            }

            foreach (var item in celestialObjects)
            {
                var satellites = this._context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
                item.Satellites = satellites;
            }

            
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = this._context.CelestialObjects.ToList();
            foreach (var item in celestialObjects)
            {
                var satellites = this._context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
                item.Satellites = satellites;
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestial)
        {
            this._context.CelestialObjects.Add(celestial);
            this._context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestial.Id },celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestial)
        {
            var celestialObject = this._context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Name = celestial.Name;
            celestialObject.OrbitalPeriod = celestial.OrbitalPeriod;
            celestialObject.OrbitedObjectId = celestial.OrbitedObjectId;
            this._context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id,string name)
        {
            var celestialObject = this._context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Name =name;
            this._context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObject = this._context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            this._context.Remove(celestialObject);
            this._context.SaveChanges();
            return NoContent();
        }
    }
}
