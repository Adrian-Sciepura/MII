using UnityEngine;

public class DefaultEnemyBehaviourSystem : BehaviourSystem
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        GameEntity playerEntity;

        if (other.tag == "Entity" && (playerEntity = other.GetComponent<GameEntity>()) != null && playerEntity.EntityCategory == GameEntityCategory.Player)
        {
            playerEntity.ReceiveDamage(gameObject, 25);
            AddKnockback(playerEntity.gameObject, 3);
        }
    }
}