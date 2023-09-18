using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotNet6TestCacheRedis.Application;
using DotNet6TestCacheRedis.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DotNet6TestCacheRedis.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _personApplication;
        public PersonController(IPersonApplication personApplication)
        {
            _personApplication = personApplication;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var record = await _personApplication.GetByIdAsync(id);
                return Ok(record);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> GetAll()
        {
            var records = await _personApplication.GetAllAsync();
            return records!;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                var record = await _personApplication.CreateAsync();
                return Ok(record);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update([FromQuery] int id)
        {
            try
            {
                await _personApplication.UpdateAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _personApplication.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
