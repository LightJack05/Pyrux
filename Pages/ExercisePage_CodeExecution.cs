using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using System.Diagnostics;

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
    private void ArbitraryCodeExecution()
    {
        //TODO: Add a mockup library for the code editor's Autocomplete/Intellisense.
        //TODO: Add non-execution block that is used to import the mockup library that is removed once the file is imported into the program.

        if (!PythonScriptRunning)
        {
            btnStart.IsEnabled = false;
            PythonScriptRunning = true;
            string pythonCode = ActiveLevel.Script;
            ScriptEngine scriptEngine = Python.CreateEngine();
            ScriptScope scriptScope = scriptEngine.CreateScope();
            scriptScope.SetVariable("TurnLeft", () => this.ActiveLevel.TurnLeft());
            scriptScope.SetVariable("TurnRight", () => this.ActiveLevel.TurnRight());
            scriptScope.SetVariable("GoForward", () => this.ActiveLevel.GoForward());
            scriptScope.SetVariable("TakeScrew", () => this.ActiveLevel.TakeScrew());
            scriptScope.SetVariable("PlaceScrew", () => this.ActiveLevel.PlaceScrew());
            scriptScope.SetVariable("WallAhead", () => this.ActiveLevel.WallAhead());
            scriptScope.SetVariable("ScrewThere", () => this.ActiveLevel.ScrewThere());


            Task.Factory.StartNew(() =>
            {
                scriptEngine.Execute(pythonCode, scriptScope);

                ExercisePage.Instance.PythonScriptRunning = false;

                DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
                dispatcherQueue.TryEnqueue(() =>
                {
                    ExercisePage.Instance.btnStart.IsEnabled = true;
                });

            });
        }
    }

}