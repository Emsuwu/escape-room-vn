using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


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
        //Debug.Log($"EXECUTING COMMAND {Command}");

        switch (Command)
        {
            default:
                Debug.LogWarning($"Undefined command - {Command}");
                break;

            case "pass":
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
                Debug.Log("Choice args length: " + Arguments.Length);
                if (Arguments.Length > 0)
                {
                    // Form new command to pass into dialog choice button
                    List<string> temp = Arguments.ToList<string>();
                    temp.RemoveAt(0);
                    string choiceCommandStr = string.Empty;
                    foreach (string part in temp)
                    {
                        choiceCommandStr += part + " ";
                    }
                    choiceCommandStr = choiceCommandStr.Substring(0, choiceCommandStr.Length - 1);
                    DialogCommand choiceCommand = new DialogCommand(choiceCommandStr);
                    Dialog.Instance.AddChoice(Arguments[0], choiceCommand);

                    Debug.Log("choiceCommandStr: " + choiceCommandStr);
                    break;
                }
                CommandError("Please put a command after the choice text argument.", Command, Arguments, 1, 999);
                break;

            case "end_choices":
                if (Arguments.Length == 0)
                {
                    Dialog.Instance.PauseExecution();
                    break;
                }
                CommandError("\"end_choice\" doesn't take arguments", Command, Arguments, 0);
                break;

            case "branch":
                if (Arguments.Length == 1)
                {
                    Dialog.Instance.StartSequence(Arguments[0]);
                    break;
                }
                CommandError("\"branch\" command requires a sequence name wrapped in \" only.", Command, Arguments, 1);
                break;

            case "hide_dialog":
                if (Arguments.Length <= 2)
                {
                    if (Arguments.Length == 0)
                    {
                        Dialog.Instance.HideDialog();
                    }
                    else
                    {
                        bool waitUntilFadedToContinue = true;
                        if (Arguments.Length == 2)
                        {
                            if (Arguments[1] == "wait") { waitUntilFadedToContinue = true; }
                            else if (Arguments[1] == "no_wait") { waitUntilFadedToContinue = false; }
                            else
                            {
                                CommandError("\"hide_dialog\" malformed. Syntax: hide_dialog [Fade Time (default 0)] [\"wait\"/\"no_wait\" (default wait)]", Command, Arguments, 0, 2);
                                break;
                            }
                        }
                        if (!float.TryParse(Arguments[0], out float hideTimeResult)) hideTimeResult = 0;
                        Dialog.Instance.HideDialog(hideTimeResult, waitUntilFadedToContinue);
                    }
                    break;
                }
                CommandError("\"hide_dialog\" malformed. Syntax: hide_dialog [Fade Time (default 0)] [\"wait\"/\"no_wait\" (default wait)]", Command, Arguments, 0, 2);
                break;


            case "show_dialog":
                if (Arguments.Length <= 2)
                {
                    if (Arguments.Length == 0)
                    {
                        Dialog.Instance.ShowDialog();
                    }
                    else
                    {
                        bool waitUntilFadedToContinue = true;
                        if (Arguments.Length == 2)
                        {
                            if (Arguments[1] == "wait") { waitUntilFadedToContinue = true; }
                            else if (Arguments[1] == "no_wait") { waitUntilFadedToContinue = false; }
                            else
                            {
                                CommandError("\"show_dialog\" error. Syntax: show_dialog [Fade Time (default 0)] [\"wait\"/\"no_wait\" (default wait)]", Command, Arguments, 0, 2);
                                break;
                            }
                        }

                        if (!float.TryParse(Arguments[0], out float showTimeResult)) showTimeResult = 0;
                        Dialog.Instance.ShowDialog(showTimeResult, waitUntilFadedToContinue);
                    }
                    break;
                }
                CommandError("\"show_dialog\" error. Syntax: show_dialog [Fade Time (default 0)] [\"wait\"/\"no_wait\" (default wait)]", Command, Arguments, 0, 2);
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
        string errorMessage = $"Dialog command error ({command}). {hint} ";
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
