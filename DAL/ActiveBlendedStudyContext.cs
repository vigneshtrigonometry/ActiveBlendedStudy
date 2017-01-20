using BOL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ActiveBlendedStudyContext : DbContext
    {
        /**********************************************************************/
        // Constructors
        /**********************************************************************/
        /// <summary>
        /// Allows entity framework to build the dB
        /// </summary>
        public ActiveBlendedStudyContext() : base("AzureConnection")
        {
            //Database.SetInitializer<ActiveBlendedStudyContext>(null);
        }

        /**********************************************************************/
        // Singleton Instance
        /**********************************************************************/
        /// <summary>
        /// Get the singleton instance of the context.
        /// </summary>
        public static ActiveBlendedStudyContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActiveBlendedStudyContext();
                }
                return instance;
            }
        }
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/
        // Singleton instance for accessing the context.
        private static ActiveBlendedStudyContext instance;


        // Db for courses
        public DbSet<Course> Courses { get; set; }
        // Db for forums
        public DbSet<Forum> Forums { get; set; }
        // Db for forum answers
        public DbSet<Forum_Answer> Forum_Answers { get; set; }
        // Db for materials
        public DbSet<Material> Materials { get; set; }
        // Db for quizzes
        public DbSet<Quiz> Quizzes { get; set; }
        // Db for quiz questions
        public DbSet<Quiz_Question> Quiz_Questions { get; set; }
        // Db for questions answers
        public DbSet<Question_Answer> Question_Answers { get; set; }
        // Db for schedules
        public DbSet<Schedule> Schedules { get; set; }
        // Db for users
        public DbSet<User> Users { get; set; }
    }
}
