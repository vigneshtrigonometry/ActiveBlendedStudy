using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BOL;

namespace BLL
{
    public class QuizManager : DashboardManager
    {
        private QuizRepository quizRepository = new QuizRepository();
        public IEnumerable<Quiz> GetAllQuiz(int scheduleId, int courseId)
        {
            return quizRepository.GetAllQuiz(scheduleId, courseId);
        }

        public IEnumerable<Quiz_Question> GetAllQuizQuestions(int quizId)
        {
            return quizRepository.GetAllQuizQuestionsForQuiz(quizId);
        }


        public void CreateNewQuiz(Quiz quiz)
        {
            quizRepository.CreateNewQuiz(quiz);
        }

        /// <summary>
        /// Adds a new question to the dB.
        /// </summary>
        /// <param name="question"></param>
        public void AddQuizQuestion(Quiz_Question question)
        {
            quizRepository.AddQuizQuestion(question);
        }

        public Quiz getQuizById(int QuizId)
        {
            return quizRepository.getQuizById(QuizId);
        }
        public Quiz_Question getQuestionById(int QuestionId)
        {

            return quizRepository.getQuestionById(QuestionId);
        }
        public void RecordStudentsAnswers(Question_Answer studentanswer)
        {
            quizRepository.RecordStudentsAnswers(studentanswer);
        }
        public void UpdateQuestions(Quiz_Question updatedQuestions)
        {
            if (updatedQuestions != null)
            {
                quizRepository.UpdateQuestions(updatedQuestions);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Add a list of answers to the dB.
        /// </summary>
        /// <param name="answers">List containing the answer objects.</param>
        public void SaveQuizAnswer(List<Question_Answer> answers)
        {
            if(answers != null)
            {
                quizRepository.SaveQuizAnswer(answers);
            }
            else
            {
                throw new NullReferenceException();
            }
        }


        ///Check whether the user is eligible to attend the quiz
        ///if she/he already answered it, return false if he's not
        ///

        public Boolean IsEligibleToAttendQuiz(int quiz_id, int user_id)
        {
            Boolean flag = true;
            List<Quiz_Question> questions = quizRepository.GetAllQuizQuestionsForQuiz(quiz_id).ToList();
            List<Question_Answer> answers = new List<Question_Answer>();
            foreach (var item in questions)
            {
                answers = quizRepository.GetAnswersForQuestionId(item.Quiz_Question_ID);
                foreach (var ans in answers)
                {
                    if (ans.User.User_ID == user_id)
                    {
                        flag = false; //user has already answered this quiz
                    }
                }
            }
            return flag;
        }


        //this will delete the quiz of the particular id

        public Boolean DeleteQuizWithID(int quiz_id)
        {
            try
            {
                quizRepository.DeleteQuiz(quiz_id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //business logic for reposting a quiz

        public Boolean RepostQuiz(int quiz_id, int user_id)
        {
            try
            {
                Quiz quiz = quizRepository.getQuizById(quiz_id);
                //get the course of the quiz
                Course course = quiz.Course;
                //check if the course in question is still available in current schedule
                Schedule schedule = new ScheduleManager().GetCurrentSchedule();
                if(schedule.Courses.Contains(course))
                {
                    //check if the user is still enrolled in the current schedule
                    User user = new LoginManager().GetUser(user_id);
                    if(schedule.Users.Contains(user))
                    {
                        //check if that user is enrolled in the course
                        if(schedule.Courses.Where(u=>u.Users.Contains(user))!=null)
                        {
                            //now repost
                            Quiz repost_quiz = new Quiz()
                            {
                                Course = course,
                                Name = quiz.Name,
                                Questions = quiz.Questions,
                                Schedule = schedule,
                                User = user
                            };
                            new QuizManager().CreateNewQuiz(repost_quiz);
                            return true;
                        }
                    }
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }

        }


        //return a list of four int values corresponding to the each option of the passed question_id respectively

        public List<int> GetStatisticsForQuestion(int question_id)
        {
            List<int> stat_for_answers = new List<int>();
            Quiz_Question question = getQuestionById(question_id);
            List<Question_Answer> answers = question.Answers;
            stat_for_answers.Add(answers.Where(a => a.Answer.Equals("Option_1")).ToList().Count);
            stat_for_answers.Add(answers.Where(a => a.Answer.Equals("Option_2")).ToList().Count);
            stat_for_answers.Add(answers.Where(a => a.Answer.Equals("Option_3")).ToList().Count);
            stat_for_answers.Add(answers.Where(a => a.Answer.Equals("Option_4")).ToList().Count);
            return stat_for_answers;
        }

        public int NumberOfEligibleStudentsForQuiz(int quiz_id)
        {
            int no_students = 0;
            Quiz quiz = getQuizById(quiz_id);
            Schedule schedule = quiz.Schedule;
            Course course = quiz.Course;
            List<User> users_for_schedule = schedule.Users;
            List<User> users_eligible = users_for_schedule.Where(m => m.Courses.Any(c => c.Course_ID == course.Course_ID)).ToList();
            no_students = users_eligible.Count - users_eligible.Where(m => m.Role.ToUpper().Equals("STAFF")).ToList().Count;
            return no_students;
        }

        /// <summary>
        /// Delete a particular quiz question from an existing quiz
        /// </summary>
        /// <param name="questionId">The questionId that needs to be deleted.</param>
        public void DeleteQuizQuestion(int questionId)
        {
            quizRepository.DeleteQuizQuestion(questionId);
        }

        /// <summary>
        /// Method to update a question present in dB.
        /// If the question is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="question">The question that needs to be updated</param>
        public void UpdateQuizQuestion(Quiz_Question question)
        {
            if (question != null)
            {
                quizRepository.UpdateQuestions(question);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
