using System.Collections.Generic;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Commands
{
    public class UndoRedoManager
    {
        private readonly Stack<IUndoableCommand> undoStack;
        private readonly Stack<IUndoableCommand> redoStack;

        public UndoRedoManager()
        {
            undoStack = new Stack<IUndoableCommand>();
            redoStack = new Stack<IUndoableCommand>();
        }

        public void ExecuteCommand(IUndoableCommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (!CanUndo())
            {
                return;
            }

            IUndoableCommand command = undoStack.Pop();
            command.Unexecute();
            redoStack.Push(command);
        }

        public void Redo()
        {
            if (!CanRedo())
            {
                return;
            }

            IUndoableCommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }

        public bool CanUndo()
        {
            return undoStack.Count > 0;
        }

        public bool CanRedo()
        {
            return redoStack.Count > 0;
        }
    }
}