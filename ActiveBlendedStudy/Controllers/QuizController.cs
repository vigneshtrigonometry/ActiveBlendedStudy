using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOL;
using BLL;
using ActiveBlendedStudy.Filters;

namespace ActiveBlendedStudy.Controllers
{
    [SessionAuthorize]
    public class QuizController : Controller
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        private QuizManager quizManager;

        /// <summary>
        /// Hold the current course Id. Will be read from the session.
        /// </summary>
        private int courseId;

        /// <summary>
        /// Hold the current schedule Id. Will be read from the session.
        /// </summary>
        private int scheduleId;

        /// <summary>
        /// Hold the current user Id. Will be read from the session.
        /// </summary>
        private int userId;

        /**********************************************************************/
        // Constructors
        /**********************************************************************/
        public QuizController()
        {
            quizManager = new QuizManager();

        }

        /**********************************************************************/
        // Public Methods / Actions
        /**********************************************************************/

        /**
        * Methods for Quiz!!
        */

        // GET: Quiz
        public ActionResult Index()
        {
            GetValuesFromSession();
            ViewBag.IsCurrentSchedule = false;
            if (new ScheduleManager().IsScheduleTheLatest(scheduleId))
            {
                ViewBag.IsCurrentSchedule = true;
            }
            List<Quiz> quizs = (quizManager.GetAllQuiz(scheduleId, courseId).ToList());
            return View(quizs);
        }

