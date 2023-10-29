#define LOG
#undef LOG

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Web;

namespace BARKOCHBA
{
 public partial class WebForm:Page
 {
  //private static int n_questions_asked=0;
  private static Logic logic;

  public int n_decimals=2;
  const float threshold=0.8f;

  TextBox txtboxDomain=new TextBox();
  //TextBox txtboxCurrentQuestion=new TextBox();
  ListBox lstboxQuestions=new ListBox();
  ListBox lstboxSolutions=new ListBox();
  //TextBox txtboxCandidateSolution=new TextBox();
  ListBox lstboxProblems=new ListBox();
  ListBox lstboxDetails=new ListBox();

  protected void Page_Load(object sender,EventArgs e)
  {
   if(logic==null)
   {
    logic=new Logic();
    //throw new HttpException(500, "Internal Server Error");
    UpdateUserInterface();
   }
  }

  // Control PostBack Event(s)
  protected void btnY_Click(object sender,EventArgs e)
  {
   var reaction=logic.ReactionsAll.FirstOrDefault(r => r.Name=="Yes");
   SomeReactionClicked(reaction);
  }
  protected void btnIDK_Click(object sender, EventArgs e)
  {
   var reaction=logic.ReactionsAll.FirstOrDefault(r => r.Name=="IDontKnow");
   SomeReactionClicked(reaction);
  }
  protected void btnN_Click(object sender, EventArgs e)
  {
   var reaction=logic.ReactionsAll.FirstOrDefault(r => r.Name=="No");
   SomeReactionClicked(reaction);
  }
  protected void btnMY_Click(object sender, EventArgs e)
  {
   var reaction=logic.ReactionsAll.FirstOrDefault(r => r.Name=="MaybeYes");
   SomeReactionClicked(reaction);
  }
  protected void btnMN_Click(object sender, EventArgs e)
  {
   var reaction=logic.ReactionsAll.FirstOrDefault(r => r.Name=="MaybeNo");
   SomeReactionClicked(reaction);
  }
  public void SomeReactionClicked(ReactionModel reaction)
  {
   //Mutex mutex=new Mutex();
   //mutex.WaitOne();
   //logic.QuestionsNotAsked_get();
   //if(Logic.QuestionsNotAsked.Count==0)
   logic.db.QuestionsGetAll();
   if(logic.QuestionsAll.Where(q=>q.Asked==false).Count()==0)
    return;
   //var selectedIndex=lstboxQuestions.SelectedIndex;
   //var question=logic.QuestionsNotAsked[selectedIndex];
   QuestionModel question=logic.QuestionsAll.FirstOrDefault(q=>q.Text==current_question.Text);
   question.Asked=true;
   //logic.ReactionOnQuestion(question,reaction);
   question.ReactionValue=reaction.Value;
   //logic.db.UpdateQuestions(logic.QuestionsAll);
   //logic.db.QuestionsGetAll();
   //mutex.ReleaseMutex();
   //n_questions_asked++;
   //Debug.WriteLine("n_questions_asked: {0}",n_questions_asked);
   logic.Calculations();
   UpdateUserInterface();
  }
  public void btnTitle_Click(object sender,EventArgs e)
  {
   logic.StartNewProblem();
   UpdateUserInterface();
  }

