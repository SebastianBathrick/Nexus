using System;
using System.Collections.Generic;
using Chow.Execution.Values;

namespace Chow.Execution
{
    class ValueLookupTable
    {
        List<StackFrame> _stackFrames = new();

        public ChowValue GetValue(int varId)
        {
            for (var i = _stackFrames.Count - 1; i >= 0; i--)
            {
                var val = _stackFrames[i].GetValue(varId);

                if (val is not null)
                    return val;
            }

            throw new InvalidOperationException($"Variable with ID {varId} not found");
        }

        public void AddIdentifier(int id)
        {
            if (_stackFrames.Count == 0)
                throw new InvalidOperationException("No scope to add value to");
            
            // The value associated with the identfier gets assigned during a separate instruction
            _stackFrames[^1].AddIdentifier(id);
        }

        public void SetValue(int id, ChowValue val)
        {
            if (_stackFrames.Count == 0)
                throw new InvalidOperationException("No scope to set value for");
            
            for (var i = _stackFrames.Count - 1; i >= 0; i--)
                if (_stackFrames[i].TrySetValue(id, val))
                    return;
            
            throw new InvalidOperationException($"Identifier {id} not found");
        }

        public void EnterScope()
        {
            _stackFrames.Add(new StackFrame());
        }

        public void ExitScope()
        {
            if (_stackFrames.Count == 0)
                throw new InvalidOperationException("No scope to exit");
            
            _stackFrames.RemoveAt(_stackFrames.Count - 1);
        }
    }

    class StackFrame
    {
        List<(int id, ChowValue? val)>? _idValueMap;

        public void AddIdentifier(int id)
        {
            _idValueMap ??= new();
            _idValueMap.Add((id, null));
        }

        public bool TrySetValue(int targId, ChowValue assignedVal)
        {
            // This intentionally overwrites any pre-existing value associated with the target identifier
            if (_idValueMap == null)
                throw new InvalidOperationException("No identifier map to set value for");

            for (var i = 0; i < _idValueMap.Count; i++)
            {
                if (_idValueMap[i].id != targId)
                    continue;

                _idValueMap[i] = (_idValueMap[i].id, assignedVal);
                return true;
            }

            return false;
        }

   public ChowValue? GetValue(int getId)
    {
        if (_idValueMap is null)
            return null;

        for (var i = 0; i < _idValueMap.Count; i++)
        {
            if (_idValueMap[i].id != getId)
                continue;

            if (_idValueMap[i].val is not null)
                return _idValueMap[i].val;

            throw new InvalidOperationException($"Value mapped to identifier {_idValueMap[i].id} is not initialized");
        }

        return null;
    }
    }
}


