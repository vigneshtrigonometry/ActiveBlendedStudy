using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LoginManager
    {
        //private LoginRepository loginRepository = new LoginRepository();
        /// <summary>
        /// Method to initialize the dB.
        /// </summary>
        /// <summary>
        /// Method to validate the login for a particular user.  
        /// </summary>
        /// <param name="user">The user which needs to be validated.</param>
        /// <returns>If the credentials are correct, then user with updated details is returned. Else null.</returns>
        public User ValidateUserLogin(int userId, string password)
        {
            User retrievedUser = LoginRepository.Instance.GetUser(userId);
            // Check if the user exists in dB
            if (retrievedUser != null && retrievedUser.Password == password)
            {
                return retrievedUser;
            }
            return null;
        }

        /// <summary>
        /// Get a user with his userId
        /// </summary>
        /// <param name="userId">The userId associated with the user.</param>
        /// <returns>The user object with the values from dB.</returns>
        public User GetUser(int userId)
        {
            return LoginRepository.Instance.GetUser(userId);
        }
    }
}
