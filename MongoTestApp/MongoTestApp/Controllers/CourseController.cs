using Microsoft.AspNetCore.Mvc;
using MongoTestApp.Services;

namespace MongoTestApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("get-course-details")]
    public async Task<IActionResult> GetProducts()
    {
        var data = await _courseService.GetCourseDetails();
        return Ok(data);
    }
}
