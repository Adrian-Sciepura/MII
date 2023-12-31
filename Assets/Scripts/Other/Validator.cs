public class Validator
{
    public static bool Validate(EntityPrefabSO entityPrefab)
    {
        if (entityPrefab.entityName == null ||
            entityPrefab.prefab == null ||
            entityPrefab.behaviourSystem == null ||
            entityPrefab.movementSystem == null)
            return false;

        return true;
    }
}