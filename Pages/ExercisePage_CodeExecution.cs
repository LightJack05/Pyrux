using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using System.Diagnostics;
using Pyrux.UserEndExceptions;

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
            if (thrownException.GetType() == typeof(WallAheadException))
            {
                Debug.WriteLine("The robot hit a wall.");
            }
            else if (thrownException.GetType() == typeof(NoScrewOnTileException))
            {

            }
            else if (thrownException.GetType() == typeof(NoScrewInInventoryException))
            {

            }
        }
    }

    private string BuildPythonCode()
    {
        string pythonCode = "try:" + Environment.NewLine;
        foreach (string line in ActiveLevel.Script.ReplaceLineEndings().Split(Environment.NewLine))
        {
            pythonCode += "    " + line + Environment.NewLine;
        }
        pythonCode +=
"""
except Exception as ex:
    raise ex
""";
        pythonCode = pythonCode.ReplaceLineEndings();
        return pythonCode;
    }

}