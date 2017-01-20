using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LoginRepository : DashboardRepository
    {

        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        private static LoginRepository instance;

        //private ActiveBlendedStudyContext dB;

        /**********************************************************************/
        // Constructors
        /**********************************************************************/

        /// <summary>
        /// Disable creating the initialization of this class. Users need to use the singleton instance.
        /// </summary>
        private LoginRepository()
        {
            dB = ActiveBlendedStudyContext.Instance;
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        /// <summary>
        /// Get Instance of the Repository
        /// </summary>
        public static LoginRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginRepository();
                }
                return instance;
            }
        }

        public void initializeContext()
        {
            dB.Database.Initialize(true);
        }
        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns>The list of all users present in the dB</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return dB.Users;
        }

        /// <summary>
        /// Get user by his user ID
        /// </summary>
        /// <param name="userId"> The user Id attached to the user.</param>
        /// <returns>The user object if found, else null</returns>
        public User GetUser(int userId)
        {
            try
            {
                return dB.Users.Single(u => u.User_ID == userId);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error occured: " + e.ToString());
                return null;
            }
            
        }
    }
}
