using System.Collections.Generic;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    public class CustomFormAppliedTo
    {
        Dictionary<string, UserState> _AppliedToStates = new Dictionary<string, UserState>();

        public CustomFormAppliedTo()
        {
        }

        public bool IsAppliedToState(string StateName)
        {
            return _AppliedToStates.ContainsKey(StateName);
        }

        public void AddAppliesTo(UserState state, CustomFormItem form)
        {
            if (!_AppliedToStates.ContainsKey(state.Name))
                _AppliedToStates.Add(state.Name, state);
            if (!state.CustomForms.Contains(form))
                state.CustomForms.Add(form);
        }

        public void RemoveAppliesTo(UserState state, CustomFormItem form)
        {
            _AppliedToStates.Remove(state.Name);
            state.CustomForms.Remove(form);
        }
    }
}