using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_App.Data;
using Social_Media_App.DTO;
using Social_Media_App.Entities;
using Social_Media_App.Interfaces;

namespace Social_Media_App.Controllers;

public class AuthController : BaseController
{   
    //DB CONTEXT 
    private readonly DataContext _context;
    
    //TOKEN SERVICE
    private readonly ITokenService _tokenService;
    
    //CONSTRUCTOR
    public AuthController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    //REGISTER CONTROLLER
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO obj)
    {
        //VERIFY DUPLICATION
        if (await VerifyDuplicate(obj.UserName)) return BadRequest("Username taken");
        
        //UTILIZED ENCRYPTION ALGORITHM 
        using var hmac = new HMACSHA512();
        
        //CREATING NEW USER BASED ON REGISTER DTO
        var user = new AppUser
        {
            UserName = obj.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(obj.Password)),
            PasswordSalt = hmac.Key
        };
        //ADDING NEW USER TO DB 
        _context.Users.Add(user);
        
        //SAVING DB CHANGES
        await _context.SaveChangesAsync();
        
        //RETURN CREATED USER
        return new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.GenerateToken(user)
        };
    }
    
    //LOGIN CONTROLLER
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> UserLogin(LoginDTO obj)
    {
        //ATTEMPT TO FETCH USER IN DB
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == obj.UserName);
        
        //401 IF USER NOT FOUND
        if (user == null) return Unauthorized("User not found");
        
        //CALCULATE USER HASH AND SALT FOR VERIFICATION
        using var hmac = new HMACSHA512(user.PasswordSalt);
        
        //GENERATE HASHED PASSWORD BASED ON THE LOGIN PASSWORD 
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(obj.Password));
        
        //COMPARING HASHES FOR LOGIN PASSWORD HASH AND DB PASSWORD HASH
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password is incorrect");
        }
        
        //RETURNS USER
        return new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.GenerateToken(user)
        };
    }
    //VERIFY IF USERNAME IS ALREADY TAKEN
    public async Task<bool> VerifyDuplicate(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}