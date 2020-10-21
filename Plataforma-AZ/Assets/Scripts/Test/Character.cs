using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    public State currentState;
    public State preState;

    private void Start()
    {
        SetState(new TestName(this));
    }
    
    private void Update()
    {
        //currentState.OnStay();
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();
        // colocando o estado anterior
        if (state != currentState)
        {
            preState = currentState;
        }
        // recebendo o novo estado
        currentState = state;

        gameObject.name = "State:" + state.GetType().Name;

        if (currentState != null)
            currentState.OnStateEnter();
    }
}
