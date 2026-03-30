
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

    /// <summary>
    /// Returns DialogCommand object after dequeuing it from command queue
    /// </summary>
    /// <returns>DialogCommand object to be executed</returns>
    public static DialogCommand DequeueCommand()
    {
        return CurrentDialogSequence.DialogCommands.Dequeue();
    }

}

