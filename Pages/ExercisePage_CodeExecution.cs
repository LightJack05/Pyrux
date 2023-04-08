using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using System.Diagnostics;
using Pyrux.UserEndExceptions;
using Pyrux.Pages.ContentDialogs.ExceptionPages;
using Pyrux.Pages.ContentDialogs;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{

    /// <summary>
    /// Start the ArbitraryCodeExecution method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnStart_Click(object sender, RoutedEventArgs e)
    {
        if (ActiveLevel.IsBuiltIn)
        {
            ResetLayoutToStart();
        }
        ArbitraryCodeExecution();
    }


    /// <summary>
    /// Execute the code that has been written by the user.
    /// Script is taken from ActiveLevel.Script instance of PyruxLevel class.
    /// Will add variables of the basic movement methods to the python environment.
    /// NOTE: Code will execute in another thread to keep UI responsive.
    /// </summary>
    private async void ArbitraryCodeExecution()
    {
        //TODO: Add a mockup library for the code editor's Autocomplete/Intellisense.
        //TODO: Add non-execution block that is used to import the mockup library that is removed once the file is imported into the program.

        Exception thrownException = null;
        string errorStackTrace = "";

        if (!PythonScriptRunning)
        {
            btnStart.IsEnabled = false;
            PythonScriptRunning = true;
            string pythonCode = BuildPythonCode();
            ScriptEngine scriptEngine = Python.CreateEngine();
            ScriptScope scriptScope = scriptEngine.CreateScope();
            scriptScope.SetVariable("TurnLeft", () => this.ActiveLevel.TurnLeft());
            scriptScope.SetVariable("TurnRight", () => this.ActiveLevel.TurnRight());
            scriptScope.SetVariable("GoForward", () => this.ActiveLevel.GoForward());
            scriptScope.SetVariable("TakeScrew", () => this.ActiveLevel.TakeScrew());
            scriptScope.SetVariable("PlaceScrew", () => this.ActiveLevel.PlaceScrew());
            scriptScope.SetVariable("WallAhead", () => this.ActiveLevel.WallAhead());
            scriptScope.SetVariable("ScrewThere", () => this.ActiveLevel.ScrewThere());
            scriptScope.SetVariable("stackTrace", errorStackTrace);
            scriptScope.SetVariable("scriptCode", ActiveLevel.Script.ReplaceLineEndings().Split(Environment.NewLine));
            


            await Task.Factory.StartNew(() =>
            {
                try
                {
                    scriptEngine.Execute(pythonCode, scriptScope);
                }
                catch (ExecutionCancelledException) { }
                catch (Exception ex)
                {
                    thrownException = ex;
                    errorStackTrace = scriptScope.GetVariable<string>("stackTrace");
                }

                ExercisePage.Instance.PythonScriptRunning = false;

                DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
                dispatcherQueue.TryEnqueue(() =>
                {
                    ExercisePage.Instance.btnStart.IsEnabled = true;
                });
            });
        }
        if (thrownException != null)
        {
            ShowUserEndExceptionDialogue(thrownException, errorStackTrace);
        }
        else
        {
            if(ActiveLevel.MapLayout.Equals(ActiveLevel.GoalMapLayout))
            {
                ActiveLevel.Completed = true;
                ShowLevelCompletedDialogue();
            }
        }
    }

    private async void ShowLevelCompletedDialogue()
    {
        ContentDialog userEndExceptionDialogue = new();
        userEndExceptionDialogue.XamlRoot = this.Content.XamlRoot;
        userEndExceptionDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        userEndExceptionDialogue.Title = "Level Completed!";
        

        userEndExceptionDialogue.SecondaryButtonText = "Level Selection";
        userEndExceptionDialogue.CloseButtonText = "Retry";
        userEndExceptionDialogue.DefaultButton = ContentDialogButton.Primary;
        LevelCompletedDialogue dialogueContent = new();
        userEndExceptionDialogue.Content = dialogueContent;
        bool nextLevelAvailable = CheckNextLevelAvailable();

        if (nextLevelAvailable)
        {
            userEndExceptionDialogue.PrimaryButtonText = "Next Level";
        }
        else
        {
            userEndExceptionDialogue.PrimaryButtonText = "Next Level";
            userEndExceptionDialogue.IsPrimaryButtonEnabled = false;
        }
        ContentDialogResult result = await userEndExceptionDialogue.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            NavigateToNextLevel();
        }
        else if (result == ContentDialogResult.Secondary)
        {
            NavigateToLevelSelection();
        }


    }

    private static bool CheckNextLevelAvailable()
    {
        bool nextLevelAvailable = false;
        if (StaticDataStore.ActiveLevel.IsBuiltIn)
        {
            int levelIndex = StaticDataStore.BuiltInLevels.FindIndex(x => x.Equals(StaticDataStore.ActiveLevel));
            int nextLevelIndex = levelIndex + 1;
            if (StaticDataStore.BuiltInLevels.Count - 1 >= nextLevelIndex)
            {
                nextLevelAvailable = true;
            }
        }
        else
        {
            int levelIndex = StaticDataStore.UserCreatedLevels.FindIndex(x => x.LevelName.Equals(StaticDataStore.ActiveLevel.LevelName));
            int nextLevelIndex = levelIndex + 1;
            if (StaticDataStore.UserCreatedLevels.Count - 1 >= nextLevelIndex)
            {
                nextLevelAvailable = true;
            }
        }

        return nextLevelAvailable;
    }

    private async void ShowUserEndExceptionDialogue(Exception exception, string stackTrace)
    {
        ContentDialog userEndExceptionDialogue = new();
        userEndExceptionDialogue.XamlRoot = this.Content.XamlRoot;
        userEndExceptionDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        userEndExceptionDialogue.Title = "An exception has occoured";
        userEndExceptionDialogue.PrimaryButtonText = "Close";
        userEndExceptionDialogue.DefaultButton = ContentDialogButton.Primary;
        UserExceptionDialogue dialogueContent = new UserExceptionDialogue();
        dialogueContent.DialogueException = exception;
        dialogueContent.DialogueStackTrace = stackTrace;
        userEndExceptionDialogue.Content = dialogueContent;
        _ = await userEndExceptionDialogue.ShowAsync();
    }

    private string BuildPythonCode()
    {
        string pythonCode = """
import sys
try:
    exec(
""" 
+
"\"\"\""
+
ActiveLevel.Script
+
"\"\"\""+")"+ Environment.NewLine +
"""
except Exception as ex:
    exc_type, exc_value, exc_traceback = sys.exc_info()
    tb = []
    while exc_traceback:
        tb.append(exc_traceback)
        exc_traceback = exc_traceback.tb_next
    tb.pop(0)
    tb_str = f"{exc_type.__name__}: {exc_value}\n"
    tb_str += "(If this is a Pyrux exception, see above for exception type.)\n\n"
    tb_str += "Traceback (most recent call last):\n"
    tb_str += ''.join(['At line {}, in {}\n     Code: {}\n'.format(int(tb_obj.tb_lineno), tb_obj.tb_frame.f_code.co_name,scriptCode[tb_obj.tb_lineno - 1].strip()) for tb_obj in tb])

    stackTrace = tb_str
    raise ex
""";
        pythonCode = pythonCode.ReplaceLineEndings();
        return pythonCode;
    }

}