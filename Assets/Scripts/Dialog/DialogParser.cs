
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Handles the loading of Dialog from a file, parsing, 
/// and branching to named sequences within the file.
/// </summary>
public class DialogParser : MonoBehaviour
{
    public static DialogSequence CurrentDialogSequence {  get; private set; }
    static List<string[]> _sequences = new List<string[]>();
    static int _currentSequence;

    public static void LoadDialog(TextAsset textAsset) { LoadDialog(textAsset.text); }
    public static void LoadDialog(string text)
    {
        CurrentDialogSequence = null;
        string[] tempSequencesArray = text.Split("end_sequence;");
        _sequences.Clear();

        /* Split file contents into list of sequences where  
         * sequences are arrays of commands split by semicolons */
        foreach (string sequence in tempSequencesArray)
        {
            string sequenceString = sequence;
            sequenceString = sequenceString.Trim();
            sequenceString = sequenceString.Replace(System.Environment.NewLine, "");
            string[] commandArray = sequenceString.Split(";");
            _sequences.Add(commandArray);
        }

        /* 
         * NOTE TO SELF:
         *  _sequences is a list of arrays.
         *  Each array is an array of commands
         *  For example, one element of the array may be:
         *  "set_background \"Unicorn/forest_2.png\"" 
         *  
         *  We never want to handle this by itself when
         *  handling dialog reading.
         *  Instead, call DialogParser.Branch(sequence) then use
         *  DialogParser.DequeueCommand() to get the next command.
         */

    }

    /// <summary>
    /// Branches to a named sequence
    /// </summary>
    /// <param name="sequenceName">Target sequence name</param>
    public static void Branch(string sequenceName)
    {
        int targetSequence = GetSequenceIndex(sequenceName);
        CurrentDialogSequence = new DialogSequence(_sequences[targetSequence]);

        //CurrentDialogSequence.DialogCommands.Dequeue();
        //CurrentDialogSequence.DialogCommands.Dequeue();
        //CurrentDialogSequence.DialogCommands.Dequeue();
        //CurrentDialogSequence.DialogCommands.Dequeue();

        //DialogCommand command = CurrentDialogSequence.DialogCommands.Dequeue();
        //Debug.Log("cmd: " + command.Command);
        //foreach (string arg in command.Arguments)
        //{
        //    Debug.Log("arg: " + arg);
        //}

    }

    /// <summary>
    /// Returns the index of a named sequence
    /// </summary>
    /// <param name="sequenceName">Name of target sequence</param>
    /// <returns></returns>
    private static int GetSequenceIndex(string sequenceName)
    {
        for (int i = 0; i < _sequences.Count; i++)
        {
            if (_sequences[i].Contains($"sequence {sequenceName}"))
            {
                //Debug.Log($"Sequence index of {sequenceName} is {i}");
                return i;
            }
        }
        Debug.LogWarning($"Sequence index of {sequenceName} not found!");
        return -1;
    }

    public static DialogCommand DequeueCommand()
    {
        return CurrentDialogSequence.DialogCommands.Dequeue();
    }

}

/// <summary>
/// Contains a queue of DialogCommands. Constructed with an array of commands in the form of strings.
/// </summary>
public class DialogSequence
{
    public Queue<DialogCommand> DialogCommands { get; private set; } = new Queue<DialogCommand>();

    // Constructor creates commands from strings and enqueues them
    public DialogSequence(string[] commandArray)
    {
        foreach (string command in commandArray)
        {
            DialogCommands.Enqueue(new DialogCommand(command));
        }
    }
}

/// <summary>
/// Handles parsing and executing dialog commands.
/// Contains information such as the Command, Arguments and whether it is Valid.
/// Handles the execution of commands through .Execute()
/// </summary>
public class DialogCommand
{
    public string Command { get; private set; }
    public string[] Arguments { get; private set; }
    public bool IsValid { get; private set; }
    public string SequenceName { get; private set; } = null;
    public bool PauseAfterExecution;

    public DialogCommand(string commandText)
    {
        IsValid = false;
        PauseAfterExecution = false;

        if (String.IsNullOrEmpty(commandText)) return;

        // Remove trailing whitespace
        commandText = commandText.Trim();

        // Gets the command name and arguments and stores them
        List<string> splitCommand = ParseCommandArgs(commandText);
        Command = splitCommand[0];
        splitCommand.RemoveAt(0);
        Arguments = splitCommand.ToArray();

        // If sequence declaration, set sequence name
        if (Command == "sequence")
        {
            if (Arguments.Length != 2) { IsValid = false; return; }
            SequenceName = Arguments[1];
        }
        else if (Command == "wait" || Command == "dialog") 
        {
            PauseAfterExecution = true;
        }

        IsValid = true;
    }

