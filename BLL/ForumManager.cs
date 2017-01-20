using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
using DAL;

namespace BLL
{
    public class ForumManager : DashboardManager
    {
       //private ForumRepository forumRepository = new ForumRepository();
        /// <summary>
        /// Retrieve all forums for a particular schedule
        /// </summary>
        /// <param name="scheduleId">The schedule for which the forums need to be retrieved.</param>
        /// <returns></returns>
        public IEnumerable<Forum> GetAllForums(int scheduleId, int courseId)
        { 
            return ForumRepository.Instance.GetAllForums(scheduleId, courseId);
        }

        /// <summary>
        /// Methot to add a forum to the dB. 
        /// This checks if the forum is null reference or not. 
        /// If yes, then it thrown a NullReferenceException.
        /// The new ID of the forum will be auto generated.
        /// </summary>
        /// <param name="forum">The forum that needs to be added to the dB. Must not be null.</param>
        public void AddForum(Forum forum)
        {
            if (forum != null)
            {
                ForumRepository.Instance.AddForum(forum);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Get the forum object from the dB for a particular ID.
        /// Sends null if the object is not found.
        /// </summary>
        /// <param name="forumId">The ID for which the forum needs to be found.</param>
        /// <returns></returns>
        public Forum GetForum(int forumId)
        {
            return ForumRepository.Instance.GetForum(forumId);
        }

        /// <summary>
        /// Method to delete an existing forum from the dB.
        /// </summary>
        /// <param name="forumId">The forum that needs to be deleted.</param>
        public void DeleteForum(int forumId)
        {
            ForumRepository.Instance.DeleteForum(forumId);
        }

        /// <summary>
        /// Method to update a forum present in dB.
        /// If the forum is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="forum">The forum that needs to be updated</param>
        public void UpdateForum(Forum forum)
        {
            if (forum != null)
            {
                ForumRepository.Instance.UpdateForum(forum);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Get all the answers related to a particular forum topic.
        /// </summary>
        /// <param name="forumID">The forum topic for which the forum answers need to be retrieved.</param>
        /// <returns></returns>
        public IEnumerable<Forum_Answer> GetAllForumAnswers(int forumID)
        {
            return ForumRepository.Instance.GetAllForumAnswers(forumID);
        }

        public void AddForumPost(Forum_Answer post)
        {
            ForumRepository.Instance.addForumPosts(post);
        }

        
    }
}
