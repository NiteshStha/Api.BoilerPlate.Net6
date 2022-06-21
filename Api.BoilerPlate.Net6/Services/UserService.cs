using Api.BoilerPlate.Net6.Helpers;
using Api.BoilerPlate.Net6.Models.Users;
using Contract;
using Ems.Api.Authorization;
using Entities.Models;
using Microsoft.Extensions.Options;
using Utility;

namespace Api.BoilerPlate.Net6.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticate the user while signing in with a username and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The AuthenticateResponse which contains the user information with a token</returns>
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        /// <summary>
        /// Sign up a new user in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The new signed up user</returns>
        Task<User> SignUp(SignUpRequest model);

        /// <summary>
        /// Get All the user from the database
        /// </summary>
        /// <returns>List of all the users signed up in the database</returns>
        Task<IEnumerable<User>> GetAll();

        /// <summary>
        /// Get a specific user with the id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Users with the given id signed up in the database</returns>
        Task<User> GetById(int id);

        /// <summary>
        /// Validates the sign up data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>String containing error descriptions if errors were found else null</returns>
        Task<string> ValidateSignUp(SignUpRequest model);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _repository;

        private readonly AppSettings _appSettings;
        private readonly IJwtUtils _jwtUtils;

        public UserService(IUnitOfWork repository, IOptions<AppSettings> appSetting, IJwtUtils jwtUtils)
        {
            _repository = repository;
            _appSettings = appSetting.Value;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _repository.User.FindById(x => x.Username == model.Username);

            // validate
            if (user == null || !PasswordHelper.Validate(user.Password, model.Password))
                throw new AppException("Username or password is incorrect");

            // authentication successful so generate jwt token
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken);
        }

        public async Task<User> SignUp(SignUpRequest model)
        {
            var validate = await ValidateSignUp(model);

            if (validate != null) throw new AppException(validate);

            User newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Password = PasswordHelper.Hash(model.Password),
                Role = model.Role
            };
            await _repository.User.Create(newUser);
            await _repository.Commit();
            return newUser;
        }

        public async Task<IEnumerable<User>> GetAll() => await _repository.User.FindAll();

        public async Task<User> GetById(int id)
        {
            var user = await _repository.User.FindById(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<string> ValidateSignUp(SignUpRequest model)
        {
            string errors = string.Empty;
            var user = await _repository.User.FindById(x => x.Username == model.Username);

            // validate if user with the same username is already signed up
            if (user != null) errors += "User with a same username already signed up.\n";

            // validate if the password and confirm password match
            if (model.Password != model.ConfirmPassword) errors += "Passwords do not match.\n";

            // return null if no errors found
            if (string.IsNullOrEmpty(errors)) return null;

            return errors;
        }
    }
}