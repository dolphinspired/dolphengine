using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine.Graphics
{
    public class Drawer<TContext>
    {
        private readonly List<Action<TContext>> _operations = new List<Action<TContext>>(0);

        public void AddOperation(Action<TContext> operation)
        {
            this._operations.Add(operation);
        }
        
        public void Run(TContext context)
        {
            foreach (var op in this._operations)
            {
                op(context);
            }

            this._operations.Clear();
        }
    }
}
