using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;

namespace DAL
{
    public class ForumRepository : DashboardRepository
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/
        private static ForumRepository instance;

        /**********************************************************************/
        // Private Methods
        /**********************************************************************/
        private ForumRepository() : base()
        {
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        public static ForumRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ForumRepository();
                }
                return instance;
            }
        }


        /// <summary>
        /// Get all the forums for a particulas schedule id
        /// </summary>
        /// <param name="scheduleId">The schedule id for which the forums need to be queried.</param>
        /// <returns></returns>
        public IEnumerable<Forum> GetAllForums(int scheduleId, int courseId)
        {
            return from forum in dB.Forums
                   where forum.Schedule.Schedule_ID == scheduleId && forum.Course.Course_ID == courseId
                   select forum;
        }

        /// <summary>
        /// Get the material using the material Id.
        /// </summary>
        /// <param name="forumId">The specific forum Id.</param>
        /// <returns></returns>
        public Forum GetForum(int forumId)
        {
            try
            {
                return (from forum in dB.Forums
                        where forum.Forum_ID == forumId
                        select forum).SingleOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method to add a new forum to the dB.
        /// </summary>
        /// <param name="forum"> The forum that needs to be added to the dB.</param>
        public void AddForum(Forum forum)
        {
            dB.Forums.Add(forum);
            dB.SaveChanges();
        }

        /// <summary>
        /// Method to delete an existing forum from the dB.
        /// </summary>
        /// <param name="forumId">The forum that needs to be deleted.</param>
        public void DeleteForum(int forumId)
        {
            //must delete the forum answers related to that forum_id 
            IEnumerable<Forum_Answer> answers = dB.Forum_Answers.Where(m => m.Forum.Forum_ID.Equals(forumId));

            if(answers!=null)
            {
                dB.Forum_Answers.RemoveRange(answers);
            }

            // Retrieve the material that needs to be deleted.
            Forum forumToDelete = (from forum in dB.Forums
                                         where forum.Forum_ID == forumId
                                         select forum).SingleOrDefault();

            // Make sure the item exists before performing the delete operation.
            if (forumToDelete != null)
            {
                dB.Forums.Remove(forumToDelete);
            }
            dB.SaveChanges();
        }

        /// <summary>
        /// Method to update a forum present in dB.
        /// If the forum is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="forum">The forum that needs to be updated</param>
        public void UpdateForum(Forum forum)
        {
            // First check if the object exists.
            Forum existingForum = (from f in dB.Forums
                                         where f.Forum_ID == forum.Forum_ID
                                         select f).SingleOrDefault();

            // Make sure the object exists in dB.
            if (existingForum != null)
            {
                existingForum.Forum_ID = forum.Forum_ID;
                existingForum.Schedule = forum.Schedule;
                existingForum.Answers = forum.Answers;
                existingForum.Topic = forum.Topic;
            }
            else
            {
                // Make sure the identity is used for updating the values.
                forum.Forum_ID = 0;
                // Add the forum since it does not exist in dB.
                AddForum(forum);
            }
            // Save the changes in dB.
            dB.SaveChanges();
        }

        /// <summary>
        /// Get all the forum answers related to particular forum topic.
        /// </summary>
        /// <param name="forumId">The forum for which the answers need to be retrieved.</param>
        /// <returns>The list of forum answers related to the forum topic.</returns>
        public IEnumerable<Forum_Answer> GetAllForumAnswers(int forumId)
        {
            return dB.Forum_Answers.Where(m => m.Forum.Forum_ID.Equals(forumId));
        }



        public void addForumPosts(Forum_Answer answer)
        {
            dB.Forum_Answers.Add(answer);
            dB.SaveChanges();
        }

    }
}
