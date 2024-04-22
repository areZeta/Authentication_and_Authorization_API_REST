using System.Net;
using IdentityAPI.DTOs;
using IdentityAPI.Entities;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.BL
{
    public class AccountBL
    {
        private readonly IdentityDbContext DbContext;
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly EmailService EmailService;
        private readonly IConfiguration Configuration;
        public AccountBL(IdentityDbContext dbContex, UserManager<User> userManager, EmailService emailSender,
            IConfiguration configuration, SignInManager<User> signInManager)
        {
            DbContext = dbContex;
            UserManager = userManager;
            EmailService = emailSender;
            Configuration = configuration;
            SignInManager = signInManager;
        }

        #region Account
        public async Task<(int StatusCode, string Message)> CreateAccount(AccountDTO AccountData)
        {
            try
            {
                User? ExistUser = await DbContext.Users.Where(Users => Users.Email!.Equals(AccountData.Email)).FirstOrDefaultAsync();
                if (ExistUser != null)
                {
                    return (StatusCodes.Status409Conflict, "Account is already exist.");
                }
                User NewUser = new User
                {
                    Email = AccountData.Email,
                    PhoneNumber = AccountData.Phone,
                    UserName = AccountData.Email,
                    FullName = AccountData.FullName,
                };
                var result = await UserManager.CreateAsync(NewUser, AccountData.Password);
                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(NewUser);
                    var url = Configuration.GetSection("Urls:callbackUrl").Value! + $"emailconfirmation?email={AccountData.Email}&code={code}";
                    var encodedUrl = WebUtility.HtmlDecode(url);
                    var (StatusCode, Message, EmailModel) = EmailService.ConfigurationEmail(AccountData.Email, "Account confirmation", $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");
                    if (StatusCode != StatusCodes.Status200OK)
                        return (StatusCode, Message);

                    (StatusCode, Message) = EmailService.SendEmail(EmailModel);
                    //TO-DO LOGGER
                }
                else return (StatusCodes.Status200OK, result.Errors.ToList()[0].Description);
                return (StatusCodes.Status200OK, "User created successfully, We send an email to you. please confirm your email.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<(int StatusCode, List<UserDTO> Users)> ReadAccounts()
        {
            List<UserDTO> Users = new();
            try
            {
                Users = await DbContext.Users.Select(User => new UserDTO
                {
                    Id = User.Id,
                    Email = User.Email!,
                    Phone = User.PhoneNumber!,
                    FullName = User.FullName
                }).ToListAsync();
                return (StatusCodes.Status200OK, Users);
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, Users);
            }
        }

        public async Task<(int StatusCode, string Message)> UpdateAccount(UserDTO UserData)
        {
            try
            {
                User? ExistUser = await DbContext.Users.Where(Users => Users.Email!.Equals(UserData.Email) &&
                !Users.Id.Equals(UserData.Id)).FirstOrDefaultAsync();
                if (ExistUser != null)
                {
                    return (StatusCodes.Status409Conflict, "Account is already exist.");
                }

                ExistUser = await UserManager.FindByIdAsync(UserData.Id.ToString());
                if (ExistUser == null)
                {
                    return (StatusCodes.Status404NotFound, "Account not found.");
                }
                ExistUser.PhoneNumber = UserData.Phone;
                ExistUser.FullName = UserData.FullName;
                await DbContext.SaveChangesAsync();
                return (StatusCodes.Status200OK, "User updated successfully.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<(int StatusCode, string Message)> DeleteAccount(Guid UserId)
        {
            try
            {
                User? ExistUser = await UserManager.FindByIdAsync(UserId.ToString());
                if (ExistUser == null)
                {
                    return (StatusCodes.Status404NotFound, "Account not found.");
                }

                await UserManager.DeleteAsync(ExistUser);
                return (StatusCodes.Status200OK, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        public async Task<(int StatusCode, string Message)> Login(CredentialDTO CredData)
        {
            try
            {
                User? ExistUser = await UserManager.FindByEmailAsync(CredData.Email);
                if (ExistUser == null)
                {
                    return (StatusCodes.Status404NotFound, "Your credentials aren´t correct.");
                }
                var result = await SignInManager.CheckPasswordSignInAsync(ExistUser, CredData.Password, false);
                if (result.Succeeded)
                {
                    //TO-DO 2FA
                    await SignInManager.SignInAsync(ExistUser, false);
                }
                return (StatusCodes.Status200OK, "User is logged in.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<(int StatusCode, String Message)> ResendAccountConfirmation(string Email)
        {
            try
            {
                User? ExistUser = await UserManager.FindByEmailAsync(Email);
                if (ExistUser == null)
                {
                    return (StatusCodes.Status404NotFound, "Account not found.");
                }
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(ExistUser);
                code = WebUtility.UrlEncode(code);
                var url = Configuration.GetSection("Urls:callbackUrl").Value! + $"emailconfirmation?email={Email}&code={code}";
                var encodedUrl = WebUtility.HtmlEncode(url);
                var (StatusCode, Message, EmailModel) = EmailService.ConfigurationEmail(Email, "Account confirmation", $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");
                if (StatusCode != StatusCodes.Status200OK)
                    return (StatusCode, Message);

                (StatusCode, Message) = EmailService.SendEmail(EmailModel);
                if (StatusCode != StatusCodes.Status200OK) return (StatusCode, "I´m sorry, something went wrong, try again.");
                return (StatusCodes.Status200OK, "Review rour email please.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<(int StatusCode, String Message)> AccountConfirmation(string Email, string Code)
        {
            try
            {
                User? ExistUser = await UserManager.FindByEmailAsync(Email);
                if (ExistUser == null)
                {
                    return (StatusCodes.Status404NotFound, "Account not found.");
                }
                var result = await UserManager.ConfirmEmailAsync(ExistUser, Code);
                if (!result.Succeeded) return (StatusCodes.Status404NotFound, "I´m sorry, something went wrong.");
                return (StatusCodes.Status200OK, "Account confirmed successfully.");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
