using UnityEngine;

namespace RPG.Core
{
    class ActionSceduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (action == currentAction) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction() 
        {
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
        }
    }
}