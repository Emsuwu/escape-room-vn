
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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
         *  Sequences is a list of arrays.
         *  Each array is an array of commands
         *  For example, one element of the array may be:
         *  "set_background \"Unicorn/forest_2.png\"" 
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

    public DialogCommand(string commandText)
    {
        IsValid = false;

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

        IsValid = true;
    }

    // Regex is evil and confusing    
    private List<string> ParseCommandArgs(string commandText)
    {
        string regexPattern = @"\""([^""]*)\""|([^\s]+)";
        List<string> splitCommand = new List<string>();
        foreach (Match match in Regex.Matches(commandText, regexPattern))
        {
            if (match.Groups[1].Success)
                splitCommand.Add(match.Groups[1].Value);
            else
                splitCommand.Add(match.Value);
        }

        return splitCommand;
    }

    /// <summary>
    /// Executes the dialog command
    /// </summary>
    public void Execute()
    {
        switch (Command)
        {
            default:
                Debug.LogWarning($"Undefined command - {Command}");
                foreach (string argument in Arguments)
                {
                    Debug.LogWarning($"... arg: {argument}");
                }
                break;
            case "dialog":
                // Push dialog in dialog handler
                break;
        }

    }
}