
/// <summary>
/// Customers can give objects to player. This contains both prefab for left and right screen
/// </summary>
[System.Serializable]
public struct FGiveToUser
{
    public InteractableOnTheLeft LeftPrefab;
    public InteractableOnTheRight RightPrefab;

    public FGiveToUser(InteractableOnTheLeft leftPrefab, InteractableOnTheRight rightPrefab)
    {
        LeftPrefab = leftPrefab;
        RightPrefab = rightPrefab;
    }
}
