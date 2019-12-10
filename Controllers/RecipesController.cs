using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;
using Microsoft.AspNetCore.Authorization;

namespace Recipes.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class RecipesController : ControllerBase
    {
        private readonly RecipesContext _context;

        /// <summary>
        /// Recipes controller
        /// </summary>
        public RecipesController(RecipesContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of recipes.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Recipes
        ///
        /// </remarks>
        /// <returns>A list of recipes</returns>
        /// <response code="200">Returns the recipes list</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        /// <summary>
        /// Get recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Recipes/1
        ///
        /// </remarks>
        /// <returns>Recipe</returns>
        /// <response code="200">Returns the recipe</response>
        /// <response code="404">Recipe not find</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        /// <summary>
        /// Update recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Recipes/1
        ///     {
        ///        "id": 1,
        ///        "name": "Ciasto",
        ///        "description": "Super przepis na ciasto owocowe"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly updated recipe</returns>
        /// <response code="201">Returns the newly updated recipe</response>
        /// <response code="400">Bad request</response> 
        /// <response code="404">Recipe not find</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutRecipe(long id, Recipe Recipe)
        {
            if (id != Recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(Recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        /// <summary>
        /// Creates a Recipe.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Recipes
        ///     {
        ///        "name": "Ciasto",
        ///        "description": "Super przepis na ciasto owocowe"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created recipe</returns>
        /// <response code="201">Returns the newly created Recipe</response>
        /// <response code="400">Bad request</response>            
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe Recipe)
        {
            
            _context.Recipes.Add(Recipe);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetRecipe), new { id = Recipe.Id }, Recipe);
        }

        /// <summary>
        /// Delete Recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Recipes/1
        ///
        /// </remarks>
        /// <returns>A newly deleted Recipe</returns>
        /// <response code="201">Returns the newly deleted Recipe</response>
        /// <response code="404">Recipe not find</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Recipe>> DeleteRecipe(long id)
        {
            var Recipe = await _context.Recipes.FindAsync(id);
            if (Recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(Recipe);
            await _context.SaveChangesAsync();

            return Recipe;
        }
        /// <summary>
        /// Search Recipe
        /// </summary>

        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipe(string name)
        {
            return await _context.Recipes.Where(Recipe => EF.Functions.Like(Recipe.Name, String.Concat("%", name, "%"))).ToListAsync();
        }


        private bool RecipeExists(long id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
