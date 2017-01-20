using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
namespace DAL
{
    public class QuizRepository : DashboardRepository
    {
       

        /// <summary>
        /// Get the list of quizzes attached to a particular schedule.
        /// </summary>
        /// <param name="scheduleId">The schedule for which the quizzes need to be queried.</param>
        /// <returns>The list of quizzes associated with the schedule.</returns>
        public IEnumerable<Quiz> GetAllQuiz(int scheduleId, int courseId)
        {
            return from quiz in dB.Quizzes
                   where quiz.Schedule.Schedule_ID == scheduleId &&
                   quiz.Course.Course_ID == courseId
                   select quiz;
        }
        public IEnumerable<Quiz_Question> GetAllQuizQuestionsForQuiz(int quizId)
        {
            return from Quiz_Questions in dB.Quiz_Questions
                   where Quiz_Questions.Quiz.Quiz_ID == quizId
                   select Quiz_Questions;
        }




        public void CreateNewQuiz(Quiz quiz)
        {
            dB.Quizzes.Add(quiz);
            dB.SaveChanges();
        }

        /// <summary>
        /// Adds a question to the dB.
        /// </summary>
        /// <param name="newquestions"></param>
        public void AddQuizQuestion(Quiz_Question question)
        {
            dB.Quiz_Questions.Add(question);
            dB.SaveChanges();
        }

        public Quiz getQuizById(int id)
        {
            return dB.Quizzes.Single(m => m.Quiz_ID.Equals(id));
        }
        public Quiz_Question getQuestionById(int id)
        {
            return dB.Quiz_Questions.Single(m => m.Quiz_Question_ID.Equals(id));
        }
        public void RecordStudentsAnswers(Question_Answer studentanswer)
        {

            dB.Question_Answers.Add(studentanswer);
            dB.SaveChanges();
        }
       



        public void UpdateQuestions(Quiz_Question updatedQuestions)
        {
            Quiz_Question existingQuizQuestion = (from qq in dB.Quiz_Questions
                                                  where qq.Quiz_Question_ID == updatedQuestions.Quiz_Question_ID
                                                  select qq).SingleOrDefault();

            // Make sure the object exists in dB.
            if (existingQuizQuestion != null)
            {
                existingQuizQuestion.Quiz_Question_ID = updatedQuestions.Quiz_Question_ID;
                existingQuizQuestion.Question = updatedQuestions.Question;
                existingQuizQuestion.Option_1 = updatedQuestions.Option_1;
                existingQuizQuestion.Option_2 = updatedQuestions.Option_2;
                existingQuizQuestion.Option_3 = updatedQuestions.Option_3;
                existingQuizQuestion.Option_4 = updatedQuestions.Option_4;
                existingQuizQuestion.Answer = updatedQuestions.Answer;
                existingQuizQuestion.Quiz = updatedQuestions.Quiz;
            }
            else
            {
                // Make sure the identity is used for updating the values.
                updatedQuestions.Quiz_Question_ID = 0;
                // Add the material since it does not exist in dB.
                AddQuizQuestion(updatedQuestions);
            }
            // Save the changes in dB.
            dB.SaveChanges();



        }

        /// <summary>
        /// Add a list of answers to the dB.
        /// </summary>
        /// <param name="answers">List containing the answer objects.</param>
        public void SaveQuizAnswer(List<Question_Answer> answers)
        {
            foreach(Question_Answer answer in answers)
            {
                dB.Question_Answers.Add(answer);
            }
            dB.SaveChanges();
        }

        ///return a list of answers for given question ID

        public List<Question_Answer> GetAnswersForQuestionId(int question_id)
        {
            return dB.Question_Answers.Where(u => u.Quiz_Question.Quiz_Question_ID.Equals(question_id)).ToList();
        }

        //this will delete the quiz that's been passed on

        public void DeleteQuiz(int quiz_to_be_deleted)
        {
            IEnumerable<Question_Answer> answers_for_quiz = dB.Question_Answers.Where(m => m.Quiz_Question.Quiz.Quiz_ID.Equals(quiz_to_be_deleted));
            IEnumerable<Quiz_Question> questions_for_quiz = dB.Quiz_Questions.Where(m => m.Quiz.Quiz_ID.Equals(quiz_to_be_deleted));
            dB.Question_Answers.RemoveRange(answers_for_quiz);
            dB.Quiz_Questions.RemoveRange(questions_for_quiz);
            dB.Quizzes.Remove(dB.Quizzes.Single(m=>m.Quiz_ID.Equals(quiz_to_be_deleted)));
            dB.SaveChanges();
        }

        /// <summary>
        /// Delete a particular quiz question from an existing quiz
        /// </summary>
        /// <param name="questionId">The questionId that needs to be deleted.</param>
        public void DeleteQuizQuestion(int questionId)
        {
            //must delete the answers related to that questionId 
            IEnumerable<Question_Answer> answers = dB.Question_Answers.Where(m => m.Quiz_Question.Quiz_Question_ID.Equals(questionId));

            if (answers != null)
            {
                dB.Question_Answers.RemoveRange(answers);
            }

            // Retrieve the material that needs to be deleted.
            Quiz_Question quiz_question = (from question in dB.Quiz_Questions
                                   where question.Quiz_Question_ID == questionId
                                   select question).SingleOrDefault();

            // Make sure the item exists before performing the delete operation.
            if (quiz_question != null)
            {
                dB.Quiz_Questions.Remove(quiz_question);
            }
            dB.SaveChanges();
        }

        /// <summary>
        /// Method to update a question present in dB.
        /// If the question is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="forum">The question that needs to be updated</param>
        public void UpdateQuizQuestion(Quiz_Question question)
        {
            // First check if the object exists.
            Quiz_Question existingQuestion = (from f in dB.Quiz_Questions
                                   where f.Quiz_Question_ID == question.Quiz_Question_ID
                                              select f).SingleOrDefault();

            // Make sure the object exists in dB.
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
                existingQuestion.Answer = question.Answer;
                existingQuestion.Option_1 = question.Option_1;
                existingQuestion.Option_2 = question.Option_2;
                existingQuestion.Option_3 = question.Option_3;
                existingQuestion.Option_4 = question.Option_4;
                existingQuestion.Quiz = question.Quiz;
                existingQuestion.User = question.User;
            }
            else
            {
                // Make sure the identity is used for updating the values.
                existingQuestion.Quiz_Question_ID = 0;
                // Add the question since it does not exist in dB.
                AddQuizQuestion(existingQuestion);
            }
            // Save the changes in dB.
            dB.SaveChanges();
        }
    }
}