        // GET: Quiz/Create
        [SessionAuthorize(role = "staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quiz/Create
        [HttpPost]
        [SessionAuthorize(role = "staff")]
        public ActionResult Create(Quiz newQuiz)
        {
            GetValuesFromSession();
            newQuiz.Schedule = new ScheduleManager().GetScheduleById(scheduleId);
            newQuiz.User = new LoginManager().GetUser(userId);
            newQuiz.Course = new CourseManager().GetCourse(courseId);
            quizManager.CreateNewQuiz(newQuiz);
            return RedirectToAction("Index");
        }

        // GET: Quiz/Delete/5
        [SessionAuthorize(role = "staff")]
        public ActionResult Delete(int id)
        {
            if (quizManager.DeleteQuizWithID(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Content("PROBLEM WITH DELETING QUIZ");
            }
        }

        /// <summary>
        /// Action to be performed when a particular quiz is selected. 
        /// Depending on the type of user, the respective page is navigated.
        /// </summary>
        /// <param name="id">The quiz id that was selected.</param>
        /// <returns>The respective View depending on the user.</returns>
        public ActionResult QuizSelected(int id)
        {
            GetValuesFromSession();
            User currentUser = new LoginManager().GetUser(userId);
            if (currentUser.Role.ToUpper() == "STAFF")
            {
                return RedirectToAction("QuizDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("AttemptQuiz", new { id = id });
            }
        }

        /**
        * Methods for Quiz Question!!
        */

        /// <summary>
        /// This will enlist all the questions present in a quiz.
        /// Essentially this is meant for viewing the questions in a quiz by the STAFF member.
        /// </summary>
        /// <param name="id">The quiz id for which the questions needs to be displayed.</param>
        /// <returns>View displaying all the questions.</returns>
        public ActionResult QuizDetails(int id)
        {
            GetValuesFromSession();
            ViewBag.IsCurrentSchedule = false;
            if (new ScheduleManager().IsScheduleTheLatest(scheduleId))
            {
                ViewBag.IsCurrentSchedule = true;
            }
            Quiz quiz = quizManager.getQuizById(id);
            ViewBag.CurrentQuizId = id;
            return View(quiz.Questions);
        }

        /// <summary>
        /// Create a new question in a quiz
        /// </summary>
        /// <param name="quizId"> The attached quiz ID.</param>
        /// <returns></returns>
        [SessionAuthorize(role = "staff")]
        public ActionResult CreateNewQuestion(int quizId)
        {
            ViewBag.CurrentQuizId = quizId;
            return View();
        }

        /// <summary>
        /// Adds a new question to the quizID that was passed as a parameter.
        /// </summary>
        /// <param name="quizId">The quiz ID to which the question needs to be added.</param>
        /// <param name="collection">The form collection containing all the values submitted by the user.</param>
        /// <returns>Back to the list of questions view.</returns>
        [HttpPost]
        [SessionAuthorize(role = "staff")]
        public ActionResult CreateNewQuestion(int quizId, FormCollection collection)
        {
            GetValuesFromSession();
            Quiz_Question question = GetQuestionFromCollection(collection);
            question.Quiz = quizManager.getQuizById(quizId);
            question.User = new LoginManager().GetUser(userId);
            quizManager.AddQuizQuestion(question);
            return RedirectToAction("QuizDetails", new { id = quizId });
        }

        [SessionAuthorize(role = "staff")]
        public ActionResult EditQuizQuestion(int id)
        {
            Quiz_Question question = quizManager.getQuestionById(id);
            return View(question);
        }

        [HttpPost]
        [SessionAuthorize(role = "staff")]
        public ActionResult EditQuizQuestion(FormCollection collection)
        {
            GetValuesFromSession();
            Quiz_Question question = GetQuestionFromCollection(collection);
            int questionId = int.Parse(collection["Quiz_Question_ID"]);
            question.Quiz_Question_ID = questionId;
            question.Quiz = quizManager.getQuestionById(questionId).Quiz;
            question.User = new LoginManager().GetUser(userId);
            quizManager.UpdateQuizQuestion(question);
            return RedirectToAction("QuizDetails", new { id = question.Quiz.Quiz_ID });
        }

        [SessionAuthorize(role = "staff")]
        public ActionResult DeleteQuestion(int id)
        {
            Quiz_Question question = quizManager.getQuestionById(id);
            int quizID = question.Quiz.Quiz_ID;
            // Delete the question from DB.
            quizManager.DeleteQuizQuestion(question.Quiz_Question_ID);
            return RedirectToAction("QuizDetails", new { id = quizID });
        }


        /// <summary>
        /// Allow the current user to attempt the quiz. 
        /// The view returned will allows the user to navigate through all the questions and submit the answers for each.
        /// </summary>
        /// <param name="id">The quiz id the current user is attempting.</param>
        /// <returns>The view which will allow the user to attempt the quiz.</returns>
        [SessionAuthorize(role = "student")]
        public ActionResult AttemptQuiz(int id)
        {
            ViewBag.HideSubmitButton = false;
            if (quizManager.IsEligibleToAttendQuiz(id, int.Parse(Session["User_ID"].ToString())))
            {
                Quiz quiz = quizManager.getQuizById(id);
                if(quiz.Questions.Count == 0)
                {
                    ViewBag.HideSubmitButton = true;
                }
                return View(quiz);
            }
            else
            {
                ViewBag.HideSubmitButton = true;
                return View();
            }
        }

        /// <summary>
        /// Get the view for a particular question
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQuizQuestion(Quiz quiz)
        {
            if (quiz.Questions.Count() > 0)
            {
                // Show the first question by default
                Quiz_Question question = sortQuestions(quiz.Questions)[0];
                return PartialView(viewName: "_GetQuizQuestion", model: question);
            }
            return Content("NO QUESTIONS FOUND!!");
        }

        /// <summary>
        /// Collect data for an answered quiz question.
        /// </summary>
        /// <returns>Also return the view for the next question.</returns>
        [HttpPost]
        public ActionResult GetQuizQuestion(int quizId, FormCollection collection)
        {
            GetValuesFromSession();
            ViewBag.HideSubmitButton = false;
            int currentQuestionId = int.Parse(collection["Quiz_Question_ID"]);
            Dictionary<int, string> answerDict;
            if (Session["Quiz_Answers"] != null)
            {
                // Add value to hashmap
                answerDict = Session["Quiz_Answers"] as Dictionary<int, string>;
            }
            else
            {
                // Create new hashmap
                answerDict = new Dictionary<int, string>();
            }
            // Set the values in session
            answerDict[currentQuestionId] = collection["answer"];
            Session["Quiz_Answers"] = answerDict;

            // Save the data in Session
            Quiz quiz = quizManager.getQuizById(quizId);

            int currentIndex = sortQuestions(quiz.Questions).FindIndex(a => a.Quiz_Question_ID == currentQuestionId);
            if ((currentIndex + 1) < quiz.Questions.Count())
            {
                Quiz_Question question = sortQuestions(quiz.Questions)[currentIndex + 1];
                return PartialView(viewName: "_GetQuizQuestion", model: question);
            }

            List<Question_Answer> answerList = new List<Question_Answer>();

            foreach (KeyValuePair<int, string> entry in answerDict)
            {
                // Read from Session & input in dB.
                Question_Answer answer = new Question_Answer();
                answer.Quiz_Question = quizManager.getQuestionById(entry.Key);
                answer.Answer = entry.Value;
                answer.User = new LoginManager().GetUser(userId);
                answerList.Add(answer);
            }
            quizManager.SaveQuizAnswer(answerList);
            ViewBag.HideSubmitButton = true;
            //return Json(new { url = Url.Action("Index") });
            //return Content("Quiz Submitted");
            return JavaScript("hideSubmitButtonAfterQuizCompletion()");
        }

        //this action will repost quiz on to the current schedule(last in list)
        [SessionAuthorize(role = "staff")]
        public ActionResult RepostQuiz(int id)
        {
            GetValuesFromSession();
            quizManager.RepostQuiz(id, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionAuthorize(role = "staff")]
        public ActionResult RepostQuiz(int id, string user)
        {
            if (quizManager.RepostQuiz(id, int.Parse(user)))
            {
                return Content("Repost Successful");
            }
            return Content("ERROR OCCURED");
        }

        [SessionAuthorize(role = "staff")]
        public ActionResult Statistics(int id)
        {
            List<Object> myModel = new List<object>();
            List<Quiz_Question> questions = quizManager.GetAllQuizQuestions(id).ToList();
            List<List<int>> stat = new List<List<int>>();
            foreach (var question in questions)
            {
                stat.Add(quizManager.GetStatisticsForQuestion(question.Quiz_Question_ID));
            }
            myModel.Add(questions);
            myModel.Add(stat);
            myModel.Add(quizManager.NumberOfEligibleStudentsForQuiz(id));
            return View(myModel);
        }
        /**********************************************************************/
        // Private Methods
        /**********************************************************************/
        /// <summary>
        /// Method to get the various values from Session.
        /// </summary>
        private void GetValuesFromSession()
        {
            scheduleId = int.Parse(Session["Schedule_ID"].ToString());
            courseId = int.Parse(Session["Course_ID"].ToString());
            userId = int.Parse(Session["User_ID"].ToString());
        }

        private List<Quiz_Question> sortQuestions(List<Quiz_Question> existing)
        {
            List<Quiz_Question> sortedList = existing.OrderBy(o => o.Quiz_Question_ID).ToList();
            return sortedList;
        }

        /// <summary>
        /// Method to get the question object from the FormCollection
        /// </summary>
        /// <param name="collection">The collection from which the question object needs to be retrieved.</param>
        /// <returns></returns>
        private Quiz_Question GetQuestionFromCollection(FormCollection collection)
        {
            Quiz_Question retrievedQuestion = new Quiz_Question();
            retrievedQuestion.Question = collection["Question"];
            retrievedQuestion.Answer = collection["Answer"];
            retrievedQuestion.Option_1 = collection["Option_1"];
            retrievedQuestion.Option_2 = collection["Option_2"];
            retrievedQuestion.Option_3 = collection["Option_3"];
            retrievedQuestion.Option_4 = collection["Option_4"];
            return retrievedQuestion;
        }
    }
}
