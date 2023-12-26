using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameEntity>() == null)
            return;

        InteractionSystem inter = new InteractionSystem();
        var dialogue = new DialogueInteractionItem();
        dialogue.message = "ABCD";

        var dialogue2 = new DialogueInteractionItem();
        dialogue2.message = "XYZ";

        inter.content.Add(dialogue);
        inter.content.Add(dialogue2);


        InteractionManager.StartInteraction(inter);
    }
}
