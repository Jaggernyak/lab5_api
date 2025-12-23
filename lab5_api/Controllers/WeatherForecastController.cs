using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace lab5_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NamesController : ControllerBase
    {
        private static List<string> _names = new List<string> { "Bayne", "David", "Vanessa", "Alvaro" };

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? sortStrategy = null)
        {
            IEnumerable<string> result = _names;

            if (sortStrategy.HasValue)
            {
                switch (sortStrategy.Value)
                {
                    case 1:
                        result = result.OrderBy(n => n);
                        break;
                    case -1:
                        result = result.OrderByDescending(n => n);
                        break;
                    default:
                        return BadRequest("Ќекорректное значение параметра sortStrategy");
                }
            }

            return Ok(result.ToList());
        }

        [HttpGet("{index}")]
        public IActionResult Get(int index)
        {
            if (index < 0 || index >= _names.Count)
            {
                return NotFound($"Ёлемент с индексом {index} не найден.");
            }

            return Ok(_names[index]);
        }

        [HttpGet("count/{name}")]
        public IActionResult GetCount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("»м€ не может быть пустым.");
            }

            int count = _names.Count(n => n.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Ok(count);
        }

        [HttpPost]
        public IActionResult Add([FromBody][Required] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("»м€ не может быть пустым.");
            }

            _names.Add(name);
            return CreatedAtAction(nameof(Get), new { index = _names.Count - 1 }, name);
        }

        [HttpPut("{index}")]
        public IActionResult Update(int index, [FromBody][Required] string newName)
        {
            if (index < 0 || index >= _names.Count)
            {
                return NotFound($"Ёлемент с индексом {index} не найден.");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest("Ќовое им€ не может быть пустым.");
            }

            _names[index] = newName;
            return NoContent();
        }

        [HttpDelete("{index}")]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= _names.Count)
            {
                return NotFound($"Ёлемент с индексом {index} не найден.");
            }

            _names.RemoveAt(index);
            return NoContent();
        }
    }
}

