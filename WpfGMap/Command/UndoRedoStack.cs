using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGMap.Command
{
    class UndoRedoStack<T>
    {
        private Stack<ICommand<T>> undoList;
        private Stack<ICommand<T>> redoList;

        public int UndoCount => undoList.Count;
        public int RedoCount => redoList.Count;

        public UndoRedoStack()
        {
            Reset();
        }
        public void Reset()
        {
            undoList = new Stack<ICommand<T>>();
            redoList = new Stack<ICommand<T>>();
        }

        public T Do(ICommand<T> cmd, T input)
        {
            T output = cmd.Do(input);
            undoList.Push(cmd);
            redoList.Clear(); // Once we issue a new command, the redo stack clears
            return output;
        }
        public T Undo(T input)
        {
            if (undoList.Count > 0)
            {
                ICommand<T> cmd = undoList.Pop();
                T output = cmd.Undo(input);
                redoList.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }
        public T Redo(T input)
        {
            if (redoList.Count > 0)
            {
                ICommand<T> cmd = redoList.Pop();
                T output = cmd.Do(input);
                undoList.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }
    }
}
