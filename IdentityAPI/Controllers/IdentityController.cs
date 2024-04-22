using IdentityAPI.BL;
using IdentityAPI.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly AccountBL AccountBL;

        public IdentityController(AccountBL accountBL)
        {
            AccountBL = accountBL;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/v1/identities/accounts/register")]
        public async Task<IActionResult> Register(AccountDTO AccountData) {
            var (Status, Message) = await AccountBL.CreateAccount(AccountData);
            return StatusCode(Status, Message);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/v1/identities/accounts/emailconfirmation")]
        public async Task<IActionResult> AccountConfirmation(string email, string code)
        {
            var (Status, Message) = await AccountBL.AccountConfirmation(email, code);
            return StatusCode(Status, Message);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/v1/identities/accounts/{email}/resendemailconfirmation")]
        public async Task<IActionResult> ResendAccountConfirmation(string email)
        {
            var (Status, Message) = await AccountBL.ResendAccountConfirmation(email);
            return StatusCode(Status, Message);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/v1/identities/accounts/login")]
        public async Task<IActionResult> Login(CredentialDTO CredData)
        {
            var (Status, Message) = await AccountBL.Login(CredData);
            return StatusCode(Status, Message);
        }

        [HttpGet]
        [Route("/v1/identities/accounts/manage")]
        public async Task<IActionResult> ReadAccounts()
        {
            var (Status, Users) = await AccountBL.ReadAccounts();
            return StatusCode(Status, Users);
        }

        [HttpPut]
        [Route("/v1/identities/accounts/manage")]
        public async Task<IActionResult> UpdateAccount(UserDTO UserData)
        {
            var (Status, Message) = await AccountBL.UpdateAccount(UserData);
            return StatusCode(Status, Message);
        }

        [HttpDelete]
        [Route("/v1/identities/accounts/{accountid:guid}/manage")]
        public async Task<IActionResult> DeleteAccount(Guid accountid)
        {
            var (Status, Message) = await AccountBL.DeleteAccount(accountid);
            return StatusCode(Status, Message);
        }
    }
}