  private void UpdateUserInterface()
  {
   UpdateDomain();
   UpdateNotAskedQuestions();
   UpdateCurrentQuestion();
   UpdateSolutions();
   UpdateCandidateSolution();
   UpdateProblems();
   UpdateDetails();
  }
  private void UpdateDomain()
  {
   DomainModel domain=new DomainModel();
   domain.Name=logic.DomainsFirst.Name;
   txtboxDomain.Text=domain.Name;
  }
  private void UpdateNotAskedQuestions()
  {
   Dictionary<string,float> question_w_reductivity=new Dictionary<string,float>();
   logic.QuestionsNotAsked_get();
   //foreach (var question in logic.QuestionsNotAsked)
   foreach (QuestionModel question in logic.QuestionsAll.Where(q=>q.Asked==false))
   {
    question_w_reductivity.Add(question.Text,(float)Math.Round(question.Reductivity,n_decimals));
   }
   FillListBoxFromDictionary(question_w_reductivity,lstboxQuestions);
  }
  private void UpdateCurrentQuestion()
  {
   TextBox txtboxCurrentQuestion=(TextBox)FindControl("current_question");
   txtboxCurrentQuestion.Text="";
   QuestionModel current_question;
   current_question=logic.QuestionsAll.Where(q=>q.Asked==false).OrderByDescending(q=>q.Reductivity).FirstOrDefault();
   if(current_question!=null)
    txtboxCurrentQuestion.Text=current_question.Text;
  }
  private void UpdateSolutions()
  {
   Dictionary<string, float> solution_w_prob = new Dictionary<string, float>();
   foreach (var solution in logic.SolutionsAll)
   {
    solution_w_prob.Add(solution.Text, (float) Math.Round(solution.CurrentProbability,n_decimals));
   }
   FillListBoxFromDictionary(solution_w_prob,lstboxSolutions);
  }
  private void UpdateCandidateSolution()
  {
   TextBox txtboxCandidateSolution=(TextBox)FindControl("candidate_solution");
   txtboxCandidateSolution.Text="";
   var selectedIndex = lstboxSolutions.SelectedIndex;
   if(selectedIndex>-1)
   {
    string[] fields=lstboxSolutions.Items[0].ToString().Split(':');
    float probability;
    if (float.TryParse(fields[0].Replace("[","").Replace("]",""),out probability))
    {
     // Conversion succeeded
     string strCandidateSolution;
     strCandidateSolution=fields[1];
     #if LOG
     Debug.WriteLine("number of not asked questions: {0}",lstboxQuestions.Items.Count);
     #endif
     if(probability>threshold||lstboxQuestions.Items.Count==0)
     {
      txtboxCandidateSolution.Text=strCandidateSolution;
     }
    }
    else
    {
     // Conversion failed
    }
   }
   #if LOG
   Debug.WriteLine("textbox Text set to '{0}'",txtboxCandidateSolution.Text);
   #endif
  }
  private void UpdateProblems()
  {
   List<string> list_problems=new List<string>();
   foreach (var problem in logic.ProblemsAll)
   {
    string strSolution="";
    foreach(SolutionModel solution in logic.SolutionsAll)
    {
     if(problem.SolutionId==solution.Id)
     {
      strSolution=solution.Text;
     }
    }
    list_problems.Add("S:"+strSolution);
   }
   FillListBoxFromList(list_problems,lstboxProblems);
  }
  private void UpdateDetails()
  {
   string[] items={};
   if(lstboxProblems.Items.Count>0)
   {
    items=lstboxProblems.SelectedItem.ToString().Split(':').ToArray();
    int problem_id;
    int.TryParse(items[0],out problem_id);
    List<string> list_questions_reactions=new List<string>();
    foreach(var detail in logic.DetailsAll.Where(d => d.ProblemId==problem_id))
    {
     string strQuestion="";
     string strReaction="";
     foreach(QuestionModel question in logic.QuestionsAll)
     {
      if(detail.QuestionId==question.Id)
      {
       strQuestion=question.Text;
      }
      var reactions=logic.ReactionsAll.FirstOrDefault(r =>r.Id==detail.ReactionId);
      strReaction=reactions.Name;
     }
     list_questions_reactions.Add("Q:"+strQuestion+" R:"+strReaction);
    }
    FillListBoxFromList(list_questions_reactions,lstboxDetails);
   }
  }
  private void FillListBoxFromDictionary(Dictionary<string, float> dictData, ListBox lstbox)
  {
   lstbox.Items.Clear();
   string[] lstboxItems={};
   lstboxItems=dictData.Select(Item => $"[{Item.Value}]:{Item.Key}").ToArray();
   ListItem[] lstItems=lstboxItems.Select(s => new ListItem(s)).ToArray();
   lstbox.Items.AddRange(lstItems);
   if (lstbox.Items.Count > 0)
   {
    lstbox.SelectedIndex = 0;
   }
  }
  private void FillListBoxFromList(List<string> listData, ListBox lstbox)
  {
   lstbox.Items.Clear();
   string[] lstboxItems={};
   lstboxItems=listData.Select((Item,iItem) => $"{iItem+1}:{Item}").ToArray();
   ListItem[] lstItems=lstboxItems.Select(s => new ListItem(s)).ToArray();
   lstbox.Items.AddRange(lstItems);
   if (lstbox.Items.Count > 0)
   {
    lstbox.SelectedIndex = 0;
   }
  }
 }
}