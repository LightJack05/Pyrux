﻿using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Pyrux.Pages.ContentDialogs;
using Pyrux.Pages.ContentDialogs.ExceptionPages;
using Pyrux.UserEndExceptions;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{
    public bool IsStepModeEnabled { get; set; }
    public bool IsNextStepRequested { get; set; }
    public bool ExecutionRanState { get; set; } = false;
    private Task _pythonThread = null;
    public CancellationTokenSource PythonCancellationTokenSource;
    public CancellationToken PythonCancellationToken;

    /// <summary>
    /// Start the ArbitraryCodeExecution method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnStart_Click(object sender, RoutedEventArgs e)
    {
        if (!PythonScriptRunning)
        {

            btnStart.Content = new SymbolIcon(Symbol.Pause);
            ToolTipService.SetToolTip(btnStart, "Pause");
            ExecutionRanState = true;
            btnStep.IsEnabled = false;
            btnWallTool.IsEnabled = false;
            btnRotate.IsEnabled = false;
            btnPlayerTool.IsEnabled = false;
            btnChipTool.IsEnabled = false;
            ArbitraryCodeExecution();
        }
        else
        {
            if (IsStepModeEnabled)
            {
                IsStepModeEnabled = false;
                IsNextStepRequested = true;
                btnStep.IsEnabled = false;
                btnStart.Content = new SymbolIcon(Symbol.Pause);
                ToolTipService.SetToolTip(btnStart, "Pause");
            }
            else
            {
                IsStepModeEnabled = true;
                btnStep.IsEnabled = true;
                btnStart.Content = new SymbolIcon(Symbol.Play);
                ToolTipService.SetToolTip(btnStart, "Continue");
            }
        }
        System.Diagnostics.Debug.WriteLine(IsStepModeEnabled);

    }


    private void btnStep_Click(object sender, RoutedEventArgs e)
    {

        if (!PythonScriptRunning)
        {
            IsNextStepRequested = true;
            IsStepModeEnabled = true;
            ExecutionRanState = true;
            btnWallTool.IsEnabled = false;
            btnRotate.IsEnabled = false;
            btnPlayerTool.IsEnabled = false;
            btnChipTool.IsEnabled = false;
            ArbitraryCodeExecution();
        }
        else
        {
            IsNextStepRequested = true;
        }
    }

    /// <summary>
    /// Execute the code that has been written by the user.
    /// Script is taken from ActiveLevel.Script instance of PyruxLevel class.
    /// Will add variables of the basic movement methods to the python environment.
    /// NOTE: Code will execute in another thread to keep UI responsive.
    /// </summary>
    private async void ArbitraryCodeExecution()
    {
        DataManagement.Restrictions.FunctionCallCount.ResetCallCount();
        Exception thrownException = null;
        string errorStackTrace = "";

        if (!PythonScriptRunning)
        {
            PythonScriptRunning = true;
            btnReset.Content = new SymbolIcon(Symbol.Stop);
            ToolTipService.SetToolTip(btnReset, "Stop");

            string pythonCode = BuildPythonCode();
            ScriptEngine scriptEngine = Python.CreateEngine();
            SetupPythonConsoleOutput(scriptEngine);

            ScriptScope scriptScope = scriptEngine.CreateScope();
            scriptScope.SetVariable("TurnLeft", () => this.ActiveLevel.TurnLeft());
            scriptScope.SetVariable("TurnRight", () => this.ActiveLevel.TurnRight());
            scriptScope.SetVariable("GoForward", () => this.ActiveLevel.GoForward());
            scriptScope.SetVariable("TakeChip", () => this.ActiveLevel.TakeChip());
            scriptScope.SetVariable("PlaceChip", () => this.ActiveLevel.PlaceChip());
            scriptScope.SetVariable("WallAhead", () => this.ActiveLevel.WallAhead());
            scriptScope.SetVariable("ChipThere", () => this.ActiveLevel.ChipThere());
            scriptScope.SetVariable("stackTrace", errorStackTrace);
            scriptScope.SetVariable("scriptCode", ActiveLevel.Script.ReplaceLineEndings().Split(Environment.NewLine));


            PythonCancellationTokenSource = new();
            PythonCancellationToken = PythonCancellationTokenSource.Token;


            try
            {
                _pythonThread = Task.Factory.StartNew(() =>
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
                }, PythonCancellationToken);
                await _pythonThread.WaitAsync(PythonCancellationToken);
            }
            catch (TaskCanceledException) { ResetLayoutToStart(); }
            catch (ExecutionCancelledException) { ResetLayoutToStart(); }
            ExercisePage.Instance.PythonScriptRunning = false;
            btnReset.Content = new SymbolIcon(Symbol.Refresh);
            ToolTipService.SetToolTip(btnReset, "Reset layout");
            btnStart.Content = new SymbolIcon(Symbol.Play);
            ToolTipService.SetToolTip(btnStart, "Execute");
            btnStart.IsEnabled = false;
            btnStep.IsEnabled = false;
            IsStepModeEnabled = false;

            if (thrownException != null)
            {
                ShowUserEndExceptionDialogue(thrownException, errorStackTrace);
            }
            else
            {
                if (StaticDataStore.ActiveLevel.MapLayout.MatchGoalLayout(ActiveLevel.GoalMapLayout))
                {
                    if (!StaticDataStore.ActiveLevel.CheckCompletionRestrictionsSatisfied())
                    {
                        ShowLevelFailedDueToRestrictionDialog();
                    }
                    else
                    {
                        ActiveLevel.Completed = true;
                        ShowLevelCompletedDialogue();
                    }
                }
            }

            if (PyruxSettings.AutoRestartOnFinish && !ActiveLevel.Completed)
            {
                if (PyruxSettings.AddDelayBeforeAutoReset)
                {
                    await Task.Run(() =>
                    {
                        Thread.Sleep(PyruxSettings.DelayBeforeAutoReset);

                    });
                    if (!PythonScriptRunning && !btnStart.IsEnabled && !btnStep.IsEnabled)
                    {
                        ExecutionRanState = false;
                        PrepareToolSelection();
                        ResetLayoutToStart();
                    }
                }
                else
                {
                    ExecutionRanState = false;
                    PrepareToolSelection();
                    ResetLayoutToStart();
                }
            }
        }
    }



    public void SetupPythonConsoleOutput(ScriptEngine scriptEngine)
    {
        ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
    }

    public void CancelScriptExecution()
    {
        if (PythonScriptRunning)
        {
            ExecutionCancelled = true;
            PythonCancellationTokenSource.Cancel();
        }
    }

    private async void ShowLevelFailedDueToRestrictionDialog()
    {
        ContentDialog levelFailedDueToRestrictionsDialog = new();
        levelFailedDueToRestrictionsDialog.XamlRoot = this.Content.XamlRoot;
        levelFailedDueToRestrictionsDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        levelFailedDueToRestrictionsDialog.Title = "Level Failed!";

        levelFailedDueToRestrictionsDialog.IsPrimaryButtonEnabled = false;
        levelFailedDueToRestrictionsDialog.IsSecondaryButtonEnabled = false;
        levelFailedDueToRestrictionsDialog.CloseButtonText = "Close";
        levelFailedDueToRestrictionsDialog.DefaultButton = ContentDialogButton.Close;
        LevelFailedDueToRestrictionsDialog dialogueContent = new();
        levelFailedDueToRestrictionsDialog.Content = dialogueContent;

        _ = await levelFailedDueToRestrictionsDialog.ShowAsync();

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
"\"\"\"" + ")" + Environment.NewLine +
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