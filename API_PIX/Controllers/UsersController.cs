using API_PIX.Application.Interfaces;
using API_PIX.Domain.ClientModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PIX.Web.Controllers
{
    public class UsersController : BaseController
    {

        private readonly IClientService ClientService;
        private readonly IAuthService AuthService;

        public UsersController(IClientService userService,
            IAuthService authService,
            IServiceProvider sp) : base(sp)
        {
            ClientService = userService;
            AuthService = authService;
        }


        [HttpGet("{id}")]
        public Client GetUser(string id)
        {
            var user = ClientService.GetClient(Guid.Parse(id));
            user.PassHash = "";
            return user;
        }

        [HttpPost("register")]
        public IActionResult Create([FromBody] Client user)
        {
            try
            {
                if (user == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }

                user = ClientService.RegisterClient(user);

            }
            catch (DbUpdateException e)
            {
                return BadRequest("Error while creating");
            }

            try
            {
                /*
                var validationMessages = ClientService.GetValidationMessage();
                if (user == null || validationMessages.Length > 0)
                    return Ok(validationMessages);
                */
            }
            catch
            {

            }

            return Ok(user);
        }

        [HttpPost("webregister")]
        public IActionResult WebCreate([FromBody] Client user)
        {
            return Create(user);
        }

        /*
        [HttpPost("reset")]
        public IActionResult ResetPassword([FromBody] string username)
        {
            if (username != null)
            {
                var newpass = UserService.ResetPassword(username, HttpContext.Connection.RemoteIpAddress.ToString());
                if (newpass != null)
                {
                    var subject = "Zuvvii Password Reset";
                    var text = @"Your Zuvvii password has been changed. Your new temporary password is " + newpass.passHash + " .<br />";
                    text += @"This password should only be used as a temporary password. Once logged in, please visit the settings page and reset your own password.<br /><br />";
                    text += @"If this was not you, please email contact@zuvvii.com immediately.<br /><br />";
                    text += @"Regards,<br />";
                    text += @"Team Zuvvii";



                    var emailTask = new Task(() =>
                    {
                        EmailService.SendEmail(subject, text, newpass.emailAddress);
                    });

                    emailTask.Start();
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Edit([FromBody] User user)
        {


            try
            {
                if (user == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }
                var userRequest = GetUser();
                if (userRequest.Id != user.Id)
                    return BadRequest("User Not Logged In");

                var result = UserService.UpdateUser(user);
                if (!result) return Ok("Error");
                return Ok(result);
            }
            catch (DbUpdateException e)
            {
                return BadRequest("Error while creating");
            }
            return NoContent();
        }
        */

        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, string> loginInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(loginInfo["username"]) || string.IsNullOrEmpty(loginInfo["passhash"]))
                    return BadRequest("Bad Login Info");

                string Email = loginInfo["username"];
                string PassHash = loginInfo["passhash"];


                var login = AuthService.AuthenticateClient(Email, PassHash);
                if (login != null)
                {
                    return Ok(login);
                }
            }
            catch (Exception ex)
            {

            }
            return BadRequest("Username or Password Is Not Valid");
        }

        /*
        [HttpGet("profile/{userid}")]
        public IActionResult GetProfile(Nullable<int> userid)
        {
            if (!userid.HasValue)
                return BadRequest("Invalid user id");

            var session = GetSession();

            Nullable<int> loggedUserId = session != null && (session.UserId != userid.Value) ? new Nullable<int>(session.UserId) : null;

            var user = UserService.GetUser(userid.Value, loggedUserId);
            if (user != null)
                return Ok(user);
            else return BadRequest("User Not Found");
        }

        [HttpGet("username/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            try
            {
                var user = UserService.GetUserByUsername(username);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest("User Not Found");
            }
        }

        [HttpGet("profile/friends")]
        public IActionResult GetFriends(int userid)
        {
            if (GetUser() == null)
                return BadRequest();
            if (string.IsNullOrEmpty("userid"))
                return BadRequest("Bad Profile Name");

            var friends = UserService.GetFriends(userid);
            var list = friends.Select(x => x.FriendId);
            return Ok(list);
        }

        [HttpPost("follow/{userId}")]
        public IActionResult FollowUser(int userId)
        {
            var user = GetUser();
            try
            {
                UserService.FollowUser(user.Id, userId);
            }
            catch
            {
                Thread.Sleep(500);
                UserService.FollowUser(user.Id, userId);

            }
            return Ok(true);
        }

        [HttpPost("unfollow/{userId}")]
        public IActionResult UnFollowUser(int userId, [FromBody] string sessionId)
        {
            var user = GetUser();
            try
            {
                UserService.UnFollowUser(user.Id, userId);
            }
            catch
            {
                Thread.Sleep(500);
                UserService.UnFollowUser(user.Id, userId);
            }

            return Ok(true);
        }

        [HttpGet("{userId}/followers/{start}/{take}")]
        public IActionResult GetFollowers(int userId, int start, int take)
        {
            var list = UserService.GetFollowers(userId, start, take);
            return Ok(list);
        }

        [HttpGet("{userId}/following/{start}/{take}")]
        public IActionResult GetFollowing(int userId, int start, int take)
        {
            var following = UserService.GetFollowing(userId, start, take);
            return Ok(following);
        }

        [HttpPostAttribute("Block")]
        public IActionResult BlockUser([FromBody] BlockedUser bu)
        {
            var user = GetUser();
            //if (user.Id != bu.UserId)
            //    return BadRequest("Authentication Error");
            var result = UserService.BlockUser(bu);
            return Ok(result);
        }
        [HttpPostAttribute("UnBlock")]
        public IActionResult UnBlockUser([FromBody] BlockedUser bu)
        {

            var user = GetUser();
            //if (user.Id != bu.UserId)
            //    return BadRequest("Authentication Error");
            var result = UserService.UnBlockUser(bu);
            return Ok(result);
        }
        [HttpGetAttribute("{userId}/blocked")]
        public IActionResult BlockedUsers(int userId)
        {

            var user = GetUser();
            //if (user.Id != userId)
            //  return BadRequest("Authentication Error");
            var result = UserService.GetBlockedUsers(userId);
            return Ok(result);
        }

        [HttpPost("addfcmtoken")]
        public IActionResult Add_FCMToken(FCMTokenUpdate fcmModel)
        {
            try
            {
                var user = UserService.GetUser(fcmModel.UserId, null);
                if (user != null)
                {
                    user.FCMToken = fcmModel.FCMToken;
                    var result = UserService.UpdateUser(user);
                    if (!result) return Ok("Error");
                    return Ok(result);
                }
                else return BadRequest("User Not Found");
            }
            catch (DbUpdateException e)
            {
                return BadRequest("Error while creating");
            }
            return NoContent();
        }
    }
        */

    }
}
