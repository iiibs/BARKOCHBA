#define LOG
//#undef LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BARKOCHBA
{
 public class QuestionModel
 {
  public int Id;
  public string Text;
  public bool Asked;
  public int ReactionId;
  public double Denominator { get; set; } // calculated field
  public double ReactionValue { get; set; } // calculated field
  public double Reductivity;
 }
 public class ReactionModel
 {
  public int Id; //1: Yes, 2: MayBeYes, 3: IDontKnow, 4: MaybeNo, 5: No
  public double Value { get; set; }
  //public float Weight;
  public string Name;
  public string Text;
  public double Probability { get; set; } // calculated field
  public double Information { get; set; } // calculated field
  public double Reductivity { get; set; } // calculated field
 }
 public class SolutionModel
 {
  public int Id;
  public int DomainId;
  public string Text;
  public double PriorProbability;
  public double CurrentProbability;
  public double Numerator { get; set; } // calculated field
  public double ReactionValue { get; set; } // calculated field
  public double Similarity { get; set; } // calculated field
  public double WeightedSimilarity { get; set; } // calculated field
  //public float Probability;
 }
 public class ProblemModel
 {
  public int Id;
  public int DomainId;
  public int SolutionId;
 }
 public class DetailModel
 {
  public int Id;
  public int ProblemId;
  public int SolutionId;
  public int QuestionId;
  public int ReactionId;
  public double ReactionValue { get; set; }
 }
 /*
 public class EpisodeModel
 {
  public int Id;
  public int ProblemId;
  public int SolutionId;
  public int QuestionId;
  public int ReactionId;
 }
 */
 public class DomainModel
 {
  public int Id;
  public string Name;
 }
 public class Logic
 {
  public Logic()
  {
   GetAllTables();
   Calculations();
  }
  public Database db=new Database();
  public DomainModel DomainsFirst
  {
   get
   {
    DomainModel domain=db.GetFirstDomain();
    return domain;
   }
  }
  public List<QuestionModel> QuestionsAll;
  public List<ReactionModel> ReactionsAll;
  public List<SolutionModel> SolutionsAll;
  public List<ProblemModel> ProblemsAll;
  public List<DetailModel> DetailsAll;
  public List<DomainModel> DomainsAll;
  //this is static because the web page life cycle recreates logic and it is overwritten
  public static Dictionary<QuestionModel,ReactionModel> AskedQuestionsReactions=new Dictionary<QuestionModel,ReactionModel>();
  //this is not static because this will always be calculated from the above AskedQuestionsReactions
  public List<QuestionModel> QuestionsNotAsked=new List<QuestionModel>();
  public void QuestionsNotAsked_get()
  {
   //get
   //{
    //lock(lockObject)
    //{
    //List<QuestionModel> lstQuestionsNotAsked;
    /*
    StepsAll=db.StepsGetAll();
    #if LOG
    Debug.WriteLine("QuestionsNotAsked calculation, steps: {0}",StepsAll.Count);
    #endif
    AskedQuestionsReactions.Clear();
    foreach(StepModel step in StepsAll)
    {
     QuestionModel question=new QuestionModel();
     question.Id=step.QuestionId;
     ReactionModel reaction=new ReactionModel();
     reaction.Id=step.ReactionId;
     Choices.Add(question,reaction);
    }
    */
    var asked_questions_reactions=AskedQuestionsReactions.Keys.ToList().Select(asked_question_reaction => asked_question_reaction.Id).ToList();
    //#if LOG
    //Debug.WriteLine("there were {0} questions asked and reacted for the current problem so far",asked_questions_reactions.Count);
    //foreach(var question_reaction in asked_questions_reactions)
    //{
    // Debug.WriteLine("question_reaction {0}",question_reaction);
    //}
    //#endif
    /*
    var steps=StepsAll.ToList().Select(step => step.QuestionId).ToList();
    */
    var except_questions=QuestionsAll.Where(except_question => asked_questions_reactions.Contains(except_question.Id)).ToList();
    QuestionsNotAsked=QuestionsAll.Except(except_questions).ToList();
    #if LOG
    Debug.WriteLine("QuestionsNotAsked has {0} items",QuestionsNotAsked.Count);
    #endif
    //return lstQuestionsNotAsked;
    //}
   //}
  }
  /**/

  public void ReactionOnQuestion(QuestionModel question,ReactionModel reaction)
  {
   AskedQuestionsReactions.Add(question,reaction);
   Debug.WriteLine("{0} questions asked and reacted to",AskedQuestionsReactions.Count);
  }

  public double entropy { get; set; } // calculated field
  public double denominator { get; set; } // calculated field
  /*
  public void ResetAllTables()
  {
   db.ResetQuestions(QuestionsAll);
  }
  */
  public void GetAllTables()
  {
   QuestionsAll=db.QuestionsGetAll();
   ReactionsAll=db.ReactionsGetAll();
   SolutionsAll=db.SolutionsGetAll();
   ProblemsAll=db.ProblemsGetAll();
   DetailsAll=db.DetailsGetAll();
   DomainsAll=db.DomainsGetAll();
  }
  public void Calculations()
  {
   //CalcSolutionsProbabilityAndSortByIt();
   ProbabilitiesBeforeCase();
   ProbabilitiesDuringCase();
   //CalcNotAskedQuestionsReductivityAndSortByIt();
   Reductivities();
  }
  private void ProbabilitiesBeforeCase()
  {
   entropy=0;
   denominator=ProblemsAll.Select(p=>p.SolutionId).Distinct().Count();
   foreach (SolutionModel solution in SolutionsAll)
   {
    solution.Numerator=ProblemsAll.Where(p=>p.SolutionId==solution.Id).Count();
    double probability=0;
    if(denominator>Single.Epsilon)
     probability=solution.Numerator/denominator;
    solution.CurrentProbability=probability;
    solution.PriorProbability=probability;
    double information=0;
    if(probability>Single.Epsilon)
     information=Math.Log(probability,2);
    double solution_entropy;
    solution_entropy=probability*information;
    entropy+=solution_entropy;
   }
   SolutionsAll=SolutionsAll.OrderByDescending(s=>s.CurrentProbability).ToList();
  }
  private void ProbabilitiesDuringCase()
  {
   foreach(QuestionModel question in QuestionsAll.Where(q=>q.Asked==true))
   {
    question.Denominator=DetailsAll.Where(d=>d.QuestionId==question.Id).Count();
    double sum_weighted_similarities=0;
    foreach(SolutionModel solution in SolutionsAll)
    {
     solution.Numerator=DetailsAll.Where(d=>d.QuestionId==question.Id&&d.SolutionId==solution.Id).Count();
     solution.ReactionValue=0;
     solution.Similarity=0;
     foreach(DetailModel detail in DetailsAll)
     {
      if(detail.SolutionId==solution.Id&&detail.QuestionId==question.Id)
      {
       solution.ReactionValue+=detail.ReactionValue;
       solution.Similarity+=(4-Math.Abs(detail.ReactionValue-question.ReactionValue))/4;
      }
     }
     solution.WeightedSimilarity=solution.Similarity*solution.PriorProbability;
     sum_weighted_similarities+=solution.WeightedSimilarity;
    }
    foreach(SolutionModel solution in SolutionsAll)
    {
     double probability=0;
     if(sum_weighted_similarities>Single.Epsilon)
      probability=solution.WeightedSimilarity/sum_weighted_similarities;
     solution.CurrentProbability=probability;
     solution.PriorProbability=probability;
    }
   }
   SolutionsAll=SolutionsAll.OrderByDescending(s=>s.CurrentProbability).ToList();
  }
  private void Reductivities()
  {
   foreach(QuestionModel question in QuestionsAll.Where(q=>q.Asked==false))
   {
    question.Reductivity=0;
    double denominator=DetailsAll.Where(d=>d.QuestionId==question.Id).Count();
    foreach(ReactionModel reaction in ReactionsAll)
    {
     double numerator=DetailsAll.Where(d=>d.QuestionId==question.Id&&d.ReactionId==reaction.Id).Count();
     double probability=0;
     if(denominator>Single.Epsilon)
      probability=numerator/denominator;
     reaction.Probability=probability;
     double information=0;
     if(probability>Single.Epsilon)
      information=-Math.Log(probability,2);
     reaction.Information=information;
     double reductivity;
     reductivity=probability*information;
     reaction.Reductivity=reductivity;
     question.Reductivity+=reaction.Reductivity;
    }
   }
   QuestionsAll=QuestionsAll.OrderByDescending(q=>q.Reductivity).ToList();
  }

  /*
  private float CalcPriorSolutionProbability(SolutionModel solution)
  {
   //number of problems with accepted solution
   int n_problems=db.CountRecords("problems");
   //number of possible solutions
   int n_solutions=db.CountRecords("solutions");
   //number of problems with given solution accepted
   int n_problems_where_solution=db.CountDetailsWhereSolution(solution.Id);
   //a priori probability of a possible solution
   float apriori=0.0f;
   //if there were already some problems resolved
   if(n_problems>0)
    apriori=(float)n_problems_where_solution/n_problems;
   else
    //if no problem was yet solved, then 
    // the a priori probability of a possible solution is estimated as 1 per the number of all possible solutions
    apriori=1.0f/n_solutions;
   return apriori;
  }
  */
  /*
  public class EqualSolutions:IEqualityComparer<EpisodeModel>
  {
   public bool Equals(EpisodeModel x,EpisodeModel y)
   {
    return x.SolutionId==y.SolutionId;
   }

   public int GetHashCode(EpisodeModel obj)
   {
    return obj.SolutionId.GetHashCode();
   }
  }
  */
  /*
  private float CalcSolutionProbability(SolutionModel solution)
  {

   //all episodes in all sessions that already happened in this domain
   List<EpisodeModel> episodes=db.EpisodesGetAll();
   int n_episodes=episodes.Count;
   #if LOG
   Console.WriteLine("{0} episodes all",n_episodes);
   foreach(var episode in episodes)
    Console.WriteLine("Id: {0}, problem: {1}, solution: {2}, question: {3}, reaction {4}",
     episode.Id,episode.ProblemId,episode.SolutionId,episode.QuestionId,episode.ReactionId);
   #endif

   //episodes where this solution was accepted
   List<EpisodeModel> episodes_where_solution=
    episodes.Where(e => e.SolutionId==solution.Id).ToList();
   int n_episodes_where_solution=episodes_where_solution.Count;
   #if LOG
   Console.WriteLine("{0} episodes where solution {1}",n_episodes_where_solution,solution.Text);
   foreach(var episode in episodes_where_solution)
    Console.WriteLine("Id: {0}, problem: {1}, solution: {2}",
     episode.Id,episode.ProblemId,episode.SolutionId);
   #endif

   float probability=0.0f;

   foreach(var question_reaction in AskedQuestionsReactions)
   {
    #if LOG
    Console.WriteLine("\n{0}->{1}",question_reaction.Key.Text,question_reaction.Value.Text);
    #endif

    //problems that have this solution
    List<EpisodeModel> problems_where_solution=
     episodes.Where(e => e.SolutionId==solution.Id).Distinct(new EqualSolutions()).ToList();
    int n_problems_where_solution=problems_where_solution.Count;
    #if LOG
    Console.WriteLine("{0} problems where solution {1}",n_problems_where_solution,solution.Text);
    foreach(var problem in problems_where_solution)
     Console.WriteLine("problem: {0}, solution: {1}",problem.ProblemId,problem.SolutionId);
    #endif

    //episodes of 'problems', where 'solution' was accepted and 'question' got 'reaction'
    QuestionModel question=question_reaction.Key;
    ReactionModel reaction=question_reaction.Value;
    List<EpisodeModel> episodes_where_solution_question_reaction=
     episodes_where_solution.Where(e => (e.QuestionId==question.Id)&&(e.ReactionId==reaction.Id)).ToList();
    int n_episodes_where_solution_question_reaction=episodes_where_solution_question_reaction.Count;
    #if LOG
    Console.WriteLine("{0} episodes where solution {1} and question {2} got reaction {3}",
     n_episodes_where_solution_question_reaction,solution.Text,question.Text,reaction.Text);
    foreach(var episode in episodes_where_solution_question_reaction)
     Console.WriteLine("problem: {0}, solution: {1}, question: {2}, reaction: {3}",
      episode.ProblemId,episode.SolutionId,episode.QuestionId,episode.ReactionId);
    #endif

    //episodes of 'problems', where 'question' got 'reaction'
    List<EpisodeModel> episodes_where_question_reaction=
     episodes.Where(e => (e.QuestionId==question.Id)&&(e.ReactionId==reaction.Id)).ToList();
    int n_episodes_where_question_reaction=episodes_where_question_reaction.Count;
    #if LOG
    Console.WriteLine("{0} episodes where question {1} got reaction {2}",
     n_episodes_where_question_reaction,question.Text,reaction.Text);
    foreach(var episode in episodes_where_question_reaction)
     Console.WriteLine("episode: {0}, question: {1}, reaction: {2}",
      episode.Id,episode.QuestionId, episode.ReactionId);
    #endif

    //take care of division by zero
    if(n_episodes_where_question_reaction>Single.Epsilon)
     probability+=(float)n_episodes_where_solution_question_reaction/n_episodes_where_question_reaction;
   }

   return probability;
  }
  */
  /*
  public void CalcSolutionsProbabilityAndSortByIt()
  {
   //number of steps (question+reaction pairs) during the current problem solving process
   //int n_steps=db.CountRecords("steps");
   int n_asked_questions_reactions=AskedQuestionsReactions.Count;
   #if LOG
   Debug.WriteLine("\nthere are {0} questions asked and reacted for the current problem",AskedQuestionsReactions.Count);
   foreach(var question_reaction in AskedQuestionsReactions)
    Debug.WriteLine("question: {0}, reaction: {1}",question_reaction.Key.Text,question_reaction.Value.Text);
   #endif
   //we will normalize the list using "unit vector normalization" (also called L1 normalization)
   // so the sum is calculated together with the individual values
   float sum_probability=0.0f;
   foreach(var solution in SolutionsAll)
   {
    //if no steps (question+reaction) happened yet for the currently open problem
    //if(n_steps==0)
    if(n_asked_questions_reactions==0)
     //then the a priori probability will be calculated and returned
     solution.Probability=CalcPriorSolutionProbability(solution);
    else
     //else the probability will be calculated based on previous problem solvings
     solution.Probability=CalcSolutionProbability(solution);
    #if LOG
    Debug.WriteLine("probability of possible solution {0} -> {1}",solution.Text,solution.Probability);
    #endif
    //the sum is updated here
    sum_probability+=solution.Probability;
   }
   //take care of the case when all probabilities are zero
   if(sum_probability>Single.Epsilon)
    //another cycle, now the normalized values are overwriting the earlier values
    foreach(var solution in SolutionsAll)
    {
     solution.Probability/=sum_probability;
    }
   //here the probability is either normalized, or is zero for each question
   //sort the list of solutions descending by probability
   SolutionsAll=SolutionsAll.OrderByDescending(solutions => solutions.Probability).ToList();
  }
  */
  /*
  private void CalcNotAskedQuestionsReductivityAndSortByIt()
  {
   //no need for this calculation if there is no more question or if there is no solution currently
   if(QuestionsNotAsked.Count==0||SolutionsAll.Count==0)
    return;
   //we will normalize the list using "unit vector normalization" (also called L1 normalization)
   // so the sum is calculated together with the individual values
   double sum_reductivity=0.0f;
   foreach(var question in QuestionsNotAsked)
   {
    question.Reductivity=CalcNotAskedQuestionReductivity(question);
    //the sum is updated here
    sum_reductivity+=question.Reductivity;
   }
   //take care of the case when all reductivities are zero
   if(sum_reductivity>Single.Epsilon)
    //another cycle, now the normalized values are overwriting the earlier values
    foreach(var question in QuestionsNotAsked)
    {
     question.Reductivity/=sum_reductivity;
    }
   //here the reductivity is either normalized, or is zero for each question
   //sort the list of questions descending by reductivity
   QuestionsAll=QuestionsAll.OrderByDescending(questions => questions.Reductivity).ToList();
  }
  */
  /*
  private double CalcNotAskedQuestionReductivity(QuestionModel question)
  {
   / *
   //all episodes in all sessions that already happened in this domain
   List<EpisodeModel> episodes=db.EpisodesGetAll();
   int n_episodes=episodes.Count;
   #if LOG
   Console.WriteLine("{0} episodes all",n_episodes);
   foreach(var episode in episodes)
    Console.WriteLine("Id: {0}, problem: {1}, solution: {2}, question: {3}, reaction {4}",
     episode.Id,episode.ProblemId,episode.SolutionId,episode.QuestionId,episode.ReactionId);
   #endif

   //P(R|Q)=P(R AND Q)/P(Q) -> P(R|Q)=P(Q AND R)/P(Q)
   //P(R|Q)=N(Q AND R)/N(Q)
   //P(R|Q)=N(Q AND R)/N(Q) -> PRQ=NQR/NQ
   //NRQ=1.00*NRYes+0.75*NRMaybeYes+0.50*NRIDK+0.25*NRMybeNo+0.00*NR
   //H(Q)=PRQ*log2(PRQ)
   //H(Q)=1.00*PRYes*log2(PRYes)+0.5*PRMaybeYes*log2(PRMaybeYes)+0.0*PRIDK*log2(PRIDK)+0.5*PRMaybeNo*log2(PRMaybeNo)+*PRNo*log2(PRNo)
   / *
   H(PYes)      = 1.00 * PYes      * log2(PYes)
   H(MaybeYes)  = 0.50 * PMaybeYes * log2(PMaybeYes)
   H(IDontKnow) = 0.00 * PMaybeYes * log2(PIDontKnow)
   H(MaybeNo)   = 0.50 * PMaybeNo  * log2(PMaybeNo)
   H(PNo)       = 1.00 * PYes      * log2(PYes)
   * /
   * /
   double HQ=0.0f;
   / *
   foreach(ReactionModel reaction in ReactionsAll)
   {

    //P(RQ)=N(QR)/N(Q)

    //N(QR)
    List<EpisodeModel> episodes_where_question_reaction=
     episodes.Where(e => (e.QuestionId==question.Id)&&(e.ReactionId==reaction.Id)).ToList();
    int n_episodes_where_question_reaction=episodes_where_question_reaction.Count;
    #if LOG
    Console.WriteLine("{0} episodes where question {1} got reaction {2}",
     n_episodes_where_question_reaction,question.Text,reaction.Text);
    foreach(var episode in episodes_where_question_reaction)
     Console.WriteLine("problem: {0}, question: {1}, reaction: {2}",
      episode.ProblemId,episode.QuestionId,episode.ReactionId);
    #endif
    //float NQR=flopsdb.CountDetailsWhereQuestionReaction(question.Id,reaction.Id);
    float NQR=n_episodes_where_question_reaction;

    //N(Q)
    List<EpisodeModel> episodes_where_question=
     episodes.Where(e => e.QuestionId==question.Id).ToList();
    int n_episodes_where_question=episodes_where_question.Count;
    #if LOG
    Console.WriteLine("{0} episodes where question {1} was asked",
     n_episodes_where_question_reaction,question.Text);
    foreach(var episode in episodes_where_question)
     Console.WriteLine("problem: {0}, question: {1}",
      episode.ProblemId,episode.QuestionId);
    #endif
    //int NQ=flopsdb.CountDetailsWhereQuestion(question.Id);
    float NQ=n_episodes_where_question;

    float PQRj=0.0f;
    if(NQR>0)
     PQRj=(float)NQR/NQ;
    if(PQRj>Single.Epsilon)
     HQ+=reaction.Weight*PQRj*(float)Math.Log(PQRj,2);
   }
   * /
   return -HQ;
  }
  */

  public void ForceProblemFinishWithSolution(DomainModel domain,SolutionModel solution)
  {
   /*
   flopsdb.ProblemAdd(domain,solution,CurrentQuestionsReactions);
   ForceNewProblem();
   */
  }

  public void StartNewProblem()
  {
   //AskedQuestionsReactions.Clear();
   //ResetAllTables();
   GetAllTables();
   Calculations();
  }
 }
}