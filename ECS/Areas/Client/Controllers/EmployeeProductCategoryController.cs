﻿using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Areas.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProductCategoryController : ControllerBase
    {
        private readonly IEmployeeProductCategoryRepository _repository;

        public EmployeeProductCategoryController(IEmployeeProductCategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddEmployeeProductCategory([FromBody] EmployeeProductCategory employeeProductCategory)
        {
            await _repository.AddEmployeeProductCategory(employeeProductCategory);
            return Ok(new { message = "EmployeeProductCategory added successfully." });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployeeProductCategory(int id)
        {
            await _repository.DeleteEmployeeProductCategory(id);
            return Ok(new { message = "EmployeeProductCategory deleted successfully." });
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<EmployeeProductCategory>>> GetAllEmployeeProductCategories()
        {
            var result = await _repository.GetAllEmployeeProductCategories();
            return Ok(result);
        }
        [HttpGet("employees/category/{categoryId}")]
        public async Task<ActionResult<List<Employee>>> GetEmployeesByCategoryId(int categoryId)
        {
            var result = await _repository.GetEmployeeForProductCategoryByCategoryId(categoryId);
            return Ok(result);
        }

        [HttpGet("categories/employee/{employeeId}")]
        public async Task<ActionResult<List<ProductCategory>>> GetCategoriesByEmployeeId(Guid employeeId)
        {
            var result = await _repository.GetProductCategoryByEmployeeId(employeeId);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeProductCategory>> GetEmployeeProductCategoryById(int id)
        {
            var result = await _repository.GetEmployeeProductCategoryById(id);
            if (result == null)
            {
                return NotFound(new { message = "EmployeeProductCategory not found." });
            }
            return Ok(result);
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateEmployeeProductCategoryDto updateDto)
        {
            if (updateDto == null)
                return BadRequest("Invalid request data.");

            try
            {
                // Create EmployeeProductCategory object from DTO
                var employeeProductCategory = new EmployeeProductCategory
                {
                    EmployeeId = updateDto.EmployeeId,
                    CategoryId = updateDto.CategoryId
                };

                // Call repository to update the category
                await _repository.UpdateEmployeeProductCategoryByEmployeeId(employeeProductCategory);

                return Ok("Employee product category updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