    // Regex is evil and confusing    
    private List<string> ParseCommandArgs(string commandText)
    {
        string regexPattern = @"\""((?:\\.|[^""])*)\""|([^\s]+)"; ;
        List<string> splitCommand = new List<string>();
        foreach (Match match in Regex.Matches(commandText, regexPattern))
        {
            if (match.Groups[1].Success)
                splitCommand.Add(match.Groups[1].Value.Replace("\\\"", "\""));
            else
                splitCommand.Add(match.Value);
        }

        return splitCommand;
    }

    /// <summary>
    /// Executes the dialog command
    /// </summary>
    public string[] Execute()
    {
        switch (Command)
        {
            default:
                Debug.LogWarning($"Undefined command - {Command}");
                break;

            case "sequence":
                Debug.Log($"Sequence loaded, {Arguments[0]}");
                break;

            case "dialog":
                Dialog.Instance.PrintDialog(Arguments);
                break;

            case "dialog_continue":
                Dialog.Instance.PrintDialog(Arguments, false);
                break;

            case "character_name":
                if (Arguments.Length != 1)
                {
                    CommandError("Could not set character name. Requires 1 string argument.", Command, Arguments, 1);
                    break;
                }
                Dialog.Instance.SetCharacterName(Arguments[0]); 
                break;

            case "dialog_speed":
                if (float.TryParse(Arguments[0], out float characterIntervalResult))
                {
                    Dialog.Instance.CharacterInterval = characterIntervalResult;
                    break;
                }
                if (Arguments[0] == "default")
                {
                    Dialog.Instance.CharacterInterval = Dialog.Instance.DefaultCharacterInterval;
                    break;
                }
                CommandError("Requires 1 numeric argument or keyword \"default\".", Command, Arguments, 1);
                break;

            case "wait":
                if (float.TryParse(Arguments[0], out float waitResult))
                {
                    Dialog.Instance.Wait(waitResult);
                    break;
                }
                CommandError("Requires 1 numeric argument.", Command, Arguments, 1);
                break;

            case "clear":
                if (Arguments.Length == 0)
                {
                    Dialog.Instance.ClearDialog();
                    break;
                }
                CommandError("Command doesn't take arguments.", Command, Arguments, 0);
                break;

            case "background":
                if (Arguments.Length == 1)
                {
                    Dialog.Instance.SetBackground(Arguments[0]);
                    break;
                }
                if (Arguments.Length == 2 && Arguments[1] == "absolute")
                {
                    Dialog.Instance.SetBackground(Arguments[0], true);
                    break;
                }
                CommandError("Requires a valid path.", Command, Arguments, 0);
                break;

            case "choice":
                if (Arguments.Length > 2)
                {
                    // Form new command to pass into dialog choice button
                    List<string> temp = Arguments.ToList<string>();
                    temp.RemoveAt(0);
                    string choiceCommandStr = string.Empty;
                    foreach (string part in temp) {
                        choiceCommandStr += part + " ";
                    }
                    choiceCommandStr = choiceCommandStr.Substring(0, choiceCommandStr.Length - 1);
                    DialogCommand choiceCommand = new DialogCommand(choiceCommandStr);
                    Dialog.Instance.AddChoice(Arguments[0], choiceCommand);

                    Debug.Log("choiceCommandStr: " + choiceCommandStr);
                }
                CommandError("Please put a command after the choice text argument.", Command, Arguments, 2, 999);
                break;

            case "end_choices":
                if (Arguments.Length == 0)
                {
                    Dialog.Instance.PauseExecution();
                }
                CommandError("End choice doesn't take arguments", Command, Arguments, 0);
                break;

            case "branch":
                if (Arguments.Length == 1) {
                    Dialog.Instance.StartSequence(Arguments[0]);
                }
                CommandError("Branch command requires a sequence name wrapped in \" only.", Command, Arguments, 1);
                break;
        }
        return Arguments;

    }

    private void CommandError(string hint, string command, string[] arguments, int minRequiredArguments, int maxRequiredArguments = -1)
    {
        // Get number of required arguments as string and build error message
        string requiredArguments;
        if (maxRequiredArguments == -1) maxRequiredArguments = minRequiredArguments;
        if (minRequiredArguments == maxRequiredArguments) requiredArguments = minRequiredArguments.ToString();
        else requiredArguments = $"between {minRequiredArguments} and {maxRequiredArguments}";
        string errorMessage = $"Dialog command error ({command}). {hint}";
        if (arguments.Length < minRequiredArguments || arguments.Length > maxRequiredArguments)
        {
            errorMessage += $"Expected {requiredArguments} arguments. Got {arguments.Length} arguments: ";
        }
        foreach (string argument in arguments)
        {
            errorMessage += argument + ", ";
        }

        Debug.LogError(errorMessage);
        Debug.Break();
    }
}