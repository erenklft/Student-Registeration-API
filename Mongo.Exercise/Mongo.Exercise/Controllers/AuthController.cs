using Microsoft.AspNetCore.Mvc;
using Mongo.Exercise.Models;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly UserService _userService;

	public AuthController(UserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] UserModel model)
	{
		var isRegistered = await _userService.RegisterUserAsync(model);
		if (!isRegistered)
		{
			return Conflict(new { Message = "Kullanıcı adı zaten mevcut." });
		}

		return Ok(new { Message = "Kullanıcı başarıyla kaydedildi." });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] UserModel model)
	{
		var user = await _userService.AuthenticateAsync(model.Username, model.Password);
		if (user == null)
		{
			return Unauthorized(new { Message = "Kullanıcı adı veya şifre hatalı." });
		}

		return Ok(new { Message = "Giriş başarılı." });
	}
}
