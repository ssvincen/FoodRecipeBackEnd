using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.BI;
using Recipes.BO;
using Recipes.BO.Recipe;

namespace RecipesWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ILogger<RecipesController> _logger;
        private readonly IRecipesDataAccess _recipesDataAccess;
        private readonly CurrentUser _currentUser;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public RecipesController(ILogger<RecipesController> logger, IRecipesDataAccess recipesDataAccess, IWebHostEnvironment hostingEnvironment, CurrentUser currentUser)
        {
            _logger = logger;
            _recipesDataAccess = recipesDataAccess;
            _hostingEnvironment = hostingEnvironment;
            _currentUser = currentUser;
        }


        [Authorize(Roles.Admin)]
        [HttpPost("SaveRecipe")]
        public async Task<IActionResult> SaveRecipeAsync([FromForm] IFormFile file, SaveRecipeModel model)
        {
            try
            {
                var user = _currentUser.UserInfo;
                string fileName = string.Empty;
                if (file != null || file.Length != 0)
                {
                    var imageName = Path.GetFileName(file.FileName);
                    //To preventDuplicateFileNames
                    fileName = imageName + " - " + DateTime.Now.ToString();
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "RecipeImages", fileName);
                    using var stream = new FileStream(imagePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }


                var resp = await _recipesDataAccess.SaveRecipeAsync(user.Id, fileName, model);
                return StatusCode(StatusCodes.Status201Created, resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to SaveRecipe. Error: {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        [AllowAnonymous, HttpGet("GetRecipe")]
        public async Task<PaginationResult<ViewRecipeModel>> GetRecipeAsync(string searchString, int pageNumber, int pageSize)
        {
            var resp = await _recipesDataAccess.GetRecipeAsync(DataFilter.PageFilterModel(searchString, pageNumber, pageSize));
            foreach (var item in resp.Items)
            {
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "RecipeImages", item.ImageName);
                var imageExists = System.IO.File.Exists(imagePath);

                if (imageExists)
                {
                    var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    var base64Image = Convert.ToBase64String(imageBytes);
                    var imageUrl = File(imageBytes, "image/jpeg");
                    item.ImageData = base64Image;
                }
            }
            return resp;
        }
    }
}
